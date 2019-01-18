using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Projectiles
{
	public class EnergyWaveCharge : BaseBeamCharge
	{
        public override void SetDefaults()
        {            
            // the maximum charge level of the ball     
            ChargeLimit = 3;

            // this is the minimum charge level you have to have before you can actually fire the beam
            MinimumChargeLevel = 1f;

            // the rate at which charge level increases while channeling
            ChargeRatePerSecond = 1f;

            // Rate at which Ki is drained while channeling
            ChargeKiDrainPerSecond = FireKiDrainPerSecond = 40;

            // rate at which firing drains charge until depleted, keep this less than the ratio between ki drain (charge and fire) or charging won't be beneficial to preserving ki.
            FireChargeDrainPerSecond = 1.2f;

            // rate at which charge decays. keeping this roughly the same as the rate it charges is okay.
            DecayChargeLevelPerSecond = 1f;

            // a frame timer used to essentially force a beam to be used for a minimum amount of time, preferably long enough for the firing sounds to play.
            MinimumFireFrames = 120;

            // this is the beam the charge beam fires when told to.
            BeamProjectileName = "EnergyWaveBeam";

            // the type of dust that should spawn when charging or decaying
            DustType = 169;

            // this is the default cooldown when firing the beam, in frames, before you can fire again, regardless of your charge level.
            InitialBeamCooldown = 180;

            // the charge ball is just a single texture.
            // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
            ChargeOrigin = new Point(0, 0);
            ChargeSize = new Point(16, 16);

            // vector to reposition the charge ball if it feels too low or too high on the character sprite
            ChannelingOffset = new Vector2(0, 4f);

            // The sound effect used by the projectile when charging up.
            ChargeSoundKey = "Sounds/EnergyWaveChargeShort";

            base.SetDefaults();
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}