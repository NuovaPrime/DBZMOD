
using DBZMOD.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using DBZMOD.Enums;
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

        public static Rectangle GetClosestTileCollisionInBeam(Vector2 tailStart, Vector2 headEnd)
        {
            Rectangle tileHitbox = Rectangle.Empty;
            float closestTile = float.MaxValue;
            Utils.PlotTileLine(tailStart, headEnd, 0f, delegate (int x, int y)
            {
                //if (!tileHitbox.Equals(Rectangle.Empty))
                //    return false;
                bool isSolidTiles = Collision.SolidTiles(x, x, y, y);
                if (isSolidTiles)
                {
                    Vector2 centerPoint = new Vector2(x * 16f + 8f, y * 16f + 8f);
                    float newDistance = Vector2.Distance(centerPoint, tailStart);
                    if (newDistance < closestTile)
                    {
                        tileHitbox = new Rectangle(x * 16, y * 16, 16, 16);
                        closestTile = newDistance;
                    }
                }
                return !isSolidTiles;
            });
            //if (tileHitbox.Equals(Rectangle.Empty))
            //{
            //    // DebugHelper.Log("Not striking tile!");
            //}
            return tileHitbox;
        }

        public static Tuple<float, Rectangle> GetClosestTileCollisionByForwardSampling(Vector2 beamEnd, float beamSpeed, Vector2 velocity)
        {
            // roughly the safe forward projection to know you're about to hit a block, soon-ish. Most beam speeds are at 15f at the time of writing
            float step = 1f;
            float polls = 0;
            while (true)
            {
                polls++;
                Vector2 pollVector = beamEnd + (step * polls * velocity);
                if (pollVector.IsInWorldBounds() && pollVector.IsPositionInTile())
                {
                    Point tileCoords = pollVector.ToTileCoordinates();
                    return new Tuple<float, Rectangle>((polls - 1) * step, new Rectangle(tileCoords.X, tileCoords.Y, 16, 16));
                }
                if (polls * step > beamSpeed)
                    break;
            }

            return new Tuple<float, Rectangle>(0f, Rectangle.Empty);
        }

        public static Tuple<bool, float, BeamHitLocation> GetCollisionData(Vector2 tailStart, Vector2 beamStart, Vector2 headStart, Vector2 headEnd, float tailWidth, float beamWidth, float headWidth, float maxDistance, Rectangle targetHitbox)
        {
            float tailPoint = Vector2.Distance(tailStart, beamStart);
            float beamPoint = maxDistance;
            float headPoint = Vector2.Distance(headStart, headEnd);

            bool tailCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                tailStart, beamStart, tailWidth, ref tailPoint);

            if (tailCollision)
                return new Tuple<bool, float, BeamHitLocation>(true, tailPoint, BeamHitLocation.Tail);

            bool beamCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                beamStart, headStart, beamWidth, ref beamPoint);

            if (beamCollision)
                return new Tuple<bool, float, BeamHitLocation>(true, beamPoint, BeamHitLocation.Beam);

            bool headCollision = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                headStart, headEnd, headWidth, ref headPoint);

            if (headCollision)
                return new Tuple<bool, float, BeamHitLocation>(true, maxDistance + headPoint, BeamHitLocation.Head);

            return new Tuple<bool, float, BeamHitLocation>(false, maxDistance, BeamHitLocation.Unspecified);
        }
    }
}
