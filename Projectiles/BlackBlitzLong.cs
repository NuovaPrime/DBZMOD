using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Projectiles
{
    public class BlackBlitzLong : KiProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.width = 28;
            projectile.height = 108;
            projectile.aiStyle = 0;
            projectile.light = 1f;
            projectile.timeLeft = 720;
            projectile.ignoreWater = true;
            projectile.netUpdate = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override bool PreAI()
        {
            projectile.rotation = Vector2.Normalize(projectile.velocity).ToRotation() + (float)(Math.PI / 2);
            return true;
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
            projectile.velocity.Y *= 1.05f;
            projectile.velocity.X *= 1.05f;
            if (projectile.timeLeft < 717)
            {
                projectile.alpha = 0;
            }
        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            projectile.alpha++;
            projectile.velocity.X /= 2;
            projectile.velocity.Y /= 2;
            if (projectile.alpha < 255)
            {
                projectile.active = false;
            }
            return false;
        }
    }
}