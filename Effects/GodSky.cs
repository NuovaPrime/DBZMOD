using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;

namespace DBZMOD.Effects
{
	public class GodSky : CustomSky
	{
		private bool active;

		public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
		}

		private float GetIntensity()
		{
			return 1f - Utils.SmoothStep(3000f, 6000f, 200f);
		}

		public override Color OnTileColor(Color inColor)
		{
			float intensity = GetIntensity();
			return new Color(Vector4.Lerp(new Vector4(0.5f, 0.8f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0f && minDepth < 0f)
			{
				float intensity = GetIntensity();
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			active = true;
		}

		public override void Deactivate(params object[] args)
		{
			active = false;
		}

		public override void Reset()
		{
			active = false;
		}

		public override bool IsActive()
		{
			return active;
		}
	}
}