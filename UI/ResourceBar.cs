using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using DBZMOD.Util;

namespace DBZMOD.UI
{
	internal enum ResourceBarMode
	{
		KI,
        OVERLOAD
	}
	class ResourceBar : UIElement
	{
		private ResourceBarMode stat;
		private float width;
		private float height;
		private int FrameTimer;
		private int FrameTimer2;
		public ResourceBar(ResourceBarMode stat, int height, int width)
		{
			this.stat = stat;
			this.width = width;
			this.height = height;
		}
		private UIText text;

		Rectangle barDestination;
        Vector2 drawPosition;
        private Color gradientA;
		private Color gradientB;
        public Texture2D texture;

        public override void OnInitialize()
		{
			Height.Set(height, 0f); //Set Height of element
			Width.Set(width, 0f);   //Set Width of element

			//assign color to panel depending on stat
			switch (stat)
			{
				case ResourceBarMode.KI:
                    gradientA = new Color(0, 208, 255); // light blue
                    gradientB = new Color(0, 80, 255); // dark blue
                    break;
                case ResourceBarMode.OVERLOAD:
                    gradientA = new Color(1, 168, 1); //dark green
                    gradientB = new Color(35, 237, 35); // light green
                    break;

                default:
					break;
			}

			text = new UIText("0/0"); //text to show stat
			text.Width.Set(width, 0f);
			text.Height.Set(height, 0f);
			text.Top.Set(height / 2 + 10, 0f); //center the UIText
			text.Left.Set(width - 60, 0f);

			Append(text);

			barDestination = new Rectangle(20, 0, (int)width, (int)height);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
			float quotient = 1f;
            float averageKi = (float)Math.Floor(cleanAverageKi.Sum() / 15f);
            //Calculate quotient
            switch (stat)
			{
				case ResourceBarMode.KI:
					quotient = averageKi / player.OverallKiMax();
					quotient = Utils.Clamp(quotient, 0, 1);
					break;
                case ResourceBarMode.OVERLOAD:
                    quotient = player.OverloadCurrent / player.OverloadMax;
                    quotient = Utils.Clamp(quotient, 0, 1);
                    break;

                default:
					break;
			}

			Rectangle hitbox = GetInnerDimensions().ToRectangle();
			hitbox.X += barDestination.X;
			hitbox.Y += barDestination.Y;
			hitbox.Width = barDestination.Width;
			hitbox.Height = barDestination.Height;
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			for (int i = 0; i < steps; i += 1)
			{
				//float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				spriteBatch.Draw(Main.magicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
			FrameTimer++;
			if (FrameTimer >= 20)
            {
                FrameTimer = 0;
            }
			int FrameHeight = GFX.KiBar.Height / 4;
			int frame = FrameTimer / 5;
            switch (stat)
            {
                case ResourceBarMode.KI:
                    texture = GFX.KiBar;
                    drawPosition = new Vector2(hitbox.X - 36, hitbox.Y - 4);
                    break;
                case ResourceBarMode.OVERLOAD:
                    texture = GFX.OverloadBar;
                    drawPosition = new Vector2(hitbox.X - 36, hitbox.Y - 4);
                    break;

                default: texture = null;
                    break;
            }
            Rectangle sourceRectangle = new Rectangle(0, FrameHeight * frame, GFX.KiBar.Width, FrameHeight);
			spriteBatch.Draw(texture, drawPosition, sourceRectangle, Color.White);

			FrameTimer2 += 3;
			if (FrameTimer2 >= 15)
            {
                FrameTimer2 = 0;
            }
			if(Transformations.IsPlayerTransformed(player.player))
			{
				Vector2 drawPosition2 = new Vector2(hitbox.X - 32, hitbox.Y - 10);
				int FrameHeight2 = GFX.KiBarLightning.Height / 3;
				int frame2 = FrameTimer / 5;
				Rectangle sourceRectangle2 = new Rectangle(0, FrameHeight2 * frame2, GFX.KiBarLightning.Width, FrameHeight2);
				spriteBatch.Draw(GFX.KiBarLightning, drawPosition2, sourceRectangle2, Color.White);
			}
			
		}

        private static List<float> cleanAverageKi = new List<float>();
		public override void Update(GameTime gameTime)
		{            
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;

            // the point of this is to get a one second average of the ki changes. This makes the ki bar stabilize instead of flickering so goddamn much.
            cleanAverageKi.Add(modplayer.GetKi());
            if (cleanAverageKi.Count > 15)
            {
                cleanAverageKi.RemoveRange(0, cleanAverageKi.Count - 15);
            }

            int averageKi = (int)Math.Floor(cleanAverageKi.Sum() / 15f);

            switch (stat)
			{
				case ResourceBarMode.KI:
                    text.SetText("Ki:" + averageKi + " / " + modplayer.OverallKiMax());
                    if (modplayer.playerTrait == "Legendary")
                    {
                        gradientA = new Color(0, 254, 0);
                        gradientB = new Color(0, 122, 91);
                    }
                    else if (modplayer.playerTrait == "Prodigy")
                    {
                        gradientA = new Color(0, 150, 255);
                        gradientB = new Color(0, 20, 255);
                    }
                    else if (modplayer.playerTrait == "Divine")
                    {
                        gradientA = new Color(15, 0, 24);
                        gradientB = new Color(106, 62, 183);
                    }
                    else if (modplayer.playerTrait == "Primal")
                    {
                        gradientA = new Color(198, 19, 46);
                        gradientB = new Color(255, 151, 0);
                    }
                    else if (Transformations.IsSSJG(player))
                    {
                        gradientA = new Color(175, 45, 63);
                        gradientB = new Color(255, 116, 48);
                    }
                    else
                    {
                        gradientA = new Color(0, 208, 255);
                        gradientB = new Color(0, 80, 255);
                    }
                    break;

                case ResourceBarMode.OVERLOAD:
                    text.SetText("Overload:" + modplayer.OverloadCurrent + " / " + modplayer.OverloadMax);
                    gradientA = new Color(1, 168, 1); //dark green
                    gradientB = new Color(35, 237, 35); // light green
                    break;
                    break;

                default:
					break;
			}
            
			base.Update(gameTime);
		}

	}
}