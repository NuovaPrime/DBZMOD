using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBZMOD.Projectiles
{
    public class EnergyShotBlast : KiProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 40;
            projectile.timeLeft = 100;
            projectile.penetrate = 200;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.aiStyle = 101;
            projectile.light = 1f;
            projectile.stepSpeed = 13;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
        }
		public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, 110);
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				Dust dust = Main.dust[Terraria.Dust.NewDust(projectile.position, 26, 26, 222, projectile.velocity.X, projectile.velocity.Y, 0, new Color(255,255,255), 1f)];
				dust.noGravity = true;
			}
		}
		public override void AI()
		{
			projectile.velocity.X *= 1.005f;
			projectile.velocity.Y *= 1.005f;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
    }
}