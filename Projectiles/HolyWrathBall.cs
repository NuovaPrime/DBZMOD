using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Util;

namespace DBZMOD.Projectiles
{
    public class HolyWrathBall : KiProjectile
    {
        public bool IsReleased = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Wrath Ball");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 226;
            projectile.height = 226;
            projectile.light = 1f;
            projectile.aiStyle = 1;
            aiType = 14;
            projectile.friendly = true;
            projectile.extraUpdates = 2;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            KiDrainRate = 12;
        }

        public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, 100);
        }

        private bool isInitialized = false;
        public override void AI()
        {
            if (!isInitialized)
            {
                projectile.scale = 0.15f;
                isInitialized = true;
            }

            Player player = Main.player[projectile.owner];

            // cancel channeling if the projectile is maxed
            if (player.channel && projectile.scale > 3.0)
            {
                player.channel = false;
            }

            if (player.channel && !IsReleased)
            {
                projectile.scale += 0.005f;
                Vector2 projectileOffset = new Vector2(-projectile.width * 0.5f, -projectile.height * 0.5f);
                projectileOffset += new Vector2(0, -(80 + projectile.scale * 115f));
                Vector2 projectileOuter = new Vector2(projectile.scale * projectile.width, projectile.scale * projectile.height);
                projectile.position = player.Center + projectileOffset;

                for (int d = 0; d < 25; d++)
                {
                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 dustPosition = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.Center + (dustPosition * projectileOuter) + projectileOffset, projectile.width, projectile.height, 15, 0f, 0f, 213, default(Color), 2.0f);
                    tDust.velocity = Vector2.Normalize((projectile.Center + projectile.Size / 2f) - tDust.position) * 2;
                    tDust.noGravity = true;
                }

                //Rock effect
                projectile.ai[1]++;
                if (projectile.ai[1] % 7 == 0)
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
                Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);


                projectile.netUpdate = true;

                if (projectile.timeLeft < 399)
                {
                    projectile.timeLeft = 400;
                }

                MyPlayer.ModPlayer(player).AddKi(-2, true, false);
                ProjectileUtil.ApplyChannelingSlowdown(player);

                projectile.netUpdate2 = true;

                // depleted check, release the ball
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
                }
            }
            else if (!IsReleased)
            {
                projectile.timeLeft = (int)Math.Ceiling(projectile.scale * 15) + 600;
                IsReleased = true;
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 6;
                projectile.tileCollide = false;
                projectile.damage *= (int)Math.Ceiling(projectile.scale * 4f);
            }
        }

        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            projectile.scale -= 0.025f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float radius = projectile.width * projectile.scale / 2f;
            float rSquared = radius * radius;

            return rSquared > Vector2.DistanceSquared(Vector2.Clamp(projectile.Center, targetHitbox.TopLeft(), targetHitbox.BottomRight()), projectile.Center);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            int radius = (int)Math.Ceiling(projectile.width / 2f * projectile.scale);
            DBZMOD.Circle.ApplyShader(radius);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}