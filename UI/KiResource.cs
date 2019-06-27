﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
 using Terraria.UI;

namespace DBZMOD.UI
{
    public class UIFlatPanel : UIElement
    {
        public Color backgroundColor = Color.Gray;
        public static Texture2D backgroundTexture;

        public UIFlatPanel()
        {
            if (backgroundTexture == null)
                backgroundTexture = GFX.bg;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }        
    }
}