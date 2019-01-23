using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Projectiles.Kamehamehas.Kamehameha10
{
    public class Kamehameha10Charge : BaseBeamCharge
    {
        public override void SetDefaults()
        {
            // the maximum charge level of the ball     
            chargeLimit = 8;

            // this is the minimum charge level you have to have before you can actually fire the beam
            minimumChargeLevel = 3f;

            // a frame timer used to essentially force a beam to be used for a minimum amount of time, preferably long enough for the firing sounds to play.
            minimumFireFrames = 120;

            // the rate at which charge level increases while channeling
            chargeRatePerSecond = 1f;

            // Rate at which Ki is drained while channeling
            chargeKiDrainPerSecond = 120;

            // rate at which firing drains charge until depleted, keep this less than the ratio between ki drain (charge and fire) or charging won't be beneficial to preserving ki.
            fireChargeDrainPerSecond = 1.2f;

            // rate at which charge decays. keeping this roughly the same as the rate it charges is okay.
            decayChargeLevelPerSecond = 1f;

            // this is the beam the charge beam fires when told to.
            beamProjectileName = "Kamehameha10Beam";

            // this determines how long the max fade in for beam opacity takes to fully "phase in", at a rate of 1f per frame.
            // For the most part, you want to make this the same as the beam's FadeInTime, *unless* you want the beam to stay partially transparent.
            beamFadeInTime = 300f;

            // the type of dust that should spawn when charging or decaying
            dustType = 183;

            // Bigger number = slower movement. For reference, 60f is pretty fast. This doesn't have to match the beam speed.
            rotationSlowness = 15f;

            // this is the default cooldown when firing the beam, in frames, before you can fire again, regardless of your charge level.
            initialBeamCooldown = 180;

            // the charge ball is just a single texture.
            // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
            chargeOrigin = new Point(0, 0);
            chargeSize = new Point(22, 22);

            // vector to reposition the charge ball if it feels too low or too high on the character sprite
            channelingOffset = new Vector2(0, 4f);

            // The sound effect used by the projectile when charging up.
            chargeSoundKey = "Sounds/Kamehameha10Charge";

            // The amount of delay between when the client will try to play the energy wave charge sound again, if the player stops and resumes charging.
            chargeSoundDelay = 120;

            base.SetDefaults();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kamehameha x10 Ball");
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