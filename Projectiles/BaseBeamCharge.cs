using System;
using Microsoft.Xna.Framework;
using Terraria;
using DBZMOD.Util;

namespace DBZMOD.Projectiles
{
    // unabashedly stolen from blushie's laser example, and then customized WIP
    public abstract class BaseBeamCharge : AbstractChargeBall
    {
        // used to offset the beam so that its origin is different from the charge ball, used for special instances like the Makankosappo
        public Vector2 BeamCreationOffset = Vector2.Zero;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        private bool ShouldFireBeam(MyPlayer modPlayer)
        {
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

        // multiplier representing increased ki cost gradient as the player continues to fire the beam, to put the kibosh on firing infinitely.
        private float KiDrainMultiplier()
        {
            return 1f + Math.Max(0f, ((CurrentFireTime - MinimumFireFrames) / MinimumFireFrames));
        }

        // Rate at which Ki is drained while firing the beam *without a charge*
        // in theory this should be higher than your charge ki drain, because that's the advantage of charging now.
        protected int FireKiDrainRate() { return (int)Math.Ceiling(GetBeamPowerMultiplier() * KiDrainMultiplier() * (ChargeKiDrainPerSecond * 2f / (60f / FIRE_KI_DRAIN_WINDOW))); }

        // the rate at which firing drains the charge level of the ball, play with this for balance.
        protected float FireDecayRate() { return GetBeamPowerMultiplier() * FireChargeDrainPerSecond / 60f; }

        public Projectile MyProjectile = null;
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
                    MyProjectile = Projectile.NewProjectileDirect(projectile.position + BeamCreationOffset, projectile.velocity, mod.ProjectileType(BeamProjectileName), GetBeamDamage(), projectile.knockBack, projectile.owner);                                        

                    // set firing time minimum for beams that auto-detach and are stationary, this prevents their self kill routine
                    MyProjectile.ai[1] = MinimumFireFrames;
                }

                // increment the fire time, this handles "IsSustainingFire" as well as stating the beam is no longer firable (it is already being fired)
                CurrentFireTime++;

                // if the player has charge left, drain the ball
                if (ChargeLevel >= FireDecayRate())
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

        public override bool PreAI()
        {
            // pre AI of the charge ball is responsible for telling us if the weapon has changed or the projectile should otherwise die

            bool isPassingPreAI = base.PreAI();
            if (!isPassingPreAI && MyProjectile != null)
            {
                ProjectileUtil.StartKillRoutine(MyProjectile);
            }
            return isPassingPreAI;
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

        public override Vector2 DoControl(Player player)
        {
            // calls the base do control, which handles some of the rotation code and other complicated nonsense.
            var mouseVector = base.DoControl(player);

            // figure out if the player is shooting and fire the laser
            HandleFiring(player, mouseVector);

            // decrease the beam cooldown if it's not zero
            HandleBeamFireCooldown();

            return mouseVector;
        }

        public void HandleBeamFireCooldown()
        {
            // less than 0 fire time means on cooldown, try to "decrease" cooldown by 1, stopping at 0 if applicable.
            if (CurrentFireTime < 0)
                CurrentFireTime = Math.Max(0, CurrentFireTime + 1f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}