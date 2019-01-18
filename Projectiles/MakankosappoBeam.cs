﻿using Microsoft.Xna.Framework;

namespace DBZMOD.Projectiles
{
    public class MakankosappoBeam : BaseBeam
    {
        public override void SetDefaults()
        {
            // all beams tend to have a similar structure, there's a charge, a tail or "start", a beam (body) and a head (forwardmost point)
            // this is the structure that helps alleviate some of the logic burden by predefining the dimensions of each segment.
            TailOrigin = new Point(6, 0);
            TailSize = new Point(24, 12);
            BeamOrigin = new Point(6, 14);
            BeamSize = new Point(24, 36);
            HeadOrigin = new Point(2, 52);
            HeadSize = new Point(32, 36);

            // this determines how long the max fade in for beam opacity takes to fully "phase in", at a rate of 1f per frame. (This is handled by the charge ball)
            BeamFadeOutTime = 30f;

            // Bigger number = slower movement. For reference, 60f is pretty fast. 180f is pretty slow.
            RotationSlowness = 60f;

            // vector to reposition the beam tail down if it feels too low or too high on the character sprite
            OffsetY = new Vector2(0, 4f);            

            // the maximum travel distance the beam can go
            MaxBeamDistance = 2000f;

            // the speed at which the beam head travels through space
            BeamSpeed = 30f;

            // the type of dust to spawn when the beam is firing
            DustType = 169;

            // the frequency at which to spawn dust when the beam is firing
            DustFrequency = 0.6f;

            // how many particles per frame fire while firing the beam.
            FireParticleDensity = 6;

            // the frequency at which to spawn dust when the beam collides with something
            CollisionDustFrequency = 1.0f;

            // how many particles per frame fire when the beam collides with something
            CollisionParticleDensity = 8;

            ImmunityFrameOverride = 60;

            // The sound effect used by the projectile when firing the beam. (plays on initial fire only)
            BeamSoundKey = "Sounds/SBCFire";

            IsStationaryBeam = true;

            IsEntityColliding = false;

            IsBeamSegmentAnimated = true;

            base.SetDefaults();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Special Beam Cannon Blast");
        }
    }
}