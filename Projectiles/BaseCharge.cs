using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;
using Util;
using Terraria.Enums;
using Network;

namespace DBZMOD.Projectiles
{
    // unabashedly stolen from blushie's laser example, and then customized WIP
    public abstract class BaseCharge : ModProjectile
    {
        // CHARGE/BEAM SETTINGS, these are the things you can change to affect charge appearance behavior!

        #region Things you should definitely mess with

        // the maximum charge level of the ball     
        public float ChargeLimit = 4;

        // this is the minimum charge level you have to have before you can actually fire the beam
        public float MinimumChargeLevel = 4f;

        // made to humanize some of the variables in the beam routine and make balance a bit simpler.
        public int ChargeKiDrainPerSecond = 40;

        public int FireKiDrainPerSecond = 80;

        public float FireChargeDrainPerSecond = 1.2f;

        public float ChargeRatePerSecond = 1f;

        public float DecayChargeLevelPerSecond = 1f;

        // a frame timer used to essentially force a beam to be used for a minimum amount of time, preferably long enough for the firing sounds to play.
        public int MinimumFireFrames = 120;

        // this is the beam the charge beam fires when told to.
        public string BeamProjectileName = "BaseBeamProj";

        // this determines how long the max fade in for beam opacity takes to fully "phase in", at a rate of 1f per frame.
        // For the most part, you want to make this the same as the beam's FadeInTime, *unless* you want the beam to stay partially transparent.
        public float BeamFadeInTime = 300f;

        // the type of dust that should spawn when charging or decaying
        public int DustType = 169;

        // the charge ball is just a single texture.
        // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
        public Point ChargeOrigin = new Point(0, 0);
        public Point ChargeSize = new Point(18, 18);

        // The sound effect used by the projectile when charging up.
        public string ChargeSoundKey = "Sounds/EnergyWaveCharge";

        // The amount of delay between when the client will try to play the energy wave charge sound again, if the player stops and resumes charging.
        public int ChargeSoundDelay = 120;

        // EXPERIMENTAL, UNUSED - needs adjustment
        // vector to reposition the charge ball when the player *isn't* charging it (or firing the beam) - held to the side kinda.
        public Vector2 NotChannelingOffset = new Vector2(-15, 20f);

        #endregion

        #region Things you probably should not mess with

        private const float MAX_DISTANCE = 1000f;
        private const float STEP_LENGTH = 10f;

        // vector to reposition the charge ball if it feels too low or too high on the character sprite
        public Vector2 ChannelingOffset = new Vector2(0, 4f);

        // rate at which decaying produces dust
        private float DecayDustFrequency = 0.6f;

        // rate at which charging produces dust
        private float ChargeDustFrequency = 0.4f;

        // rate at which dispersal of the charge ball (from weapon swapping) produces dust
        private float DisperseDustFrequency = 1.0f;

        // the amount of dust that tries to spawn when the charge orb disperses from weapon swapping.
        private int DisperseDustQuantity = 40;

        // Bigger number = slower movement. For reference, 60f is pretty fast. This doesn't have to match the beam speed.
        protected float RotationSlowness = 15f;

        // this is the default cooldown when firing the beam, in frames, before you can fire again, regardless of your charge level.
        protected int InitialBeamCooldown = 180;

        // this field determines whether the beam tracks the player, "rooting" the tail origin to the player.
        protected bool IsBeamOriginTracking = true;

        // the rate at which charge level increases while channeling
        private float ChargeRate() { return ChargeRatePerSecond / 60f; }

        // Rate at which Ki is drained while channeling
        private int ChargeKiDrainRate() { return ChargeKiDrainPerSecond / (60 / CHARGE_KI_DRAIN_WINDOW); }

        // determines the frequency at which ki drain ticks. Bigger numbers mean slower drain.
        private const int CHARGE_KI_DRAIN_WINDOW = 2;

        // Rate at which Ki is drained while firing the beam *without a charge*
        // in theory this should be higher than your charge ki drain, because that's the advantage of charging now.
        private int FireKiDrainRate() { return (int)Math.Ceiling(GetBeamPowerMultiplier() * (FireKiDrainPerSecond / (60f / FIRE_KI_DRAIN_WINDOW))); }

        // determines the frequency at which ki drain ticks while firing. Again, bigger number, slower drain.
        private const int FIRE_KI_DRAIN_WINDOW = 2;

        // the rate at which firing drains the charge level of the ball, play with this for balance.
        private float FireDecayRate() { return GetBeamPowerMultiplier() * FireChargeDrainPerSecond / 60f; }

        // the rate at which the charge decays when not channeling
        private float DecayRate() { return DecayChargeLevelPerSecond / 60f; }

        // The sound slot used by the projectile to kill the sounds it's making
        private KeyValuePair<uint, SoundEffectInstance> ChargeSoundSlotId;

        #endregion

        #region Things you should not mess with

        // the maximum ChargeLimit of the attack after bonuses from gear (etc) are applied
        public float FinalChargeLimit = 4;

        // the distance charge particle from the player center, as a factor of its size plus some padding.
        public float ChargeBallHeldDistance
        {
            get
            {
                return (float)Math.Sqrt(Math.Pow(ChargeSize.X, 2) + Math.Pow(ChargeSize.Y, 2));
            }
        }

        // the amount of time the beam has been firing, used to track whether it has surpassed the minimum fire time the beam *has* to be fired.
        public float CurrentFireTime
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

        // the amount of charge currently in the ball, handles how long the player can fire and how much damage it deals
        public float ChargeLevel
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

        public bool IsSustainingFire
        {
            get
            {
                return CurrentFireTime > 0;
            }
        }

        // Any nonzero number is on cooldown
        public bool IsOnCooldown
        {
            get
            {
                return CurrentFireTime != 0;
            }
        }

        // Any nonzero number is on cooldown
        public float ChargeSoundCooldown
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

        // responsible for tracking if the player changed weapons in use, nullifying their charge immediately.
        private int weaponBinding = -1;
        
        private Rectangle _chargeRectangle;
        public Rectangle ChargeRectangle
        {
            get
            {
                if (_chargeRectangle == null)
                {
                    _chargeRectangle = new Rectangle(ChargeOrigin.X, ChargeOrigin.Y, ChargeSize.X, ChargeSize.Y);
                }
                return _chargeRectangle;
            }
        }

        #endregion

        public override void SetDefaults()
        {
            projectile.width = ChargeSize.X;
            projectile.height = ChargeSize.Y;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
        }

        public float GetTransparency()
        {
            return projectile.alpha / 255f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            // don't draw the ball when firing.
            if (IsSustainingFire)
                return false;
            float originalRotation = -1.57f;
            // float experimentalRotation = 200f;
            DrawChargeBall(spriteBatch, Main.projectileTexture[projectile.type], projectile.damage, originalRotation, 1f, MAX_DISTANCE, Color.White);
            return false;
        }

        public Vector2 GetPlayerHandPosition(Player drawPlayer)
        {
            var Position = drawPlayer.position;
            var handVector = new Vector2((float)((int)(Position.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
            return handVector;
        }

        public Vector2 GetChargeBallPosition()
        {
            Player player = Main.player[projectile.owner];
            Vector2 positionOffset = player.channel ? ChannelingOffset + projectile.velocity * ChargeBallHeldDistance : NotChannelingOffset + GetPlayerHandPosition(player);
            return Main.player[projectile.owner].Center + positionOffset;
        }

        // The core function of drawing a charge ball
        public void DrawChargeBall(SpriteBatch spriteBatch, Texture2D texture, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color))
        {
            float r = projectile.velocity.ToRotation() + rotation;
            spriteBatch.Draw(texture, GetChargeBallPosition() - Main.screenPosition,
                new Rectangle(0, 0, ChargeSize.X, ChargeSize.Y), color * GetTransparency(), r, new Vector2(ChargeSize.X * .5f, ChargeSize.Y * .5f), scale, 0, 0.99f);
        }

        private bool WasCharging = false;

        public void HandleChargingKi(Player player)
        {
            bool IsCharging = false;

            FinalChargeLimit = ChargeLimit + MyPlayer.ModPlayer(player).ChargeLimitAdd;

            // stop channeling if the player is out of ki
            if (MyPlayer.ModPlayer(player).IsKiDepleted())
            {
                player.channel = false;
            }

            // keep alive routine.
            if (projectile.timeLeft < 4)
            {
                projectile.timeLeft = 10;
            }

            MyPlayer modPlayer = MyPlayer.ModPlayer(player);

            // The energy in the projectile decays if the player stops channeling.
            if (!player.channel && !modPlayer.IsMouseRightHeld && !IsSustainingFire)
            {
                // kill the tracked charge sound if the player let go, immediately
                ChargeSoundSlotId = SoundUtil.KillTrackedSound(ChargeSoundSlotId);

                if (ChargeLevel > 0f)
                {
                    ChargeLevel = Math.Max(0, ChargeLevel - DecayRate());

                    // don't draw the ball when firing.
                    if (!IsSustainingFire)
                        ProjectileUtil.DoChargeDust(GetChargeBallPosition(), DustType, DecayDustFrequency, true, ChargeSize.ToVector2());
                }
                else
                {
                    // the charge level zeroed out, kill the projectile.
                    projectile.Kill();
                }
            }

            // charge the ball if the proper keys are held.
            // increment the charge timer if channeling and apply slowdown effect
            if (player.channel && projectile.active && modPlayer.IsMouseRightHeld && !IsSustainingFire)
            {
                // the player can hold the charge all they like once it's fully charged up. Currently this doesn't incur a movespeed debuff either.
                if (ChargeLevel < FinalChargeLimit && !modPlayer.IsKiDepleted())
                {
                    IsCharging = true;

                    // drain ki from the player when charging
                    if (Main.time > 0 && Math.Ceiling(Main.time % CHARGE_KI_DRAIN_WINDOW) == 0)
                    {
                        MyPlayer.ModPlayer(player).AddKi(-ChargeKiDrainRate());
                    }

                    // increase the charge
                    ChargeLevel = Math.Min(FinalChargeLimit, ChargeRate() + ChargeLevel);

                    // slow down the player while charging.
                    ProjectileUtil.ApplyChannelingSlowdown(player);

                    // shoot some dust into the ball to show it's charging, and to look cool.
                    if (!IsSustainingFire)
                        ProjectileUtil.DoChargeDust(GetChargeBallPosition(), DustType, ChargeDustFrequency, false, ChargeSize.ToVector2());
                }
            }

            // play the sound if the player just started charging and the audio is "off cooldown"
            if (!WasCharging && IsCharging && ChargeSoundCooldown == 0f)
            {
                if (!Main.dedServ)
                    ChargeSoundSlotId = SoundUtil.PlayCustomSound(ChargeSoundKey, projectile.Center);
                ChargeSoundCooldown = ChargeSoundDelay;
            }
            else
            {
                ChargeSoundCooldown = Math.Max(0f, ChargeSoundCooldown - 1);
            }

            // set the wasCharging flag for proper tracking
            WasCharging = IsCharging;
        }

        private bool ShouldFireBeam(MyPlayer modPlayer)
        {
            // DebugUtil.Log(string.Format("Charge level: {0} Cooldown? {1} SustainingFire? {2} FireTime: {3} MouseLeftHeld {4}", ChargeLevel, IsOnCooldown, IsSustainingFire, CurrentFireTime, modPlayer.IsMouseLeftHeld));
            return ((ChargeLevel >= MinimumChargeLevel && !IsOnCooldown) || IsSustainingFire)
                && (modPlayer.IsMouseLeftHeld || (IsSustainingFire && (CurrentFireTime > 0 && CurrentFireTime < MinimumFireFrames)));
        }

        private float GetBeamPowerMultiplier()
        {
            return (1f + ChargeLevel / 10f);
        }

        private int GetBeamDamage()
        {
            return (int)Math.Ceiling(projectile.damage * GetBeamPowerMultiplier());
        }

        private bool WasSustainingFire = false;

        Projectile MyProjectile = null;
        public void HandleFiring(Player player, Vector2 mouseVector)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            // minimum charge level is required to fire in the first place, but once you fire, you can keep firing.
            if (ShouldFireBeam(modPlayer))
            {
                // force the mouse state - this indicates that the player hasn't achieved the minimum fire time set on the beam; we should treat it like it's still firing so it renders.
                if (!modPlayer.IsMouseLeftHeld && IsBeamOriginTracking)
                {
                    modPlayer.IsMouseLeftHeld = true;
                }

                // kill the charge sound if we're firing
                ChargeSoundSlotId = SoundUtil.KillTrackedSound(ChargeSoundSlotId);

                if (!WasSustainingFire)
                {
                    // fire the laser!
                    MyProjectile = Projectile.NewProjectileDirect(projectile.position, projectile.velocity, mod.ProjectileType(BeamProjectileName), GetBeamDamage(), projectile.knockBack, projectile.owner);

                    // set firing time minimum for beams that auto-detach and are stationary, this prevents their self kill routine
                    MyProjectile.ai[1] = MinimumFireFrames;
                }

                // increment the fire time, this handles "IsSustainingFire" as well as stating the beam is no longer firable (it is already being fired)
                CurrentFireTime++;

                // if the player has charge left, drain the ball
                if (ChargeLevel > 0f)
                {
                    ChargeLevel = Math.Max(0f, ChargeLevel - FireDecayRate());
                }
                else if (!modPlayer.IsKiDepleted())
                {
                    if (Main.time > 0 && Main.time % FIRE_KI_DRAIN_WINDOW == 0)
                    {
                        modPlayer.AddKi(-FireKiDrainRate());
                    }
                }
                else
                {
                    // beam is no longer sustainable
                    KillBeam();
                }
            }
            else
            {
                // player has stopped firing or something else has stopped them
                KillBeam();
            }

            WasSustainingFire = IsSustainingFire;
        }

        public void KillBeam()
        {
            // set the cooldown
            CurrentFireTime = -InitialBeamCooldown;

            if (MyProjectile != null)
            {
                ProjectileUtil.StartKillRoutine(MyProjectile);
            }
        }

        public void HandleChargeBallVisibility()
        {
            var chargeVisibility = (int)Math.Ceiling((Math.Sqrt(ChargeLevel) / Math.Sqrt(FinalChargeLimit)) * 255f);
            projectile.alpha = chargeVisibility;
            projectile.hide = ChargeLevel <= 0f;
        }

        public bool ShouldHandleWeaponChangesAndContinue(Player player)
        {
            if (player.HeldItem == null)
            {
                if (MyProjectile != null)
                {
                    ProjectileUtil.StartKillRoutine(MyProjectile);
                }
                projectile.Kill();
                return false;
            }

            if (weaponBinding == -1)
            {
                weaponBinding = player.HeldItem.type;
            }
            else
            {
                if (player.HeldItem.type != weaponBinding)
                {
                    // do a buttload of decay dust
                    for (var i = 0; i < DisperseDustQuantity; i++)
                    {
                        ProjectileUtil.DoChargeDust(GetChargeBallPosition(), DustType, DisperseDustFrequency, true, ChargeSize.ToVector2());
                    }
                    if (MyProjectile != null)
                    {
                        ProjectileUtil.StartKillRoutine(MyProjectile);
                    }
                    projectile.Kill();
                    return false;
                }
            }
            // weapon's correct, keep doing what you're doing.
            return true;
        }

        // helper field lets us limit mouse movement's impact on the charge ball rotation.
        private Vector2 OldMouseVector = Vector2.Zero;

        // the old screen position helps us offset the MouseWorld vector by our screen position so it's more stable.
        private Vector2 OldScreenPosition = Vector2.Zero;

        // The AI of the projectile
        public override void AI()
        {
            // capture the current mouse vector, we're going to normalize movement prior to updating the charge ball location.
            Vector2 mouseVector = Main.MouseWorld;
            Vector2 screenPosition = Main.screenPosition;
            if (OldMouseVector != Vector2.Zero)
            {
                Vector2 mouseMovementVector = (mouseVector - OldMouseVector) / RotationSlowness;
                Vector2 screenChange = screenPosition - OldScreenPosition;
                mouseVector = OldMouseVector + mouseMovementVector + screenChange;
            }

            // capture the player instance so we can toss it around.
            Player player = Main.player[projectile.owner];

            // track whether charge level has changed by snapshotting it.
            float oldChargeLevel = ChargeLevel;

            // handles the initial binding to a weapon and determines if the player has changed items, which should kill the projectile.
            if (!ShouldHandleWeaponChangesAndContinue(player))
                return;

            // decrease the beam cooldown if it's not zero
            HandleBeamFireCooldown();

            // handle.. handling the charge.
            UpdateChargeBallLocationAndDirection(player, mouseVector);

            // handle pouring ki into the ball (or decaying if not channeling)
            HandleChargingKi(player);

            // handle whether the ball should be visible, and how visible.
            HandleChargeBallVisibility();

            // figure out if the player is shooting and fire the laser
            HandleFiring(player, mouseVector);

            // Handle Audio
            SoundUtil.UpdateTrackedSound(ChargeSoundSlotId, projectile.Center);

            // capture the current mouse vector as the previous mouse vector.
            OldMouseVector = mouseVector;

            // capture the current screen position as the previous screen position.
            OldScreenPosition = screenPosition;

            // If we just crossed a threshold, display combat text for the charge level increase.
            if (Math.Floor(oldChargeLevel) != Math.Floor(ChargeLevel) && oldChargeLevel != ChargeLevel)
            {
                Color chargeColor = oldChargeLevel < ChargeLevel ? new Color(51, 224, 255) : new Color(251, 74, 55);
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), chargeColor, (int)ChargeLevel, false, false);
            }
        }

        public void HandleBeamFireCooldown()
        {
            // less than 0 fire time means on cooldown, try to "decrease" cooldown by 1, stopping at 0 if applicable.
            if (CurrentFireTime < 0)
                CurrentFireTime = Math.Max(0, CurrentFireTime + 1f);
        }

        public void UpdateChargeBallLocationAndDirection(Player player, Vector2 mouseVector)
        {
            // custom channeling handler
            if (player.GetModPlayer<MyPlayer>().IsMouseRightHeld)
                player.channel = true;

            // Multiplayer support here, only run this code if the client running it is the owner of the projectile
            if (projectile.owner == Main.myPlayer)
            {
                if (player.heldProj != projectile.whoAmI)
                {
                    player.heldProj = projectile.whoAmI;

                    // unsure if this is even necessary, syncing held projectile
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetworkHelper.playerSync.SendChangedHeldProjectile(256, player.whoAmI, player.whoAmI, projectile.whoAmI);
                }
                Vector2 diff = mouseVector - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = mouseVector.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
            projectile.position = player.Center - new Vector2(0, ChargeSize.Y / 2f) + projectile.velocity * ChargeBallHeldDistance;
            projectile.timeLeft = 10;
            if (player.channel)
            {
                player.itemTime = 10;
                player.itemAnimation = 10;
                int dir = projectile.direction;
                player.ChangeDir(dir);
                player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}