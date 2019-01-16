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
    public class SupernovaBall : KiProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Supernova Ball");
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

        private float HeldTime
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }


        private bool isInitialized = false;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (!isInitialized)
            {
                HeldTime = 1;
                projectile.scale = 0.15f;
                isInitialized = true;
            }

            // cancel channeling if the projectile is maxed
            if (player.channel && projectile.scale > 2.5)
            {
                player.channel = false;
            }

            if (player.channel && HeldTime > 0)
            {
                projectile.scale += 0.005f;
                Vector2 projectileOffset = new Vector2(-projectile.width * 0.5f, -projectile.height * 0.5f);
                projectileOffset += new Vector2(0, -(80 + projectile.scale * 115f));
                projectile.position = player.Center + projectileOffset;

                projectile.netUpdate = true;

                //Rock effect
                projectile.ai[1]++;
                if (projectile.ai[1] % 7 == 0)
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
                Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);
                
                if (projectile.timeLeft < 399)
                {
                    projectile.timeLeft = 400;
                }

                projectile.netUpdate2 = true;

                MyPlayer.ModPlayer(player).AddKi(-2, true, false);
                ProjectileUtil.ApplyChannelingSlowdown(player);

                // depleted check, release the ball
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
                }
            }
            else if (HeldTime > 0)
            {
                HeldTime = 0;
                projectile.timeLeft = (int)Math.Ceiling(projectile.scale * 15) + 600;
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 6;
                projectile.tileCollide = false;
                projectile.damage *= (int)Math.Ceiling(projectile.scale * 3f);
            }            
        }

        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            projectile.scale -= 0.025f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float radius = (float)projectile.width * projectile.scale / 2f;
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