using DBZMOD;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace DBZMOD.Util
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
                float randomLengthOnBeam = Main.rand.NextFloat(TailHeldDistance, travelDistance + TailHeldDistance);
                Vector2 beamWidthVariance = tailSize / 2f;
                float xVar = Math.Abs(beamWidthVariance.X);
                float yVar = Math.Abs(beamWidthVariance.Y);
                Vector2 variance = new Vector2(Main.rand.NextFloat(-xVar, xVar), Main.rand.NextFloat(-yVar, yVar));
                Vector2 randomPositionOnBeam = TailPosition - (tailSize / 2f) + variance * velocity + randomLengthOnBeam * velocity;
                Dust tDust = Dust.NewDustDirect(randomPositionOnBeam, (int)tailSize.X, (int)tailSize.Y, dustId, 0f, 0f, 213, default(Color), 1f);
                float angleVariance = Main.rand.NextFloat() < 0.5f ? -90 : 90f;
                tDust.velocity = DegreesToVector(VectorToDegrees(velocity) + angleVariance) * (tailSize.Y / 40f);
                tDust.noGravity = true;
            }
        }

        // spawn some dust (of type: dustId) that approaches or leaves the ball's center, depending on whether it's charging or decaying. Frequency is the chance to spawn one each frame.
        public static void DoBeamCollisionDust(int dustId, float dustFrequency, Vector2 velocity, Vector2 endPosition)
        {
            // snazzy charge up dust, reduced to less or equal to one per frame.
            if (Main.rand.NextFloat() < dustFrequency)
            {
                //float angle = Main.rand.NextFloat(-62.5f, 62.5f);
                Vector2 backDraftVector = velocity * -1f;
                Vector2 backDraft = DegreesToVector(VectorToDegrees(backDraftVector) + Main.rand.NextFloat(-45f, 45f));
                //float angleRad = MathHelper.ToRadians(angle);
                //Vector2 backdraftWithRandomization = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad)) + backDraft;
                Dust tDust = Dust.NewDustDirect(endPosition - new Vector2(8f, 8f), 30, 30, dustId, 0f, 0f, 213, default(Color), 1.0f);
                tDust.velocity = backDraft * 15f;
                tDust.noGravity = true;
            }
        }

        public static float VectorToRadians(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static float VectorToDegrees(Vector2 vector)
        {
            return MathHelper.ToDegrees(VectorToRadians(vector));
        }

        public static Vector2 DegreesToVector(float degrees)
        {
            return RadiansToVector(MathHelper.ToRadians(degrees));
        }

        public static Vector2 RadiansToVector(float angleRad)
        {
            return new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
        }

        // starts the kill routine for beams that lets them detach from the charge ball and fade incrementally.
        public static void StartKillRoutine(Projectile projectile)
        {
            if (projectile == null)
                return;

            if (projectile.localAI[0] == 0)
                projectile.localAI[0] = 1;
        }

        public static int GetTileX(float xCoord)
        {
            return (int)Math.Min(Main.maxTilesX - 1, Math.Max(1, xCoord / 16f));
        }

        public static int GetTileY(float yCoord)
        {
            return (int)Math.Min(Main.maxTilesY - 1, Math.Max(1, yCoord / 16f));
        }

        public static bool IsInWorldBounds(Vector2 hitVector)
        {
            return hitVector.X >= 0 && hitVector.X <= Main.maxTilesX * 16f && hitVector.Y >= 0 && hitVector.Y <= Main.maxTilesY * 16f;
        }

        // shameless appropriation of vanilla collision check with modifications to be more.. lasery.
        public static bool CanHitLine(Vector2 Position1, Vector2 Position2)
        {
            var step = Vector2.Normalize(Position2 - Position1) * 8f;
            bool isColliding = false;
            // since the step loop is going to depend on quadrant/direction, I took the cowardly approach and divided it into four quadrants.
            if (step.X < 0)
            {
                while (Position1.X >= Position2.X && IsInWorldBounds(Position1))
                {
                    Position1 += step;
                    isColliding = IsPositionInTile(Position1);
                    if (isColliding)
                        break;
                }
            } else if (step.X > 0)
            {
                while (Position1.X <= Position2.X && IsInWorldBounds(Position1))
                {
                    Position1 += step;
                    isColliding = IsPositionInTile(Position1);
                    if (isColliding)
                        break;
                }
            } else if (step.Y < 0)
            {
                while (Position1.Y >= Position2.Y && IsInWorldBounds(Position1))
                {
                    Position1 += step;
                    isColliding = IsPositionInTile(Position1);
                    if (isColliding)
                        break;
                }
            } else if (step.Y > 0)
            {
                while (Position1.Y <= Position2.Y && IsInWorldBounds(Position1))
                {
                    Position1 += step;
                    isColliding = IsPositionInTile(Position1);
                    if (isColliding)
                        break;
                }
            }
            return !isColliding;
        }

        public static bool IsPositionInTile(Vector2 position)
        {
            var tilePoint = new Point(GetTileX(position.X), GetTileY(position.Y));
            var tile = Framing.GetTileSafely(tilePoint.X, tilePoint.Y);
            if (tile == null)
                return false;
            if (tile.active() && Main.tileSolid[tile.type])
            {
                return true;
            }
            return false;
        }
    }
}
