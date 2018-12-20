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

namespace DBZMOD.Projectiles
{
    public class FinalFlashCharge : BaseCharge
    {
        public override void SetDefaults()
        {
            // the maximum charge level of the ball     
            ChargeLimit = 10;

            // this is the minimum charge level you have to have before you can actually fire the beam
            MinimumChargeLevel = 4f;

            // a frame timer used to essentially force a beam to be used for a minimum amount of time, preferably long enough for the firing sounds to play.
            MinimumFireFrames = 120;

            // the rate at which charge level increases while channeling
            ChargeRate = 0.016f; // approximately 1 level per second.

            // Rate at which Ki is drained while channeling
            ChargeKiDrainRate = 12;

            // determines the frequency at which ki drain ticks. Bigger numbers mean slower drain.
            CHARGE_KI_DRAIN_WINDOW = 2;

            // Rate at which Ki is drained while firing the beam *without a charge*
            // in theory this should be higher than your charge ki drain, because that's the advantage of charging now.
            FireKiDrainRate = 48;

            // determines the frequency at which ki drain ticks while firing. Again, bigger number, slower drain.
            FIRE_KI_DRAIN_WINDOW = 2;

            // the rate at which firing drains the charge level of the ball, play with this for balance.
            FireDecayRate = 0.036f;

            // the rate at which the charge decays when not channeling
            DecayRate = 0.016f; // very slow decay when not channeling

            // this is the beam the charge beam fires when told to.
            BeamProjectileName = "FinalFlashBeam";

            // this determines how long the max fade in for beam opacity takes to fully "phase in", at a rate of 1f per frame.
            // For the most part, you want to make this the same as the beam's FadeInTime, *unless* you want the beam to stay partially transparent.
            BeamFadeInTime = 300f;

            // the type of dust that should spawn when charging or decaying
            DustType = 169;

            // the percentage frequency at which dust spawns each frame

            // rate at which decaying produces dust
            DecayDustFrequency = 0.6f;

            // rate at which charging produces dust
            ChargeDustFrequency = 0.4f;

            // rate at which dispersal of the charge ball (from weapon swapping) produces dust
            DisperseDustFrequency = 1.0f;

            // the amount of dust that tries to spawn when the charge orb disperses from weapon swapping.
            DisperseDustQuantity = 40;

            // Bigger number = slower movement. For reference, 60f is pretty fast. This doesn't have to match the beam speed.
            RotationSlowness = 15f;

            // this is the default cooldown when firing the beam, in frames, before you can fire again, regardless of your charge level.
            InitialBeamCooldown = 180;

            // the charge ball is just a single texture.
            // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
            ChargeOrigin = new Point(0, 0);
            ChargeSize = new Point(18, 18);

            // vector to reposition the charge ball if it feels too low or too high on the character sprite
            ChannelingOffset = new Vector2(0, 4f);

            // The sound effect used by the projectile when charging up.
            ChargeSoundKey = "Sounds/FinalFlashCharge";

            // The amount of delay between when the client will try to play the energy wave charge sound again, if the player stops and resumes charging.
            ChargeSoundDelay = 120;

            // EXPERIMENTAL, UNUSED - needs adjustment
            // vector to reposition the charge ball when the player *isn't* charging it (or firing the beam) - held to the side kinda.
            NotChannelingOffset = new Vector2(-15, 20f);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Final Flash Ball");
        }
    }
}