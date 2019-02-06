using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Projectiles.EnergyWaves
{
	public class EnergyWaveCharge : BaseBeamCharge
	{
        public override void SetDefaults()
        {            
            // the maximum charge level of the ball     
            chargeLimit = 3;

            // the rate at which charge level increases while channeling
            chargeRatePerSecond = 1f;

            // a frame timer used to essentially force a beam to be used for a minimum amount of time, preferably long enough for the firing sounds to play.
            // minimumFireFrames = 120;

            // this is the beam the charge beam fires when told to.
            beamProjectileName = "EnergyWaveBeam";

            // the type of dust that should spawn when charging or decaying
            dustType = 15;

            // this is the default cooldown when firing the beam, in frames, before you can fire again, regardless of your charge level.
            // initialBeamCooldown = 180;

            // the charge ball is just a single texture.
            // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
            chargeOrigin = new Point(0, 0);
            chargeSize = new Point(18, 18);

            // vector to reposition the charge ball if it feels too low or too high on the character sprite
            channelingOffset = new Vector2(0, 4f);

            // The sound effect used by the projectile when charging up.
            chargeSoundKey = "Sounds/EnergyWaveChargeShort";

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