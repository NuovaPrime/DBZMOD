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
		Ki,
        Overload
	}
	class ResourceBar : UIElement
	{
		private ResourceBarMode _stat;
		private float _width;
		private float _height;
		private int _frameTimer;
		private int _frameTimer2;
		public ResourceBar(ResourceBarMode stat, int height, int width)
		{
			this._stat = stat;
			this._width = width;
			this._height = height;
		}
		private UIText _text;

		Rectangle _barDestination;
        Vector2 _drawPosition;
        private Color _gradientA;
		private Color _gradientB;
        public Texture2D texture;

        public override void OnInitialize()
		{
			Height.Set(_height, 0f); //Set Height of element
			Width.Set(_width, 0f);   //Set Width of element

			//assign color to panel depending on stat
			switch (_stat)
			{
				case ResourceBarMode.Ki:
                    _gradientA = new Color(86, 238, 242); // light blue
                    _gradientB = new Color(53, 146, 183); // dark blue
                    break;
                case ResourceBarMode.Overload:
                    _gradientA = new Color(221, 255, 28); //light green
                    _gradientB = new Color(70, 150, 93); // dark green
                    break;

                default:
					break;
			}

			_text = new UIText("0/0"); //text to show stat
			_text.Width.Set(_width, 0f);
			_text.Height.Set(_height, 0f);
			_text.Top.Set(_height / 2 + 10, 0f); //center the UIText
			_text.Left.Set(_width - 60, 0f);

			Append(_text);

			_barDestination = new Rectangle(20, 0, (int)_width, (int)_height);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
			float quotient = 1f;
            float averageKi = (float)Math.Floor(_cleanAverageKi.Sum() / 15f);
            //Calculate quotient
            switch (_stat)
			{
				case ResourceBarMode.Ki:
					quotient = averageKi / player.OverallKiMax();
					quotient = Utils.Clamp(quotient, 0, 1);
					break;
                case ResourceBarMode.Overload:
                    quotient = player.overloadCurrent / player.overloadMax;
                    quotient = Utils.Clamp(quotient, 0, 1);
                    break;

                default:
					break;
			}

			Rectangle hitbox = GetInnerDimensions().ToRectangle();
			hitbox.X += _barDestination.X;
			hitbox.Y += _barDestination.Y;
			hitbox.Width = _barDestination.Width;
			hitbox.Height = _barDestination.Height;
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			for (int i = 0; i < steps; i += 1)
			{
				//float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				spriteBatch.Draw(Main.magicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(_gradientA, _gradientB, percent));
			}
			_frameTimer++;
			if (_frameTimer >= 20)
            {
                _frameTimer = 0;
            }
			int frameHeight = Gfx.kiBar.Height / 4;
			int frame = _frameTimer / 5;
            switch (_stat)
            {
                case ResourceBarMode.Ki:
                    texture = Gfx.kiBar;
                    _drawPosition = new Vector2(hitbox.X - 36, hitbox.Y - 4);
                    break;
                case ResourceBarMode.Overload:
                    texture = Gfx.overloadBar;
                    _drawPosition = new Vector2(hitbox.X - 36, hitbox.Y - 4);
                    break;

                default: texture = null;
                    break;
            }
            Rectangle sourceRectangle = new Rectangle(0, frameHeight * frame, Gfx.kiBar.Width, frameHeight);
			spriteBatch.Draw(texture, _drawPosition, sourceRectangle, Color.White);

			_frameTimer2 += 3;
			if (_frameTimer2 >= 15)
            {
                _frameTimer2 = 0;
            }
			if(TransformationHelper.IsPlayerTransformed(player.player))
			{
				Vector2 drawPosition2 = new Vector2(hitbox.X - 32, hitbox.Y - 10);
				int frameHeight2 = Gfx.kiBarLightning.Height / 3;
				int frame2 = _frameTimer / 5;
				Rectangle sourceRectangle2 = new Rectangle(0, frameHeight2 * frame2, Gfx.kiBarLightning.Width, frameHeight2);
				spriteBatch.Draw(Gfx.kiBarLightning, drawPosition2, sourceRectangle2, Color.White);
			}
			
		}

        private static List<float> _cleanAverageKi = new List<float>();
		public override void Update(GameTime gameTime)
		{            
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;

            // the point of this is to get a one second average of the ki changes. This makes the ki bar stabilize instead of flickering so goddamn much.
            _cleanAverageKi.Add(modplayer.GetKi());
            if (_cleanAverageKi.Count > 15)
            {
                _cleanAverageKi.RemoveRange(0, _cleanAverageKi.Count - 15);
            }

            int averageKi = (int)Math.Floor(_cleanAverageKi.Sum() / 15f);

            switch (_stat)
			{
				case ResourceBarMode.Ki:
                    _text.SetText("Ki:" + averageKi + " / " + modplayer.OverallKiMax()); //a is light b is dark
                    if (modplayer.playerTrait == "Legendary")
                    {
						_gradientA = new Color(221, 255, 28);
						_gradientB = new Color(70, 150, 93);
                    }
                    else if (modplayer.playerTrait == "Prodigy")
                    {
                        _gradientA = new Color(0, 104, 249);
                        _gradientB = new Color(7, 28, 76);
                    }
                    else if (modplayer.playerTrait == "Divine")
                    {
                        _gradientA = new Color(163, 57, 136);
                        _gradientB = new Color(31, 0, 47);
                    }
                    else if (modplayer.playerTrait == "Primal")
                    {
                        _gradientA = new Color(255, 182, 0);
                        _gradientB = new Color(255, 92, 78);
                    }
                    else if (TransformationHelper.IsSSJG(player))
                    {
                        _gradientA = new Color(255, 140, 48);
                        _gradientB = new Color(175, 45, 63);
                    }
                    else
                    {
                        _gradientA = new Color(86, 238, 242);
						_gradientB = new Color(53, 146, 183);
                    }
                    break;

                case ResourceBarMode.Overload:
                    _text.SetText("Overload:" + modplayer.overloadCurrent + " / " + modplayer.overloadMax);
                    _gradientA = new Color(1, 168, 1); //dark green
                    _gradientB = new Color(35, 237, 35); // light green
                    break;
                    break;

                default:
					break;
			}
            
			base.Update(gameTime);
		}

	}
}