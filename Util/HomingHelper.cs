using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Util
{
    public static class HomingHelper
    {
        /// <summary>
        ///     Performs a single frame of homing, detecting the closest target and adjusting its own velocity
        /// </summary>
        /// <param name="projectile">The projectile doing the homing.</param>
        /// <param name="homingRadius">The radius around the projectile to track targets in</param>
        /// <param name="acceleration">Complicated. Values should be somewhere between 1 and 10, with 10 being extremely tight tracking.</param>
        /// <param name="topSpeed">The speed at which the projectile will move, at most, when hunting a target.</param>
        /// <param name="isLineOfSightNeeded">Whether the projectile needs a clear path to its target before tracking it.</param>
        public static void DoHoming(Projectile projectile, float homingRadius, float topSpeed, bool isLineOfSightNeeded)
        {
            NPC closestTarget = null;
            float closestTargetDistance = float.MaxValue;
            foreach (NPC target in Main.npc)
            {

                //Get the shoot trajectory from the projectile and target
                // pass over if they're not in radius, friendly or inactive.
                float distance = Vector2.Distance(projectile.Center, target.Center);                
                if (distance > homingRadius || target.friendly || !target.active)
                    continue;

                if (isLineOfSightNeeded)
                {
                    if (!Collision.CanHitLine(projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        continue;
                    }
                }                

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    closestTarget = target;
                }                
            }

            // we've captured a target, the closest target possible.
            if (closestTarget != null)
            {
                // kind of redundant, get the offset velocity
                Vector2 offsetVector = closestTarget.Center - projectile.Center;
                Vector2 normalizedVelocity = (offsetVector * (topSpeed / 20) + projectile.velocity);
                normalizedVelocity.Normalize();
                Vector2 trueVelocity = normalizedVelocity * topSpeed;
                projectile.velocity = trueVelocity;
            }
        }
    }
}
