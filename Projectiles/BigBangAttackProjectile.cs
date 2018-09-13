using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
	
	public class BigBangAttackProjectile : KiProjectile
	{
		
		public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
			projectile.tileCollide = false;
            projectile.width = 120;
            projectile.height = 120;
			projectile.aiStyle = 1;
			projectile.light = 1f;
			projectile.timeLeft = 170;
            projectile.ignoreWater = true;
            aiType = 14;
            projectile.netUpdate = true;
            projectile.penetrate = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
		
		public override void Kill(int timeLeft)
        {
            for (int i = 2; i < 3; i++) //Makes it so that 3 projectiles split from this projectile
            {
                int velo = Main.rand.Next(1, 1); //determines the X velocity
                int velo1 = Main.rand.Next(4, 4); //determines the Y velocity //determines the Y velocity
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velo, velo1, mod.ProjectileType("BigBangAttackProjectile2"), projectile.damage / 2, 30, projectile.owner, 0f, 1f); 
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velo, velo1, mod.ProjectileType("BigBangAttackProjectile2"), projectile.damage / 2, 30, projectile.owner, 0f, 1f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velo, velo1, mod.ProjectileType("BigBangAttackProjectile2"), projectile.damage / 2, 30, projectile.owner, 0f, 1f); 
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velo, velo1, mod.ProjectileType("BigBangAttackProjectile2"), projectile.damage / 2, 20, projectile.owner, 0f, 1f);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velo, velo1, mod.ProjectileType("BigBangAttackProjectile2"), projectile.damage / 2, 20, projectile.owner, 0f, 1f);
            }
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
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BigBangAttackProjectile");
        }
	}
}