using System;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Projectiles
{
    // unabashedly stolen from blushie's laser example, and then customized WIP
    public abstract class BaseBeamCharge : AbstractChargeBall
    {
        // used to offset the beam so that its origin is different from the charge ball, used for special instances like the Makankosappo
        public Vector2 beamCreationOffset = Vector2.Zero;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        private bool ShouldFireBeam(MyPlayer modPlayer)
        {
            var shouldFire = ((ChargeLevel >= minimumChargeLevel && !IsOnCooldown) || IsSustainingFire)
                && (modPlayer.isMouseLeftHeld || (IsSustainingFire && (CurrentFireTime > 0 && CurrentFireTime < minimumFireFrames)));
            return shouldFire;
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
        private float KiDrainMultiplier(MyPlayer modPlayer)
        {
            return (1f + Math.Min(3.0f, Math.Max(0f, (CurrentFireTime - minimumFireFrames) / minimumFireFrames))) * modPlayer.kiDrainMulti;
        }

        // Rate at which Ki is drained while firing the beam *without a charge*
        // in theory this should be higher than your charge ki drain, because that's the advantage of charging now.
        protected int FireKiDrainRate(MyPlayer modPlayer) { return (int)Math.Ceiling(GetBeamPowerMultiplier() * KiDrainMultiplier(modPlayer) * (chargeKiDrainPerSecond * 2f / (60f / FIRE_KI_DRAIN_WINDOW))); }

        // the rate at which firing drains the charge level of the ball, play with this for balance.
        protected float FireDecayRate() { return GetBeamPowerMultiplier() * fireChargeDrainPerSecond / 60f; }

        public Projectile myProjectile = null;

        public void HandleFiring(Player player, Vector2 mouseVector)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            // minimum charge level is required to fire in the first place, but once you fire, you can keep firing.
            if (ShouldFireBeam(modPlayer))
            {
                // kill the charge sound if we're firing
                chargeSoundSlotId = SoundHelper.KillTrackedSound(chargeSoundSlotId);

                if (!wasSustainingFire && myProjectile == null)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient || Main.myPlayer == player.whoAmI)
                    {
                        // fire the laser!
                        myProjectile = Projectile.NewProjectileDirect(projectile.position + beamCreationOffset, projectile.velocity, mod.ProjectileType(beamProjectileName), GetBeamDamage(), projectile.knockBack, projectile.owner);

                        // set firing time minimum for beams that auto-detach and are stationary, this prevents their self kill routine
                        myProjectile.ai[1] = minimumFireFrames;
                    }
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
                    if (DBZMOD.IsTickRateElapsed(FIRE_KI_DRAIN_WINDOW))
                    {
                        modPlayer.AddKi(-FireKiDrainRate(modPlayer), true, false);
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

            wasSustainingFire = IsSustainingFire;
        }

        public override bool PreAI()
        {
            // pre AI of the charge ball is responsible for telling us if the weapon has changed or the projectile should otherwise die

            bool isPassingPreAi = base.PreAI();
            if (!isPassingPreAi && myProjectile != null)
            {
                ProjectileHelper.StartKillRoutine(myProjectile);
            }
            return isPassingPreAi;
        }

        public void KillBeam()
        {
            // set the cooldown
            CurrentFireTime = -initialBeamCooldown;

            if (myProjectile != null)
            {
                ProjectileHelper.StartKillRoutine(myProjectile);
                myProjectile = null;
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