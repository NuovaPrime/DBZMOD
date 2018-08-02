﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace DBZMOD.UI
{
    internal enum ResourceBarMode
    {
        KI
    }
    class ResourceBar : UIElement
    {
        private ResourceBarMode stat;
        private float width;
        private float height;

        public ResourceBar(ResourceBarMode stat, int height, int width)
        {
            this.stat = stat;
            this.width = width;
            this.height = height;
        }
        private UIFlatPanel currentBar;
        private UIText text;

        public override void OnInitialize()
        {
            Height.Set(height, 0f); //Set Height of element
            Width.Set(width, 0f);   //Set Width of element

            UIFlatPanel barBackground = new UIFlatPanel(); //Create gray background
            barBackground.Left.Set(0f, 0f);
            barBackground.Top.Set(0f, 0f);
            barBackground.backgroundColor = new Color(0f, 0f, 0f, 0);
            barBackground.Width.Set(width, 0f);
            barBackground.Height.Set(height, 0f);

            currentBar = new UIFlatPanel(); //Create current value panel 
            currentBar.SetPadding(0);
            currentBar.Left.Set(0f, 0f);
            currentBar.Top.Set(0f, 0f);
            currentBar.Width.Set(width, 0f);
            currentBar.Height.Set(height, 0f);


            //assign color to panel depending on stat
            switch (stat)
            {
                case ResourceBarMode.KI:
                    currentBar.backgroundColor = new Color(0, 255, 255); //blue
                    break;
                default:
                    break;
            }

            text = new UIText("0/0"); //text to show current hp or mana
            text.Width.Set(width, 0f);
            text.Height.Set(height, 0f);
            text.Top.Set(height / 2 + 10, 0f); //center the UIText
            text.Left.Set(width - 100, 0f);
            
                        
            barBackground.Append(currentBar);
            barBackground.Append(text);
            base.Append(barBackground);
        }
        
            
        public override void Draw(SpriteBatch spriteBatch)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            float quotient = 1f;
            //Calculate quotient
            switch (stat)
            {
                case ResourceBarMode.KI:
                    quotient = (float)player.KiCurrent / (float)player.KiMax;
                    break;

                default:
                    break;
            }
            currentBar.Width.Set(quotient * width, 0f);
            Recalculate(); // recalculate the position and size

            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>(); //Get Player
            switch (stat)
            {
                case ResourceBarMode.KI:
                    text.SetText("Ki:" + player.KiCurrent + " / " + player.KiMax); //Set Life
                    break;

                default:
                    break;
            }
            base.Update(gameTime);
        }
        
    }
}