using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Effects
{

    public class Circle : ModProjectile
    {
		private float scaletime;
        public override void SetDefaults()
        {
            projectile.width = 1214;
            projectile.height = 1214;
            projectile.timeLeft = 2000;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.aiStyle = 101;
            projectile.light = 1f;
            projectile.stepSpeed = 13;
            projectile.netUpdate = true;
			scaletime = 50;
			projectile.alpha = 255;
        }
		public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, projectile.alpha);
        }
		
		public override void AI()
		{
			float scalemult = 2;
			scalemult += 0.1f; 
			Player player = Main.player[projectile.owner];
			projectile.Center = player.Center;
			projectile.alpha -= 5;
			if (scaletime > 0)
            {
                projectile.scale = (scaletime / 50f) * scalemult;
                scaletime--;
            }
			else
            {
                projectile.Kill();
            }
		}
		
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            int radius = (int)Math.Ceiling(projectile.width / 2f * projectile.scale);
            DBZMOD.circle.ApplyShader(radius);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}