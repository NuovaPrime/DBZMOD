using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using DBZMOD.Util;

namespace DBZMOD.Projectiles
{
    public class HellzoneGrenadeProjectile : KiProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("HellzoneGrenadeProjectile");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
			projectile.light = 1f;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.netUpdate = true;
            projectile.timeLeft = 280;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void AI()
        {
            if (Main.rand.Next(4) == 0)
            {
            	projectile.velocity *= 0.9f;
            }
        }
		
		public override Color? GetAlpha(Color lightColor)
        {
			if (projectile.timeLeft < 85) 
			{
				byte b2 = (byte)(projectile.timeLeft * 3);
				byte a2 = (byte)(100f * ((float)b2 / 255f));
				return new Color((int)b2, (int)b2, (int)b2, (int)a2);
			}
			return new Color(255, 255, 255, 100);
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
		
        public override void Kill(int timeLeft)
        {
            SoundHelper.PlayVanillaSound(2, projectile.position, 122);
        	int num251 = 1;
        	if (projectile.owner == Main.myPlayer)
        	{
				for (int num252 = 0; num252 < num251; num252++)
				{
					Vector2 value15 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					while (value15.X == 0f && value15.Y == 0f)
					{
						value15 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					}
					value15.Normalize();
					value15 *= (float)Main.rand.Next(70, 101) * 0.1f;
					Projectile.NewProjectile(projectile.oldPosition.X + (float)(projectile.width / 2), projectile.oldPosition.Y + (float)(projectile.height / 2), value15.X, value15.Y, mod.ProjectileType("HellzoneGrenadeProjectile2"), (int)((double)projectile.damage), 0f, projectile.owner, 0f, 0f);
			    }
        	}
        }
    }
}
		