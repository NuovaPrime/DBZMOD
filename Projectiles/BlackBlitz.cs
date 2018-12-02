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
    public class BlackBlitz : KiProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 6;
            projectile.light = 1f;
            projectile.timeLeft = 10;
            projectile.ignoreWater = true;
            projectile.netUpdate = true;
            projectile.penetrate = 1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void PostAI()
        {
            for (int d = 0; d < 0.2; d++)
            {
                if (Main.rand.NextFloat() < 1f)
                {
                    Dust dust = Main.dust[Terraria.Dust.NewDust(projectile.position, 16, 16, 54, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                }
                if (Main.rand.NextFloat() < 0.1f)
                {
                    Dust dust = Main.dust[Terraria.Dust.NewDust(projectile.position, 16, 16, 107, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                }
            }
        }
        public override void AI()
        {
            projectile.rotation = Vector2.Normalize(projectile.velocity).ToRotation() + (float)(Math.PI / 2);
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X * 5, projectile.velocity.Y * 5, mod.ProjectileType("BlackBlitzLong"), projectile.damage, 30, projectile.owner, 0f, 1f);
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