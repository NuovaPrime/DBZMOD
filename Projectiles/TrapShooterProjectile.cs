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
    public class TrapShooterProjectile : KiProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TrapShooterProjectile");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 20;
            projectile.aiStyle = 17;
            projectile.light = 1f;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 120;
            projectile.aiStyle = 1;
            aiType = 14;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 0, 0, 0, mod.ProjectileType("TrapShooterExplosion"), projectile.damage, 4f, projectile.owner, 0, projectile.rotation);
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