using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Projectiles
{
    public class DoubleSundayCharge : BaseBeamCharge
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DoubleSundayCharge");
		}
    	
        public override void SetDefaults()
        {
            // the maximum charge level of the ball     
            chargeLimit = 4;

            // a frame timer used to essentially force a beam to be used for a minimum amount of time, preferably long enough for the firing sounds to play.
            // minimumFireFrames = 120;

            // the rate at which charge level increases while channeling
            chargeRatePerSecond = 1f;

            // this is the beam the charge beam fires when told to.
            beamProjectileName = "DoubleSundayBeam";

            // the type of dust that should spawn when charging or decaying
            dustType = 27;

            // Bigger number = slower movement. For reference, 60f is pretty fast. This doesn't have to match the beam speed.
            rotationSlowness = 15f;

            // this is the default cooldown when firing the beam, in frames, before you can fire again, regardless of your charge level.
            // initialBeamCooldown = 180;

            // the charge ball is just a single texture.
            // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
            chargeOrigin = new Point(0, 0);
            chargeSize = new Point(22, 22);

            // vector to reposition the charge ball if it feels too low or too high on the character sprite
            channelingOffset = new Vector2(0, 4f);

            // The sound effect used by the projectile when charging up.
            chargeSoundKey = "Sounds/EnergyWaveCharge";

            // The amount of delay between when the client will try to play the energy wave charge sound again, if the player stops and resumes charging.
            chargeSoundDelay = 120;

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
    }
}