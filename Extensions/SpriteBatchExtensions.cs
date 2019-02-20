using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBZMOD.Extensions
{
    internal static class SpriteBatchExtensions
    {
        // https://stackoverflow.com/questions/17275315/xna-drawing-2d-lines
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int) begin.X, (int) begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);

            float angle = (float) Math.Acos(Vector2.Dot(v, -Vector2.UnitX));

            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(GFX.whiteSquare, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static void DrawPolyLine(this SpriteBatch spriteBatch, Vector2[] points, Color color, int width = 1, bool closed = false)
        {
            for (int i = 0; i < points.Length - 1; i++)
                spriteBatch.DrawLine(points[i], points[i + 1], color, width);
            if (closed)
                spriteBatch.DrawLine(points[points.Length - 1], points[0], color, width);
        }
    }
}
