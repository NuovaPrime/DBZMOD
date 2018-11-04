using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class DestructoDiskAssaultProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shinkienzan");
        }
        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 54;
            projectile.timeLeft = 100;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.aiStyle = 56; //perfect ai for gravless and rotation, useful for disks 
            projectile.light = 3f;
            projectile.stepSpeed = 13; ;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(target.Center.X - 40, target.Center.Y + 20, 0, 0, mod.ProjectileType("DestructoDiskAssaultMini"), projectile.damage, 3f);
            Projectile.NewProjectile(target.Center.X - 40, target.Center.Y + 10, 0, 0, mod.ProjectileType("DestructoDiskAssaultMini"), projectile.damage, 3f);
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
        public override void PostAI()
        {
            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 1f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 52, 52, 222, 0f, 0f, 0, new Color(255, 255, 255), 0.7236842f);
                    dust.noGravity = true;
                }
            }
        }
    }
}