using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Projectiles.FinalShine
{
    public class FinalShineCharge : BaseBeamCharge
    {
        public override void SetDefaults()
        {
            // the maximum charge level of the ball     
            chargeLimit = 11;

            // this is the beam the charge beam fires when told to.
            beamProjectileName = "FinalShineBeam";

            // the type of dust that should spawn when charging or decaying
            dustType = 74;

            // Bigger number = slower movement. For reference, 60f is pretty fast. This doesn't have to match the beam speed.
            rotationSlowness = 15f;

            // the charge ball is just a single texture.
            // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
            chargeOrigin = new Point(0, 0);
            chargeSize = new Point(56, 56);

            // vector to reposition the charge ball if it feels too low or too high on the character sprite
            channelingOffset = new Vector2(0, 4f);

            // The sound effect used by the projectile when charging up.
            chargeSoundKey = "Sounds/EnergyWaveCharge";

            // The amount of delay between when the client will try to play the energy wave charge sound again, if the player stops and resumes charging.
            chargeSoundDelay = 120;

            base.SetDefaults();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Final Shine Ball");
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