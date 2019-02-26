using System;
using DBZMOD.Extensions;
using DBZMOD.Utilities;
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

        private bool ShouldFireBeam(MyPlayer modPlayer)
        {
            return ChargeLevel > 0f && (!modPlayer.isMouseLeftHeld || IsFired);
        }

        private float GetBeamPowerMultiplier()
        {
            return (float)Math.Sqrt(ChargeLevel);
        }

        private int GetBeamDamage()
        {
            // Convenient place to set the damage multiplier of all beams by a flat coefficient for tuning - this is beam damage halved.
            return (int)Math.Ceiling(projectile.damage * GetBeamPowerMultiplier() / 3f);
        }

        // the rate at which firing drains the charge level of the ball, play with this for balance.
        protected float FireDecayRate() { return GetBeamPowerMultiplier() / 60f; }

        public Projectile myProjectile = null;

        public void HandleFiring(Player player, Vector2 mouseVector)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            // minimum charge level is required to fire in the first place, but once you fire, you can keep firing.
            if (ShouldFireBeam(modPlayer))
            {
                // once fired, there's no going back.
                IsFired = true;

                // kill the charge sound if we're firing
                chargeSoundSlotId = SoundHelper.KillTrackedSound(chargeSoundSlotId);

                if (myProjectile == null)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient || Main.myPlayer == player.whoAmI)
                    {
                        // fire the laser!
                        myProjectile = Projectile.NewProjectileDirect(projectile.position + beamCreationOffset, projectile.velocity, mod.ProjectileType(beamProjectileName), GetBeamDamage(), projectile.knockBack, projectile.owner);
                    }
                }

                // increment the fire time, this handles "IsSustainingFire" as well as stating the beam is no longer firable (it is already being fired)
                CurrentFireTime++;

                // if the player has charge left, drain the ball
                if (ChargeLevel >= 0f)
                {
                    ChargeLevel = Math.Max(0f, ChargeLevel - FireDecayRate());

                    if (myProjectile != null) // TODO Temporary fix for two beams crossing
                    {
                        BaseBeam projectileAsBeam = myProjectile.modProjectile as BaseBeam;
                        projectileAsBeam.BeamIntensity = ChargeLevel;
                    }
                }

                // beam is no longer sustainable, and neither is the charge ball
                if (ChargeLevel <= 0f)
                {
                    KillBeam();
                }
            }
        }

        public override bool PreAI()
        {
            // pre AI of the charge ball is responsible for telling us if the weapon has changed or the projectile should otherwise die

            bool isPassingPreAi = base.PreAI();
            if (!isPassingPreAi && myProjectile != null)
            {
                myProjectile.StartKillRoutine();
            }
            return isPassingPreAi;
        }

        public void KillBeam()
        {
            if (myProjectile != null)
            {
                myProjectile.StartKillRoutine();
                myProjectile = null;
            }
            // kill the charge ball
            projectile.Kill();
        }

        public override Vector2 DoControl(Player player)
        {
            // calls the base do control, which handles some of the rotation code and other complicated nonsense.
            var mouseVector = base.DoControl(player);

            // figure out if the player is shooting and fire the laser
            HandleFiring(player, mouseVector);

            return mouseVector;
        }
    }
}