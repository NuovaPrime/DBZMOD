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
        public bool Released = false;
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
            projectile.extraUpdates = 0;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
            KiDrainRate = 12;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            /*if (projectile.timeLeft < 85) 
			{
				byte b2 = (byte)(projectile.timeLeft * 3);
				byte a2 = (byte)(100f * ((float)b2 / 255f));
				return new Color((int)b2, (int)b2, (int)b2, (int)a2);
			}*/
            return new Color(255, 255, 255, 100);
        }

        public override void Kill(int timeLeft)
        {
            if (!projectile.active)
            {
                return;
            }

            Projectile proj = Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), mod.ProjectileType("SupernovaExplosion"), projectile.damage, projectile.knockBack, projectile.owner);
            //proj.Hitbox.Inflate(1000, 1000);
            proj.width *= (int)projectile.scale;
            proj.height *= (int)projectile.scale;

            projectile.active = false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (Main.myPlayer == projectile.owner)
            {
                if (!Released)
                {
                    projectile.scale += 0.05f;

                    projectile.position = player.position + new Vector2(20, -80 - (projectile.scale * 17));

                    if (projectile.timeLeft < 399)
                    {
                        projectile.timeLeft = 400;
                    }
                    if (MyPlayer.ModPlayer(player).IsKiDepleted())
                    {
                        projectile.Kill();
                    }

                    MyPlayer.ModPlayer(player).AddKi(-2, true, false);
                    ProjectileUtil.ApplyChannelingSlowdown(player);

                    //Rock effect
                    projectile.ai[1]++;
                    if (projectile.ai[1] % 7 == 0)
                        Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);
                }

                projectile.netUpdate = true;
            }

            //if button let go
            if (!player.channel || projectile.scale > 25)
            {
                //projectile.Kill();
                if (!Released)
                {
                    Released = true;
                    projectile.velocity = Vector2.Normalize(Main.MouseWorld - projectile.position) * 6;
                    projectile.tileCollide = false;
                    projectile.damage *= (int)projectile.scale / 2;
                }

                //Projectile p = Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.Center.Y) - (projectile.velocity * 5), new Vector2(0, 0), mod.ProjectileType("EnergyWaveTrail"), projectile.damage / 3, 4f, projectile.owner, 0, projectile.rotation);
                //p.scale = projectile.scale * 1.5f;
            }

        }
        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            projectile.scale -= 0.25f;
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
            DBZMOD.Circle.ApplyShader(-9001);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
        }
    }
}