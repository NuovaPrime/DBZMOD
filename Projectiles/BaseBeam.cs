using System;
using System.Collections.Generic;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    // unabashedly stolen from blushie's laser example, and then customized WIP
	public abstract class BaseBeam : ModProjectile
    {
        // all beams tend to have a similar structure, there's a charge, a tail or "start", a beam (body) and a head (forwardmost point)
        // this is the structure that helps alleviate some of the logic burden by predefining the dimensions of each segment.
        protected Point 
            tailOrigin = new Point(14, 0),
            tailSize = new Point(46, 72),
            beamOrigin = new Point(14, 74),
            beamSize = new Point(46, 36),
            headOrigin = new Point(0, 112),
            headSize = new Point(74, 74);
        
        // this determines how long the max fade in for beam opacity takes to fully "phase in", at a rate of 1f per frame. (This is handled by the charge ball)
        public float beamFadeOutTime = 30f;

        // Bigger number = slower movement. For reference, 60f is pretty fast. 180f is pretty slow.
        public float rotationSlowness = 60f;

        // vector to reposition the beam tail down if it feels too low or too high on the character sprite
        public Vector2 offsetY = new Vector2(0, -14f);

        // the maximum travel distance the beam can go
        public float maxBeamDistance = 2000f;

        // the speed at which the beam head travels through space
        public float beamSpeed = 24f;

        // the type of dust to spawn when the beam is firing
        public int dustType = 169;

        // the frequency at which to spawn dust when the beam is firing
        public float dustFrequency = 0.6f;

        // how many particles per frame fire while firing the beam.
        public int fireParticleDensity = 6;

        // the frequency at which to spawn dust when the beam collides with something
        public float collisionDustFrequency = 1.0f;

        // how many particles per frame fire when the beam collides with something
        public int collisionParticleDensity = 8;

        // how many I-Frames your target receives when taking damage from the blast. Take care, this makes beams stupid strong.
        public int immunityFrameOverride = 5;

        // Flag for whether the beam segment is animated (meaning it has its own movement protocol), defaults to false.
        public bool isBeamSegmentAnimated = false;

        // The sound effect used by the projectile when firing the beam. (plays on initial fire only)
        public string beamSoundKey = "Sounds/BasicBeamFire";

        // The sound slot used by the projectile to kill the sounds it's making
        public KeyValuePair<uint, SoundEffectInstance> beamSoundSlotId;

        // I'm not sure this ever needs to be changed, but we can always change it later.
        //The distance charge particle from the player center
        public float TailHeldDistance
        {
            get
            {
                return (tailSize.Y / 2f) + 10f;
            }
        }

        // Beam can't be moved when rotating the mouse, it can only stay in its original position
        public bool isStationaryBeam = false;

        // Beam doesn't penetrate targets until they're dead (it doesn't penetrate at all, really)
        public bool isEntityColliding = false;
        
        // controls what sections of the beam segment we're drawing at any given point in time (assumes two or more beam segments tile correctly)
        private int _beamSegmentAnimation = 0;

        public Rectangle TailRectangle()
        {
            return new Rectangle(tailOrigin.X, tailOrigin.Y, tailSize.X, tailSize.Y);
        }
                
        public Rectangle BeamRectangle()
        {
            return new Rectangle(beamOrigin.X, beamOrigin.Y, beamSize.X, beamSize.Y);
        }

        // special handling for segment animation when a beam has animations in the central segment.
        public Rectangle BeamRectangleAnimatedSegment1()
        {
            return new Rectangle(beamOrigin.X, beamOrigin.Y + beamSize.Y - _beamSegmentAnimation, beamSize.X, _beamSegmentAnimation);
        }

        public Rectangle BeamRectangleAnimatedSegment2()
        {
            return new Rectangle(beamOrigin.X, beamOrigin.Y, beamSize.X, beamSize.Y - _beamSegmentAnimation);
        }

        public Rectangle HeadRectangle()
        {
            return new Rectangle(headOrigin.X, headOrigin.Y, headSize.X, headSize.Y);
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
        
        // the length of a "step" of the body, defined loosely as the body's Y length minus an arbitrary cluster of pixels to overlap cleanly.
        public float StepLength()
        {
            return (beamSize.Y - 1) * projectile.scale;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {            
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Color.White, projectile.scale);
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Color color, float scale = 1f)
        {               
            // half pi subtracted from the rotation.
            float rotation = projectile.velocity.ToRotation() - 1.57f;
            
            // draw the beam tail
            spriteBatch.Draw(texture, TailPositionStart() - Main.screenPosition, TailRectangle(), color, rotation, new Vector2(tailSize.X * .5f, tailSize.Y * .5f), scale, 0, 0f);
                        
            // draw the body between the beam and its destination point. We do this in two sections if the beam is "animated"
            for (float i = -1f; i < Distance - StepLength(); i += StepLength())
            {
                Vector2 origin = TailPositionEnd() + i * projectile.velocity;
                
                if (_beamSegmentAnimation > 0)
                {
                    spriteBatch.Draw(texture, origin - Main.screenPosition, BeamRectangleAnimatedSegment1(), color, rotation, new Vector2(beamSize.X * .5f, beamSize.Y * .5f), scale, 0, 0f);
                    spriteBatch.Draw(texture, origin + (_beamSegmentAnimation * projectile.velocity) - Main.screenPosition, BeamRectangleAnimatedSegment2(), color, rotation, new Vector2(beamSize.X * .5f, beamSize.Y * .5f), scale, 0, 0f);
                }
                else
                {
                    spriteBatch.Draw(texture, origin - Main.screenPosition, BeamRectangle(), color, rotation, new Vector2(beamSize.X * .5f, beamSize.Y * .5f), scale, 0, 0f);
                }
            }

            // draw the beam head
            spriteBatch.Draw(texture, BodyPositionEnd() - Main.screenPosition, HeadRectangle(), color, rotation, new Vector2(headSize.X * .5f, headSize.Y * .5f), scale, 0, 0f);
        }

        public Vector2 TailPositionCollisionStart()
        {
            return projectile.position + offsetY + 16f  * projectile.velocity;
        }

        public Vector2 TailPositionStart()
        {
            return projectile.position + offsetY + (TailHeldDistance * projectile.scale * projectile.velocity);
        }

        public Vector2 TailPositionEnd()
        {
            return TailPositionStart() + (tailSize.Y * projectile.scale * projectile.velocity) + ((beamSize.Y / 2f) - (tailSize.Y / 2f)) * projectile.scale * projectile.velocity;
        }

        public Vector2 BodyPositionEnd()
        {
            return TailPositionEnd() + Math.Max(0f, Distance - StepLength()) * projectile.scale * projectile.velocity;
        }

        public Vector2 HeadPositionEnd()
        {
            return BodyPositionEnd() + (headSize.Y * 0.66f) * projectile.scale * projectile.velocity;
        }

        public Vector2 HeadPositionCollisionEnd()
        {
            return BodyPositionEnd() + (headSize.Y * 0.2f) * projectile.scale * projectile.velocity;
        }

        private const float BEAM_ENTITY_DISTANCE_GRADIENT = 1f;
        // Alceris hack, since collision is already handled outside of the Collider now, this always returns true
        // and gets trumped by whatever is returned from CanHit methods.
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            bool isAnyCollision = DoCollisionCheck(target.Hitbox) && !target.dontTakeDamage && !target.friendly;
            if (isAnyCollision && !IsDetached && isEntityColliding)
            {
                while (DoCollisionCheck(target.Hitbox))
                {
                    if (Distance < 0f)
                        break;
                    Distance -= BEAM_ENTITY_DISTANCE_GRADIENT;
                }
            }

            if (target.immune[projectile.owner] > 0) return false;
            return isAnyCollision;
        }

        public override bool CanHitPlayer(Player target)
        {
            bool isAnyCollision = DoCollisionCheck(target.Hitbox) && !target.immune;
            if (isAnyCollision && !IsDetached && isEntityColliding)
            {
                while (DoCollisionCheck(target.Hitbox))
                {
                    if (Distance < 0f)
                        break;
                    Distance -= BEAM_ENTITY_DISTANCE_GRADIENT;
                }
            }

            return isAnyCollision;
        }

        public override bool CanHitPvp(Player target)
        {            
            bool isAnyCollision = DoCollisionCheck(target.Hitbox) && target.hostile && target.team != Main.player[projectile.owner].team;
            if (isAnyCollision && !IsDetached && isEntityColliding)
            {
                while (DoCollisionCheck(target.Hitbox))
                {
                    if (Distance < 0f)
                        break;
                    Distance -= BEAM_ENTITY_DISTANCE_GRADIENT;
                }
            }

            if (target.immune) return false;
            return isAnyCollision;
        }

        public bool DoCollisionCheck(Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            Vector2 unit = projectile.velocity;
            float tailPoint = 0f;
            float bodyPoint = 0f;
            float headPoint = 0f;

            bool tailCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), TailPositionCollisionStart(), TailPositionEnd(), tailSize.X, ref tailPoint);

            bool bodyCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), TailPositionEnd(), BodyPositionEnd(), beamSize.X, ref bodyPoint);

            bool headCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), BodyPositionEnd(), HeadPositionCollisionEnd(), headSize.X, ref headPoint);

            bool isAnyCollision = tailCollision || headCollision || bodyCollision;

            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return isAnyCollision;
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.immune[projectile.owner] = immunityFrameOverride;            
        }

        // helper field lets us limit mouse movement's impact on the charge ball rotation.
        private Vector2 _oldMouseVector = Vector2.Zero;

        // the old screen position helps us offset the MouseWorld vector by our screen position so it's more stable.
        private Vector2 _oldScreenPosition = Vector2.Zero;

        // Just fired bool is true the moment the beam comes into existence, to process audio, and then immediately set to false afterwards to prevent sound from looping.
        private bool _justFired = true;

        // used to trakc the original mouse vector for beams that don't track at all.
        private Vector2 _originalMouseVector = Vector2.Zero;
        private Vector2 _originalScreenPosition = Vector2.Zero;

        private const float BEAM_TILE_DISTANCE_GRADIENT = 8f;
        // The AI of the projectile
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            ProcessKillRoutine(player);

            // stationary beams are instantaneously "detached", they behave weirdly.
            if (isStationaryBeam && !IsDetached)
            {
                DetachmentTimer = 1;
            }

            // capture the current mouse vector, we're going to normalize movement prior to updating the charge ball location.
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 mouseVector = Main.MouseWorld;

                if (_originalMouseVector == Vector2.Zero)
                {
                    _originalMouseVector = mouseVector;
                }

                if (isStationaryBeam && _originalMouseVector != Vector2.Zero)
                {
                    mouseVector = _originalMouseVector;
                }

                Vector2 screenPosition = Main.screenPosition;

                if (_originalScreenPosition == Vector2.Zero)
                {
                    _originalScreenPosition = screenPosition;
                }

                if (isStationaryBeam && _originalScreenPosition != Vector2.Zero)
                {
                    screenPosition = _originalScreenPosition;
                }

                if (_oldMouseVector != Vector2.Zero && !isStationaryBeam)
                {
                    Vector2 mouseMovementVector = (mouseVector - _oldMouseVector) / rotationSlowness;
                    Vector2 screenChange = screenPosition - _oldScreenPosition;
                    mouseVector = _oldMouseVector + mouseMovementVector + screenChange;
                }

                UpdateBeamTailLocationAndDirection(player, mouseVector);

                _oldMouseVector = mouseVector;

                _oldScreenPosition = screenPosition;
            }

            UpdateBeamPlayerItemUse(player);

            // handle the distance routine
            // the difference between distance and tracked distance is that distance is the actual travel.
            // tracked distance is with collision, and resets distance if it's too high.
            Distance += beamSpeed;
            float trackedDistance;
            for (trackedDistance = 0f; trackedDistance <= maxBeamDistance; trackedDistance += BEAM_TILE_DISTANCE_GRADIENT)
            {
                Vector2 origin = TailPositionStart() + projectile.velocity * (trackedDistance + headSize.Y * projectile.scale - StepLength());
                
                if (!ProjectileHelper.CanHitLine(TailPositionStart(), origin))
                {
                    // changed to a while loop at a much finer gradient to smooth out beam transitions. Experimental.
                    trackedDistance -= BEAM_TILE_DISTANCE_GRADIENT;
                    if (trackedDistance <= 0)
                    {
                        trackedDistance = 0;
                    }
                    break;
                }
            }

            // handle animation frames on animated beams
            if (isBeamSegmentAnimated)
            {
                _beamSegmentAnimation += 8;
                if (_beamSegmentAnimation >= StepLength())
                {
                    _beamSegmentAnimation = 0;
                }
            }

            // if distance is about to be throttled, we're hitting something. Spawn some dust.
            if (Distance >= trackedDistance)
            {
                var dustVector = TailPositionStart() + (trackedDistance + headSize.Y - StepLength()) * projectile.velocity;
                ProjectileHelper.DoBeamCollisionDust(dustType, collisionDustFrequency, projectile.velocity, dustVector);                    
            }

            // throttle distance by collision
            Distance = Math.Min(trackedDistance, Distance);

            // shoot sweet sweet particles
            for (var i = 0; i < fireParticleDensity; i++)
            {
                ProjectileHelper.DoBeamDust(projectile.position, projectile.velocity, dustType, dustFrequency, Distance, TailHeldDistance, tailSize.ToVector2(), beamSpeed);
            }

            // Handle the audio playing, note this positionally tracks at the head position end for effect.
            if (_justFired)
            {
                beamSoundSlotId = SoundHelper.PlayCustomSound(beamSoundKey, HeadPositionEnd());
            }

            _justFired = false;

            // Update tracked audio
            SoundHelper.UpdateTrackedSound(beamSoundSlotId, HeadPositionEnd());

            //Add lights
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - TailHeldDistance), beamSize.Y, DelegateMethods.CastLight);
        }

        public void UpdateBeamTailLocationAndDirection(Player player, Vector2 mouseVector)
        {
            // server has no business running this code.
            if (Main.netMode == NetmodeID.Server)
                return;

            // Multiplayer support here, only run this code if the client running it is the owner of the projectile
            if (projectile.owner == Main.myPlayer && (!IsDetached || isStationaryBeam))
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

            if (!modPlayer.isMouseLeftHeld)
            {
                ProjectileHelper.StartKillRoutine(projectile);
            }

            if (IsDetached && FiringTime == 0)
            {
                DetachmentTimer++;
                projectile.scale /= 1.1f;
            }

            if (FiringTime > 0)
            {
                FiringTime--;
            }

            if (player.dead || DetachmentTimer >= beamFadeOutTime || (DetachmentTimer > 0 && Distance <= 0))
            {
                projectile.Kill();
            }
        }

        // helper flag  used to set the initial position of a detached beam once and only once before removing it from the player "anchor point"
        private bool _isAttachedOnce = false;
        public void UpdateBeamPlayerItemUse(Player player)
        {
            // skip this entire routine if the detachment timer is greater than 0
            if (IsDetached)
            {
                if (!_isAttachedOnce)
                {
                    projectile.position = player.Center;
                    _isAttachedOnce = true;
                }
                return;
            }

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.position = player.Center;
            int dir = projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            if (modPlayer.isMouseLeftHeld)
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
            Utils.PlotTileLine(TailPositionStart(), TailPositionStart() + unit * (Distance + headSize.Y * 0.66f), (beamSize.Y) * projectile.scale, DelegateMethods.CutTiles);
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
            damage = GetPvpDamageReduction(damage);
            //damage = GetPlayerKiDamageAfterMultipliers(damage);
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            base.ModifyHitPvp(target, ref damage, ref crit);
            damage = GetPvpDamageReduction(damage);
            //damage = GetPlayerKiDamageAfterMultipliers(damage);
        }

        public int GetPvpDamageReduction(int damage)
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