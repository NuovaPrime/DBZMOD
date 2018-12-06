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
	public class MasenkoBlast : KiProjectile
	{
		public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SwordBeam);
            projectile.hostile = false;
            projectile.friendly = true;
			projectile.tileCollide = false;
            projectile.width = 64;
            projectile.height = 48;
			projectile.aiStyle = 1;
			projectile.light = 1f;
            projectile.knockBack = DefaultBeamKnockback;
            projectile.timeLeft = 250;
			projectile.damage = 32;
			aiType = 14;
            projectile.ignoreWater = true;
			projectile.penetrate = -1;
            projectile.netUpdate = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MasenkoBlast");
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
   
        public override void AI()
        {
			 projectile.ai[1]++;
            if(projectile.ai[1] == 3)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 0, 0, 0, mod.ProjectileType("MasenkoTrail"), projectile.damage / 2, 4f, projectile.owner, 0, projectile.rotation);
                projectile.ai[1] = 0;
                projectile.netUpdate = true;
            }
        }
	}
}