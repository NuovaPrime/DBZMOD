using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;
using Terraria.Enums;
using DBZMOD.Util;

namespace DBZMOD.Projectiles
{
    // unabashedly stolen from blushie's laser example, and then customized WIP
	public abstract class BaseBeam : ModProjectile
    {
        // all beams tend to have a similar structure, there's a charge, a tail or "start", a beam (body) and a head (forwardmost point)
        // this is the structure that helps alleviate some of the logic burden by predefining the dimensions of each segment.
        public Point TailOrigin = new Point(14, 0);
        public Point TailSize = new Point(46, 72);
        public Point BeamOrigin = new Point(14, 74);
        public Point BeamSize = new Point(46, 36);
        public Point HeadOrigin = new Point(0, 112);
        public Point HeadSize = new Point(74, 74);
        
        // this determines how long the max fade in for beam opacity takes to fully "phase in", at a rate of 1f per frame. (This is handled by the charge ball)
        public float BeamFadeOutTime = 30f;

        // Bigger number = slower movement. For reference, 60f is pretty fast. 180f is pretty slow.
        public float RotationSlowness = 60f;

        // vector to reposition the beam tail down if it feels too low or too high on the character sprite
        public Vector2 OffsetY = new Vector2(0, -14f);

        // the maximum travel distance the beam can go
        public float MaxBeamDistance = 2000f;

        // the speed at which the beam head travels through space
        public float BeamSpeed = 24f;

        // the type of dust to spawn when the beam is firing
        public int DustType = 169;

        // the frequency at which to spawn dust when the beam is firing
        public float DustFrequency = 0.6f;

        // how many particles per frame fire while firing the beam.
        public int FireParticleDensity = 6;

        // the frequency at which to spawn dust when the beam collides with something
        public float CollisionDustFrequency = 1.0f;

        // how many particles per frame fire when the beam collides with something
        public int CollisionParticleDensity = 8;

        // how many I-Frames your target receives when taking damage from the blast. Take care, this makes beams stupid strong.
        public int ImmunityFrameOverride = 15;

        // Flag for whether the beam segment is animated (meaning it has its own movement protocol), defaults to false.
        public bool IsBeamSegmentAnimated = false;

        // The sound effect used by the projectile when firing the beam. (plays on initial fire only)
        public string BeamSoundKey = "Sounds/BasicBeamFire";

        // The sound slot used by the projectile to kill the sounds it's making
        public KeyValuePair<uint, SoundEffectInstance> BeamSoundSlotId;

        // I'm not sure this ever needs to be changed, but we can always change it later.
        //The distance charge particle from the player center
        public float TailHeldDistance
        {
            get
            {
                return (TailSize.Y / 2f) + 10f;
            }
        }

        // Beam can't be moved when rotating the mouse, it can only stay in its original position
        public bool IsStationaryBeam = false;

        // Beam doesn't penetrate targets until they're dead (it doesn't penetrate at all, really)
        public bool IsEntityColliding = false;
        
        // controls what sections of the beam segment we're drawing at any given point in time (assumes two or more beam segments tile correctly)
        private int BeamSegmentAnimation = 0;

        public Rectangle TailRectangle()
        {
            return new Rectangle(TailOrigin.X, TailOrigin.Y, TailSize.X, TailSize.Y);
        }
                
        public Rectangle BeamRectangle()
        {
            return new Rectangle(BeamOrigin.X, BeamOrigin.Y, BeamSize.X, BeamSize.Y);
        }

        // special handling for segment animation when a beam has animations in the central segment.
        public Rectangle BeamRectangleAnimatedSegment1()
        {
            return new Rectangle(BeamOrigin.X, BeamOrigin.Y + BeamSize.Y - BeamSegmentAnimation, BeamSize.X, BeamSegmentAnimation);
        }

        public Rectangle BeamRectangleAnimatedSegment2()
        {
            return new Rectangle(BeamOrigin.X, BeamOrigin.Y, BeamSize.X, BeamSize.Y - BeamSegmentAnimation);
        }

        public Rectangle HeadRectangle()
        {
            return new Rectangle(HeadOrigin.X, HeadOrigin.Y, HeadSize.X, HeadSize.Y);
        }

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
                projectile.netUpdate = true;
            }
        }

        public float FiringTime
        {
            get
            {
                return projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
                projectile.netUpdate = true;
            }
        }

        public bool IsDetached
        {
            get
            {
                return projectile.localAI[0] > 0f;
            }
        }        

        public float DetachmentTimer
        {
            get
            {
                return projectile.localAI[0];
            }
            set
            {
                projectile.localAI[0] = value;
                projectile.netUpdate = true;
            }
        }

        public float TailDistance
        {
            get
            {
                return projectile.localAI[1];
            }
            set
            {
                projectile.localAI[1] = value;
                projectile.netUpdate = true;
            }
        }
        
        // the length of a "step" of the body, defined loosely as the body's Y length minus an arbitrary cluster of pixels to overlap cleanly.
        public float StepLength()
        {
            return BeamSize.Y - 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = true;
        }

        // doesn't work!
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {            
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Color.White * GetTransparency());
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {               
            // half pi subtracted from the rotation.
            float rotation = projectile.velocity.ToRotation() - 1.57f;
            
            // draw the beam tail
            spriteBatch.Draw(texture, TailPositionStart() - Main.screenPosition, TailRectangle(), color, rotation, new Vector2(TailSize.X * .5f, TailSize.Y * .5f), 1f, 0, 0f);
                        
            // draw the body between the beam and its destination point. We do this in two sections if the beam is "animated"
            for (float i = -1f; i < Distance - StepLength(); i += StepLength())
            {
                Vector2 origin = TailPositionEnd() + i * projectile.velocity;
                
                if (BeamSegmentAnimation > 0)
                {
                    spriteBatch.Draw(texture, origin - Main.screenPosition, BeamRectangleAnimatedSegment1(), color, rotation, new Vector2(BeamSize.X * .5f, BeamSize.Y * .5f), 1f, 0, 0f);
                    spriteBatch.Draw(texture, origin + (BeamSegmentAnimation * projectile.velocity) - Main.screenPosition, BeamRectangleAnimatedSegment2(), color, rotation, new Vector2(BeamSize.X * .5f, BeamSize.Y * .5f), 1f, 0, 0f);
                }
                else
                {
                    spriteBatch.Draw(texture, origin - Main.screenPosition, BeamRectangle(), color, rotation, new Vector2(BeamSize.X * .5f, BeamSize.Y * .5f), 1f, 0, 0f);
                }
            }

            // draw the beam head
            spriteBatch.Draw(texture, BodyPositionEnd() - Main.screenPosition, HeadRectangle(), color, rotation, new Vector2(HeadSize.X * .5f, HeadSize.Y * .5f), 1f, 0, 0f);
        }
        
        public Vector2 TailPositionStart()
        {
            return projectile.position + OffsetY + ((TailHeldDistance + TailDistance) * projectile.velocity);
        }

        public Vector2 TailPositionEnd()
        {
            return TailPositionStart() + (TailSize.Y * projectile.velocity) + ((BeamSize.Y / 2f) - (TailSize.Y / 2f)) * projectile.velocity;
        }

        public Vector2 BodyPositionEnd()
        {
            return TailPositionEnd() + Math.Max(0f, Distance - StepLength()) * projectile.velocity;
        }

        public Vector2 HeadPositionEnd()
        {
            return BodyPositionEnd() + (HeadSize.Y * 0.66f) * projectile.velocity;
        }

        public Vector2 HeadPositionCollisionEnd()
        {
            return BodyPositionEnd() + (HeadSize.Y * 0.2f) * projectile.velocity;
        }

        private const float BEAM_ENTITY_DISTANCE_GRADIENT = 1f;
        // This collision check is for living entities, not tiles. This is what determines damage is being dealt.
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            
            bool isAnyCollision = DoCollisionCheck(targetHitbox);
            if (IsEntityColliding && (isAnyCollision))
            {
                while (DoCollisionCheck(targetHitbox))
                {
                    if (Distance < 0f)
                        break;
                    Distance -= BEAM_ENTITY_DISTANCE_GRADIENT;
                }
            }

            return isAnyCollision;
        }

        public bool DoCollisionCheck(Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            Vector2 unit = projectile.velocity;
            float tailPoint = 0f;
            float bodyPoint = 0f;
            float headPoint = 0f;

            bool tailCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), TailPositionStart(), TailPositionEnd(), TailSize.X, ref tailPoint);

            bool bodyCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), TailPositionEnd(), BodyPositionEnd(), BeamSize.X, ref bodyPoint);

            bool headCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), BodyPositionEnd(), HeadPositionCollisionEnd(), HeadSize.X, ref headPoint);

            bool isAnyCollision = tailCollision || headCollision || bodyCollision;

            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return isAnyCollision;
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.immune[projectile.owner] = ImmunityFrameOverride;            
        }

        public void HandleBeamVisibility()
        {
            var beamTransparency = (int)Math.Min(255f, (1f - (DetachmentTimer / BeamFadeOutTime)) * 255f);
            projectile.alpha = beamTransparency;
        }

        public float GetTransparency()
        {
            return projectile.alpha / 255f;
        }

        // helper field lets us limit mouse movement's impact on the charge ball rotation.
        private Vector2 OldMouseVector = Vector2.Zero;

        // the old screen position helps us offset the MouseWorld vector by our screen position so it's more stable.
        private Vector2 OldScreenPosition = Vector2.Zero;

        // Just fired bool is true the moment the beam comes into existence, to process audio, and then immediately set to false afterwards to prevent sound from looping.
        private bool JustFired = true;

        // used to trakc the original mouse vector for beams that don't track at all.
        private Vector2 OriginalMouseVector = Vector2.Zero;
        private Vector2 OriginalScreenPosition = Vector2.Zero;

        private const float BEAM_TILE_DISTANCE_GRADIENT = 8f;
        // The AI of the projectile
        public override void AI()
        {
            if (DebugUtil.IsSecondElapsed())
                DebugUtil.Log(string.Format("Beam routine says damage is {0}", projectile.damage));
            Player player = Main.player[projectile.owner];

            ProcessKillRoutine(player);

            // stationary beams are instantaneously "detached", they behave weirdly.
            if (IsStationaryBeam && !IsDetached)
            {
                DetachmentTimer = 1;
            }

            // capture the current mouse vector, we're going to normalize movement prior to updating the charge ball location.
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 mouseVector = Main.MouseWorld;

                if (OriginalMouseVector == Vector2.Zero)
                {
                    OriginalMouseVector = mouseVector;
                }

                if (IsStationaryBeam && OriginalMouseVector != Vector2.Zero)
                {
                    mouseVector = OriginalMouseVector;
                }

                Vector2 screenPosition = Main.screenPosition;

                if (OriginalScreenPosition == Vector2.Zero)
                {
                    OriginalScreenPosition = screenPosition;
                }

                if (IsStationaryBeam && OriginalScreenPosition != Vector2.Zero)
                {
                    screenPosition = OriginalScreenPosition;
                }

                if (OldMouseVector != Vector2.Zero && !IsStationaryBeam)
                {
                    Vector2 mouseMovementVector = (mouseVector - OldMouseVector) / RotationSlowness;
                    Vector2 screenChange = screenPosition - OldScreenPosition;
                    mouseVector = OldMouseVector + mouseMovementVector + screenChange;
                }

                UpdateBeamTailLocationAndDirection(player, mouseVector);

                OldMouseVector = mouseVector;

                OldScreenPosition = screenPosition;
            }

            UpdateBeamPlayerItemUse(player);

            // handle whether the beam should be visible, and how visible.
            HandleBeamVisibility();

            // handle the distance routine
            // the difference between distance and tracked distance is that distance is the actual travel.
            // tracked distance is with collision, and resets distance if it's too high.
            Distance += BeamSpeed;
            float TrackedDistance;
            for (TrackedDistance = 0f; TrackedDistance <= MaxBeamDistance; TrackedDistance += BEAM_TILE_DISTANCE_GRADIENT)
            {
                Vector2 origin = TailPositionStart() + projectile.velocity * (TrackedDistance + HeadSize.Y - StepLength());
                
                if (!ProjectileUtil.CanHitLine(TailPositionStart(), origin))
                {
                    // changed to a while loop at a much finer gradient to smooth out beam transitions. Experimental.
                    TrackedDistance -= BEAM_TILE_DISTANCE_GRADIENT;
                    if (TrackedDistance <= 0)
                    {
                        TrackedDistance = 0;
                    }
                    break;
                }
            }

            // handle animation frames on animated beams
            if (IsBeamSegmentAnimated)
            {
                BeamSegmentAnimation += 8;
                if (BeamSegmentAnimation >= StepLength())
                {
                    BeamSegmentAnimation = 0;
                }
            }

            // if distance is about to be throttled, we're hitting something. Spawn some dust.
            if (Distance >= TrackedDistance)
            {
                var dustVector = TailPositionStart() + (TrackedDistance + HeadSize.Y - StepLength()) * projectile.velocity;
                ProjectileUtil.DoBeamCollisionDust(DustType, CollisionDustFrequency, projectile.velocity, dustVector);                    
            }

            // throttle distance by collision
            Distance = Math.Min(TrackedDistance, Distance);

            // shoot sweet sweet particles
            for (var i = 0; i < FireParticleDensity; i++)
            {
                ProjectileUtil.DoBeamDust(projectile.position, projectile.velocity, DustType, DustFrequency, Distance, TailHeldDistance, TailSize.ToVector2(), BeamSpeed);
            }

            // Handle the audio playing, note this positionally tracks at the head position end for effect.
            if (JustFired)
            {
                BeamSoundSlotId = SoundUtil.PlayCustomSound(BeamSoundKey, HeadPositionEnd());
            }

            JustFired = false;

            // Update tracked audio
            SoundUtil.UpdateTrackedSound(BeamSoundSlotId, HeadPositionEnd());

            //Add lights
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - TailHeldDistance), BeamSize.Y, DelegateMethods.CastLight);
        }

        public void UpdateBeamTailLocationAndDirection(Player player, Vector2 mouseVector)
        {
            // server has no business running this code.
            if (Main.netMode == NetmodeID.Server)
                return;

            // Multiplayer support here, only run this code if the client running it is the owner of the projectile
            if (projectile.owner == Main.myPlayer && (!IsDetached || IsStationaryBeam))
            {
                Vector2 diff = mouseVector - projectile.position;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = mouseVector.X > projectile.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
        }

        public void ProcessKillRoutine(Player player)
        {
            projectile.timeLeft = 2;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            if (!modPlayer.IsMouseLeftHeld)
            {
                ProjectileUtil.StartKillRoutine(projectile);
            }

            if (IsDetached && FiringTime == 0)
            {
                DetachmentTimer++;
                TailDistance += BeamSpeed;
                Distance -= BeamSpeed;
            }

            if (FiringTime > 0)
            {
                FiringTime--;
            }

            if (player.dead || DetachmentTimer >= BeamFadeOutTime || (DetachmentTimer > 0 && Distance <= 0))
            {
                projectile.Kill();
            }
        }

        public void UpdateBeamPlayerItemUse(Player player)
        {
            // skip this entire routine if the detachment timer is greater than 0
            if (IsDetached)
                return;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.position = player.Center;
            int dir = projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            if (modPlayer.IsMouseLeftHeld)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(TailPositionStart(), TailPositionStart() + unit * (Distance + HeadSize.Y * 0.66f), (BeamSize.Y) * projectile.scale, DelegateMethods.CutTiles);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
            //damage = GetPlayerKiDamageAfterMultipliers(damage);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            base.ModifyHitPlayer(target, ref damage, ref crit);
            damage = GetPVPDamageReduction(damage);
            //damage = GetPlayerKiDamageAfterMultipliers(damage);
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            base.ModifyHitPvp(target, ref damage, ref crit);
            damage = GetPVPDamageReduction(damage);
            //damage = GetPlayerKiDamageAfterMultipliers(damage);
        }

        public int GetPVPDamageReduction(int damage)
        {
            return (int)Math.Ceiling(damage / 2f);
        }

        public MyPlayer GetPlayerOwner()
        {
            var player = Main.player[projectile.owner];
            if (player != null)
            {
                return player.GetModPlayer<MyPlayer>();
            }
            return null;
        }

        //public int GetPlayerKiDamageAfterMultipliers(int damage)
        //{
        //    if (GetPlayerOwner() == null)
        //        return damage;
        //    float kiMultiplier = GetPlayerOwner().KiDamage;
        //    return (int)Math.Ceiling(damage * kiMultiplier);
        //}
    }
}