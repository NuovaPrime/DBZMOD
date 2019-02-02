using DBZMOD.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using DBZMOD.Extensions;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DBZMOD.Util
{
    public static class ProjectileHelper
    {
        // find the closest projectile to a player (owned by that player) of a given type, used to "recapture" charge balls, letting the player resume charging them whenever they want.

        public static void RegisterMassiveBlast(int projectileType)
        {
            if (MassiveBlastProjectileTypes.Contains(projectileType))
                return;
            MassiveBlastProjectileTypes.Add(projectileType);
        }

        public static List<int> MassiveBlastProjectileTypes = new List<int>();

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
        public static void DoBeamDust(Vector2 tailPosition, Vector2 velocity, int dustId, float dustFrequency, float travelDistance, float tailHeldDistance, Vector2 tailSize, float beamSpeed)
        {
            // snazzy beam shooting dust, reduced to less than 1 per frame.
            if (Main.rand.NextFloat() < dustFrequency)
            {
                float randomLengthOnBeam = Main.rand.NextFloat(tailHeldDistance, travelDistance + tailHeldDistance);
                Vector2 beamWidthVariance = tailSize / 2f;
                float xVar = Math.Abs(beamWidthVariance.X);
                float yVar = Math.Abs(beamWidthVariance.Y);
                Vector2 variance = new Vector2(Main.rand.NextFloat(-xVar, xVar), Main.rand.NextFloat(-yVar, yVar));
                Vector2 randomPositionOnBeam = tailPosition - (tailSize / 2f) + variance * velocity + randomLengthOnBeam * velocity;
                Dust tDust = Dust.NewDustDirect(randomPositionOnBeam, (int)tailSize.X, (int)tailSize.Y, dustId, 0f, 0f, 213, default(Color), 1f);
                float angleVariance = Main.rand.NextFloat() < 0.5f ? -90 : 90f;
                float resultVectorDegrees = velocity.VectorToDegrees() + angleVariance;
                tDust.velocity = resultVectorDegrees.DegreesToVector() * (tailSize.Y / 40f);
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
                float resultDegrees = backDraftVector.VectorToDegrees() + Main.rand.NextFloat(-45f, 45f);
                Vector2 backDraft = resultDegrees.DegreesToVector();
                //float angleRad = MathHelper.ToRadians(angle);
                //Vector2 backdraftWithRandomization = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad)) + backDraft;
                Dust tDust = Dust.NewDustDirect(endPosition - new Vector2(8f, 8f), 30, 30, dustId, 0f, 0f, 213, default(Color), 1.0f);
                tDust.velocity = backDraft * 15f;
                tDust.noGravity = true;
            }
        }

        public static Tuple<bool, float> GetClosestTileCollisionInRay(Vector2 start, Vector2 velocity, float distance, float beamSpeed)
        {
            float closestPoint = distance + beamSpeed;
            Vector2 end = start + (distance + beamSpeed) * velocity;
            Tuple<bool, float> resultCollisionData = new Tuple<bool, float>(false, closestPoint);
            Utils.PlotTileLine(start, end, 0.0001f, delegate (int x, int y)
            {
                DebugHelper.Log($"Plotting line at {x} {y}");
                bool isSolidTiles = Collision.SolidTiles(x, x, y, y);
                if (isSolidTiles)
                {
                    Rectangle tileHitbox = new Rectangle(x * 16, y * 16, 16, 16); // slightly larger than a tile
                    Tuple<bool, float> collisionData = GetTileCollisionData(start, end, tileHitbox);
                    bool isColliding = collisionData.Item1;
                    float collisionLength = collisionData.Item2;
                    if (isColliding && collisionLength < closestPoint)
                    {
                        DebugHelper.Log($"Collision constrained at {x} {y} at {collisionLength} distance");
                        resultCollisionData = collisionData;
                        closestPoint = collisionLength;
                    }
                }
                return !isSolidTiles;
            });

            if (resultCollisionData.Item1)
            {
                DebugHelper.Log($"Final beam collision occurring {distance} distance");
            }

            return resultCollisionData;
        }

        public static Tuple<bool, float> GetTileCollisionData(Vector2 startPoint, Vector2 endPoint, Rectangle targetHitbox)
        {
            float maxDistance = Vector2.Distance(startPoint, endPoint);
            float beamPoint = maxDistance;

            bool beamCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startPoint, endPoint, 0f, ref beamPoint);

            if (beamCollision)
                return new Tuple<bool, float>(true, beamPoint);

            return new Tuple<bool, float>(false, maxDistance);
        }

        public static Tuple<bool, float> GetCollisionData(Vector2 tailStart, Vector2 bodyStart, Vector2 headStart, Vector2 headEnd, float tailWidth, float bodyWidth, float headWidth, float maxDistance, Rectangle targetHitbox)
        {
            float tailPoint = maxDistance;
            float bodyPoint = maxDistance;
            float headPoint = maxDistance;

            bool tailCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), tailStart, bodyStart, tailWidth, ref tailPoint);

            if (tailCollision)
                return new Tuple<bool, float>(true, tailPoint);

            bool bodyCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), bodyStart, headStart, bodyWidth, ref bodyPoint);

            if (bodyCollision)
                return new Tuple<bool, float>(true, bodyPoint);

            bool headCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), headStart, headEnd, headWidth, ref headPoint);

            if (headCollision)
                return new Tuple<bool, float>(true, headPoint);

            return new Tuple<bool, float>(false, maxDistance);
        }

        public static bool CanHitLine(Vector2 start, Vector2 end)
        {
            return Utils.PlotTileLine(start, end, 0f, delegate (int x, int y)
            {
                Tile tile = Main.tile[x, y];
                return tile == null || tile.inActive() || !Main.tile[x, y].active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type];
            });
        }

        //// shameless appropriation of vanilla collision check with modifications to be more.. lasery.
        //public static bool OldCanHitLine(Vector2 position1, Vector2 position2)
        //{
        //    var step = Vector2.Normalize(position2 - position1) * 8f;
        //    bool isColliding = false;
        //    // since the step loop is going to depend on quadrant/direction, I took the cowardly approach and divided it into four quadrants.
        //    if (step.X < 0)
        //    {
        //        while (position1.X >= position2.X && position1.IsInWorldBounds())
        //        {
        //            position1 += step;
        //            isColliding = position1.IsPositionInTile();
        //            if (isColliding)
        //                break;
        //        }
        //    } else if (step.X > 0)
        //    {
        //        while (position1.X <= position2.X && position1.IsInWorldBounds())
        //        {
        //            position1 += step;
        //            isColliding = position1.IsPositionInTile();
        //            if (isColliding)
        //                break;
        //        }
        //    } else if (step.Y < 0)
        //    {
        //        while (position1.Y >= position2.Y && position1.IsInWorldBounds())
        //        {
        //            position1 += step;
        //            isColliding = position1.IsPositionInTile();
        //            if (isColliding)
        //                break;
        //        }
        //    } else if (step.Y > 0)
        //    {
        //        while (position1.Y <= position2.Y && position1.IsInWorldBounds())
        //        {
        //            position1 += step;
        //            isColliding = position1.IsPositionInTile();
        //            if (isColliding)
        //                break;
        //        }
        //    }
        //    return !isColliding;
        //}
    }
}
