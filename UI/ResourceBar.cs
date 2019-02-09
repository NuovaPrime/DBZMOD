﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using DBZMOD.Extensions;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using DBZMOD.Util;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

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
        private int _segments;
        public Texture2D texture;

        private int GetSegmentsBasedOnResourceMode()
        {
            switch (_stat)
            {
                case ResourceBarMode.Ki:
                    return 8;
                case ResourceBarMode.Overload:
                    return 6;
                default:
                    return 0;
            }
        }

        private Rectangle GetBarDestinationBasedOnResourceMode()
        {

            switch (_stat)
            {
                case ResourceBarMode.Ki:
                    return new Rectangle(20, 0, (int)_width, (int)_height);
                case ResourceBarMode.Overload:
                    return new Rectangle(15, 0, (int)_width, (int)_height);
                default:
                    return Rectangle.Empty;
            }
        }

        public override void OnInitialize()
		{
			Height.Set(_height, 0f); //Set Height of element
			Width.Set(_width, 0f);   //Set Width of element

            _segments = GetSegmentsBasedOnResourceMode();

			_text = new UIText("0/0"); //text to show stat
			_text.Width.Set(_width, 0f);
			_text.Height.Set(_height, 0f);
			_text.Top.Set(_height / 2 + 10, 0f); //center the UIText
			_text.Left.Set(_width - 60, 0f);

			Append(_text);

            _barDestination = GetBarDestinationBasedOnResourceMode();
		}

        private float GetPlayerResourceQuotient(MyPlayer player)
        {
            float averageResource = (float)Math.Floor(_cleanAverageKi.Sum() / 15f);
            //Calculate quotient
            switch (_stat)
            {
                case ResourceBarMode.Ki:
                    return Utils.Clamp(averageResource / player.OverallKiMax(), 0, 1);
                case ResourceBarMode.Overload:
                    return Utils.Clamp(averageResource / player.overloadMax, 0, 1);
                default:
                    return 0f;
            }
        }

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
			float quotient = GetPlayerResourceQuotient(player);

			Rectangle hitbox = GetInnerDimensions().ToRectangle();
			hitbox.X += _barDestination.X;
			hitbox.Y += _barDestination.Y;
			hitbox.Width = _barDestination.Width;
			hitbox.Height = _barDestination.Height;
			_frameTimer++;
			if (_frameTimer >= 20)
            {
                _frameTimer = 0;
            }

            int frameHeight = 0;
			int frame = _frameTimer / 5;
            Vector2 textureOffset = Vector2.Zero;
            switch (_stat)
            {
                case ResourceBarMode.Ki:
                    texture = Gfx.GetKiBar(player).KiBarFrame;
                    frameHeight = texture.Height / 4;
                    textureOffset = new Vector2(16, 8);
                    _drawPosition = new Vector2(hitbox.X - 36, hitbox.Y - 4);
                    break;
                case ResourceBarMode.Overload:
                    texture = Gfx.overloadBarFrame;
                    frameHeight = texture.Height / 4;
                    textureOffset = new Vector2(18, 6);
                    _drawPosition = new Vector2(hitbox.X - 36, hitbox.Y - 4);
                    break;

                default: texture = null;
                    break;
            }
            Rectangle sourceRectangle = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
			spriteBatch.Draw(texture, _drawPosition, sourceRectangle, Color.White);

			//_frameTimer2 += 3;
			//if (_frameTimer2 >= 15)
   //         {
   //             _frameTimer2 = 0;
   //         }
            //if(player.player.IsPlayerTransformed())
            //{
            //             switch (_stat)
            //             {
            //                 case ResourceBarMode.Ki:
            //                     Vector2 drawPosition2 = new Vector2(hitbox.X - 32, hitbox.Y - 10);
            //                     int frameHeight2 = Gfx.kiBarLightning.Height / 3;
            //                     int frame2 = _frameTimer / 5;
            //                     Rectangle sourceRectangle2 = new Rectangle(0, frameHeight2 * frame2, Gfx.kiBarLightning.Width, frameHeight2);
            //                     spriteBatch.Draw(Gfx.kiBarLightning, drawPosition2, sourceRectangle2, Color.White);
            //                     break;
            //                 case ResourceBarMode.Overload:
            //                     break;
            //             }	
            //}

            Texture2D barSegmentTexture = null;
            switch (_stat)
            {
                case ResourceBarMode.Ki:
                    barSegmentTexture = Gfx.GetKiBar(player).KiBarSegment;
                    break;
                case ResourceBarMode.Overload:
                    barSegmentTexture = Gfx.overloadBarSegment;
                    break;
            }

            // pessimistic doj
            if (barSegmentTexture != null)
            {
                // draw the segments last.
                int actualSegments = (int) Math.Ceiling(_segments * quotient);
                for (int i = 0; i < actualSegments; i += 1)
                {
                    Vector2 segmentOffsetX = new Vector2(i * 12, 0);
                    Vector2 segmentPosition = _drawPosition + textureOffset + segmentOffsetX;
                    if (i == actualSegments - 1)
                    {
                        float segmentValue = 1f / _segments;
                        float segmentRemainder = quotient % segmentValue;
                        float segmentQuotient = segmentRemainder / segmentValue;
                        // if there's no remainder, it's a full segment, render the whole thing.
                        if (segmentQuotient == 0f)
                        {
                            segmentQuotient = 1f;
                        }
                        // we're on a partial segment, render the whole thing.
                        spriteBatch.Draw(barSegmentTexture, segmentPosition, new Rectangle(0, 0, (int) Math.Ceiling(barSegmentTexture.Width * segmentQuotient), barSegmentTexture.Height), Color.White);
                    }
                    else
                    {
                        // we're on a full segment, render the whole thing.
                        spriteBatch.Draw(barSegmentTexture, segmentPosition, new Rectangle(0, 0, barSegmentTexture.Width, barSegmentTexture.Height), Color.White);
                    }
                }
            }
        }

        private static List<float> _cleanAverageKi = new List<float>();
        private static List<float> _cleanAverageOverload = new List<float>();
        public override void Update(GameTime gameTime)
		{            
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            switch (_stat)
			{
				case ResourceBarMode.Ki:
                    // the point of this is to get a one second average of the ki changes. This makes the ki bar stabilize instead of flickering so goddamn much.
                    _cleanAverageKi.Add(modPlayer.GetKi());
                    if (_cleanAverageKi.Count > 15)
                    {
                        _cleanAverageKi.RemoveRange(0, _cleanAverageKi.Count - 15);
                    }

                    int averageKi = (int)Math.Floor(_cleanAverageKi.Sum() / 15f);

                    _text.SetText("Ki:" + averageKi + " / " + modPlayer.OverallKiMax());
                    break;
                case ResourceBarMode.Overload:

                    // the point of this is to get a one second average of the ki changes. This makes the ki bar stabilize instead of flickering so goddamn much.
                    _cleanAverageOverload.Add(modPlayer.overloadCurrent);
                    if (_cleanAverageOverload.Count > 15)
                    {
                        _cleanAverageOverload.RemoveRange(0, _cleanAverageOverload.Count - 15);
                    }

                    int averageOverload = (int)Math.Floor(_cleanAverageOverload.Sum() / 15f);

                    _text.SetText("Overload:" + modPlayer.overloadCurrent + " / " + modPlayer.overloadMax);
                    break;

                default:
					break;
			}
            
			base.Update(gameTime);
		}

	}
}