using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Models
{
    /// <summary>
    ///     Used to track the velocity decayed by beam attacks on targets, for game feel as well as mechanical viability of beams in general.
    /// </summary>
    public class HitStunInfo
    {
        /// <summary>
        ///     The *whoAmI* of the target. Right now this class is only intended for use on NPCs/Bosses.
        /// </summary>
        public int targetId;
        
        /// <summary>
        ///     A reference to the NPC target being slowed down. Compared to the targetId during application to make sure the target hasn't gone stale.
        /// </summary>
        public NPC target;

        /// <summary>
        ///     The original velocity of the target, first struck.
        /// </summary>
        public float originalVelocity;

        /// <summary>
        ///     Value representing the old velocity of the target
        /// </summary>
        public float lastVelocity = 0f;

        /// <summary>
        ///     The amount of slowdown to apply to the enemy. This is a multiplier, so lower numbers mean slower movement.
        /// </summary>
        public float velocityCoefficient;

        /// <summary>
        ///     Value representing the current decay value being recovered by the tracker.
        /// </summary>
        public float currentDecay;

        /// <summary>
        ///     The number of ticks the tracker should be alive for. The velocity is restored over the duration of a beam attack.
        /// </summary>
        public int ticksAlive;

        /// <summary>
        ///     The timestamp of the decaying velocity tracker's creation, allows it to be marginally self aware about how long it has existed.
        /// </summary>
        public int maxDuration;

        /// <summary>
        ///     Once the ticks alive is less than this percentage from its originating value, the hitstun will start to decay, restoring the target's velocity.
        /// </summary>
        public float decaySlowRatio;

        /// <summary>
        ///     On instantiation the hitstun hasn't happened yet. Keep track of this to apply it once, before running other conditional checks.
        /// </summary>
        public bool wasEverApplied = false;
        
        /// <summary>
        ///     Instantiate a new container for histun data.
        /// </summary>
        /// <param name="target">The target being slowed down</param>
        /// <param name="hitStunMaxDuration">The expected duration of the slowdown</param>
        /// <param name="slowRatio">The coefficient to apply to the target's speed when the hitstun is applied.</param>
        /// <param name="velocityRestorationThreshold">The percentage of the number of ticks remaining in the stun devoted to *restoring* the enemy's speed.</param>
        public HitStunInfo(NPC target, int hitStunMaxDuration, float slowRatio, float velocityRestorationThreshold)
        {
            this.target = target;
            originalVelocity = target.velocity.Length();
            maxDuration = hitStunMaxDuration;
            ticksAlive = 0;
            velocityCoefficient = slowRatio;
            decaySlowRatio = velocityRestorationThreshold;
        }

        /// <summary>
        ///     Apply the slowdown effect on the target.
        /// </summary>
        /// <returns>True if the tracker needs to dispose of this information, for any reason.</returns>
        public bool IsExpired()
        {
            if (target == null || !target.active || target.whoAmI != targetId)
                return true;
            if (ticksAlive >= maxDuration)
                return true;
            return false;
        }

        public void ApplyHitstun()
        {
            float ticksLeftRatio = 1f - ((float)ticksAlive / maxDuration);
            DebugHelper.Log($"Applying hitstun to {targetId} - {ticksLeftRatio}% remaining.");
            // the hit stun has no time left, it's time to start restoring velocity to the target
            if (ticksLeftRatio <= decaySlowRatio)
            {
                int ticksRemaining = maxDuration - ticksAlive;
                float recoveryCoefficient = 1f + (currentDecay / ticksRemaining);
                target.velocity *= recoveryCoefficient;
                DebugHelper.Log($"Recovering velocity by {(recoveryCoefficient - 1f) * 100f}%");
                target.netUpdate = true;
                currentDecay -= (recoveryCoefficient - 1f);
            }
            else
            {
                float intensityToApply = 0f;
                // the hit stun is going into effect or hasn't tapered off yet and we're in a subsequent hit.
                if (!wasEverApplied)
                {
                    // this is the first time to hit them, they get the full effect of the velocity hit.
                    wasEverApplied = true;
                    intensityToApply = velocityCoefficient;
                }
                else
                {
                    float intensity = target.velocity.Length();
                    // this is a subsequent hit, so the velocity hit is based on how much of their speed they recovered.
                    // if they didn't recover *any* speed, this doesn't nothing. If they did recover some speed, knock them back down.
                    float intensityChange = intensity - lastVelocity;
                    // the target has sped up since last frame's poll was taken. 
                    if (intensityChange > 0f)
                    {
                        intensityToApply = 1f - (intensityChange / intensity);
                    }
                }

                if (intensityToApply > 0f)
                {
                    DebugHelper.Log($"Cutting velocity by {(1f - intensityToApply) * 100f}%");
                    target.velocity *= intensityToApply;
                    target.netUpdate = true;
                    currentDecay += (1 - intensityToApply);
                }
            }
        }
    }
}