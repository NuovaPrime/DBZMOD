using DBZMOD;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace Util
{
    public static class ProjectileUtil
    {
        public static void ApplyChannelingSlowdown(Player player)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            if (modPlayer.IsFlying)
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                float yVelocity = -(player.gravity + 0.001f);
                if (modPlayer.IsDownHeld || modPlayer.IsUpHeld)
                {
                    yVelocity = player.velocity.Y / (1.2f - chargeMoveSpeedBonus);
                }
                else
                {
                    yVelocity = Math.Min(-0.4f, player.velocity.Y / (1.2f - chargeMoveSpeedBonus));
                }
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), yVelocity);
            }
            else
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                // don't neuter falling - keep the positive Y velocity if it's greater - if the player is jumping, this reduces their height. if falling, falling is always greater.                        
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), Math.Max(player.velocity.Y, player.velocity.Y / (1.2f - chargeMoveSpeedBonus)));
            }
        }

        // find the closest projectile to a player (owned by that player) of a given type, used to "recapture" charge balls, letting the player resume charging them whenever they want.
        public static Projectile FindNearestOwnedProjectileOfType(Player player, int type)
        {
            int closestProjectile = -1;
            float distance = float.MaxValue;
            for(var i = 0; i < Main.projectile.Length; i++)
            {
                var proj = Main.projectile[i];

                // abort if the projectile is invalid, the player isn't the owner, the projectile is inactive or the type doesn't match what we want.
                if (proj == null || proj.owner != player.whoAmI || !proj.active || proj.type != type)
                    continue;               
                
                var projDistance = proj.Distance(player.Center);
                if (projDistance < distance)
                {
                    distance = projDistance;
                    closestProjectile = i;
                }                
            }
            return closestProjectile == -1 ? null : Main.projectile[closestProjectile];
        }

        public static bool RecapturePlayerProjectile(Player player, int type)
        {            
            var proj = FindNearestOwnedProjectileOfType(player, type);
            if (proj != null)
            {
                // the part that matters
                player.heldProj = proj.whoAmI;
                return true;
            }
            return false;
        }        

        // spawn some dust (of type: dustId) that approaches or leaves the ball's center, depending on whether it's charging or decaying. Frequency is the chance to spawn one each frame.
        public static void DoChargeDust(Vector2 chargeBallPosition, int dustId, float dustFrequency, bool isDecaying, Vector2 chargeSize)
        {
            // snazzy charge up dust, reduced to less or equal to one per frame.
            if (Main.rand.NextFloat() < dustFrequency)
            {
                chargeBallPosition -= chargeSize / 2f;
                float angle = Main.rand.NextFloat(360);
                float angleRad = MathHelper.ToRadians(angle);
                Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
                // float hypotenuse = chargeSize.LengthSquared();
                Vector2 offsetPosition = chargeBallPosition + position * (10f + 2.0f);
                Vector2 spawnPosition = isDecaying ? chargeBallPosition : offsetPosition;
                // DebugUtil.Log(string.Format("Trying to spawn charge particles at {0}, {1} - Decaying? {2}", spawnPosition.X, spawnPosition.Y, isDecaying));
                Vector2 velocity = isDecaying ? Vector2.Normalize(spawnPosition - offsetPosition) : Vector2.Normalize(chargeBallPosition - spawnPosition);
                Dust tDust = Dust.NewDustDirect(spawnPosition, (int)chargeSize.X, (int)chargeSize.Y, dustId, 0f, 0f, 213, default(Color), 1.0f);
                tDust.velocity = velocity;
                tDust.noGravity = true;
            }
        }

        // spawn some dust (of type: dustId) that approaches or leaves the ball's center, depending on whether it's charging or decaying. Frequency is the chance to spawn one each frame.
        public static void DoBeamDust(Vector2 TailPosition, Vector2 velocity, int dustId, float dustFrequency, float travelDistance, float TailHeldDistance, Vector2 tailSize, float BeamSpeed)
        {
            // snazzy beam shooting dust, reduced to less than 1 per frame.
            if (Main.rand.NextFloat() < dustFrequency)
            {
                TailPosition -= tailSize / 2f;
                float angle = velocity.ToRotation();
                Vector2 spawnPositionOffset = tailSize + new Vector2(tailSize.X * (Main.rand.NextFloat(0.5f) - 0.25f), tailSize.Y * (Main.rand.NextFloat(0.5f) - 0.25f));
                Vector2 beamTailPosition = TailPosition + tailSize * velocity;
                Dust tDust = Dust.NewDustDirect(beamTailPosition, (int)tailSize.X, (int)tailSize.Y, dustId, 0f, 0f, 213, default(Color), 1f);
                tDust.velocity = velocity * travelDistance / 10f;
                tDust.noGravity = true;
            }
        }

        // spawn some dust (of type: dustId) that approaches or leaves the ball's center, depending on whether it's charging or decaying. Frequency is the chance to spawn one each frame.
        public static void DoBeamCollisionDust(int dustId, float dustFrequency, Vector2 velocity, Vector2 endPosition)
        {
            // snazzy charge up dust, reduced to less or equal to one per frame.
            if (Main.rand.NextFloat() < dustFrequency)
            {
                Vector2 beamCollisionPosition = endPosition;
                float angle = Main.rand.NextFloat(-62.5f, 62.5f);
                Vector2 backDraft = velocity * -1f;
                float angleRad = MathHelper.ToRadians(angle) + backDraft.ToRotation();
                Vector2 backdraftWithRandomization = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
                Dust tDust = Dust.NewDustDirect(beamCollisionPosition, 30, 30, dustId, 0f, 0f, 213, default(Color), 1.0f);
                tDust.velocity = backdraftWithRandomization;
                tDust.noGravity = true;
            }
        }

        // starts the kill routine for beams that lets them detach from the charge ball and fade incrementally.
        public static void StartKillRoutine(Projectile projectile)
        {
            if (projectile == null)
                return;

            if (projectile.localAI[0] == 0)
                projectile.localAI[0] = 1;
        }
    }
}
