using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Extensions
{
    /// <summary>
    ///     Extension class containing utilities for Vectors I've needed for various things.
    /// </summary>
    public static class CoordinateExtensions
    {
        public static float VectorToRadians(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static float VectorToDegrees(this Vector2 vector)
        {
            return MathHelper.ToDegrees(vector.VectorToRadians());
        }

        public static Vector2 DegreesToVector(this float degrees)
        {
            return MathHelper.ToRadians(degrees).RadiansToVector();
        }

        public static Vector2 RadiansToVector(this float angleRad)
        {
            return new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
        }

        //public static int GetTileX(this float xCoord)
        //{
        //    return (int)Math.Min(Main.maxTilesX - 1, Math.Max(1, xCoord / 16f));
        //}

        //public static int GetTileY(this float yCoord)
        //{
        //    return (int)Math.Min(Main.maxTilesY - 1, Math.Max(1, yCoord / 16f));
        //}

        public static bool IsInWorldBounds(this Vector2 hitVector)
        {
            return hitVector.X >= 0 && hitVector.X <= Main.maxTilesX * 16f && hitVector.Y >= 0 && hitVector.Y <= Main.maxTilesY * 16f;
        }

        public static bool IsPositionInTile(this Vector2 position)
        {
            return Collision.GetTilesIn(position, position).Count > 0;
            //var tilePoint = new Point(position.X.GetTileX(), position.Y.GetTileY());
            //var tile = Framing.GetTileSafely(tilePoint.X, tilePoint.Y);
            //if (tile == null)
            //    return false;
            //// dunno if this works.
            //if (Main.tileSolidTop[tile.type])
            //    return false;
            //if (tile.active() && Main.tileSolid[tile.type])
            //{
            //    return true;
            //}
            //return false;
        }
    }
}