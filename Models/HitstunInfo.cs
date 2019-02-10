using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Models
{
    /// <summary>
    ///     Used to track the velocity decayed by beam attacks on targets, for game feel as well as mechanical viability of beams in general.
    /// </summary>
    public class HitstunInfo
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
        public Vector2 originalVelocity;

        /// <summary>
        ///     Value representing the old velocity of the target
        /// </summary>
        public Vector2 lastVelocity;

        /// <summary>
        ///     The amount of slowdown to apply to the enemy. This is a multiplier, so lower numbers mean slower movement.
        /// </summary>
        public float velocityCoefficient;

        /// <summary>
        ///     The result vector after the slowdown specified is applied.
        /// </summary>
        public Vector2 transformedVelocity;

        /// <summary>
        ///     Value representing the current decay value being recovered by the tracker.
        /// </summary>
        public Vector2 currentDecay;

        /// <summary>
        ///     The number of ticks the tracker should be alive for. The velocity is restored over the duration of a beam attack.
        /// </summary>
        public int ticksAlive;

        /// <summary>
        ///     The timestamp of the decaying velocity tracker's creation, allows it to be marginally self aware about how long it has existed.
        /// </summary>
        public int duration;

        /// <summary>
        ///     Once the ticks alive is less than this percentage from its originating value, the hitstun will start to decay, restoring the target's velocity.
        /// </summary>
        public float undecayRatio;

        /// <summary>
        ///     On instantiation the hitstun hasn't happened yet. Keep track of this to apply it once, before running other conditional checks.
        /// </summary>
        public bool wasEverApplied = false;
        
        /// <summary>
        ///     Instantiate a new container for histun data.
        /// </summary>
        /// <param name="target">The target being slowed down</param>
        /// <param name="remainingHitstunDuration">The expected duration of the slowdown</param>
        /// <param name="slowRatio"></param>
        public HitstunInfo(NPC target, int remainingHitstunDuration, float slowRatio, float velocityRestorationThreshold)
        {
            this.target = target;
            originalVelocity = target.velocity;
            lastVelocity = originalVelocity;
            duration = remainingHitstunDuration;
            ticksAlive = 0;
            velocityCoefficient = slowRatio;
            transformedVelocity = originalVelocity * slowRatio;
            currentDecay = originalVelocity - transformedVelocity;
            undecayRatio = velocityRestorationThreshold;
        }

        /// <summary>
        ///     Apply the slowdown effect on the target.
        /// </summary>
        /// <returns>True if the tracker needs to dispose of this information, for any reason.</returns>
        public bool CheckShouldDispose()
        {
            if (target == null || !target.active || target.whoAmI != targetId)
                return true;
            if (ticksAlive >= duration)
                return true;
            return false;
        }

        public bool CheckShouldApply()
        {
            if (!wasEverApplied)
            {
                wasEverApplied = true;
                return true;
            }

            if (lastVelocity.Length() < target.velocity.Length())
                return true;

            return false;
        }

        public void ApplyHitstun()
        {
            if (CheckShouldApply())
            {

            }
        }
    }
}