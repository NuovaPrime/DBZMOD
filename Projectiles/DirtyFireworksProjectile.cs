﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class DirtyFireworksProjectile : KiProjectile
    {
        NPC targetNPC = null;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dirty Fireworks");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
			projectile.light = 1f;
			projectile.aiStyle = 1;
			aiType = 14;
            projectile.friendly = true;
			projectile.extraUpdates = 0;
            projectile.ignoreWater = true;
            projectile.penetrate = 12;
            projectile.timeLeft = 200;
			projectile.tileCollide = false;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
            //projectile.hide = true;
        }

		 public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 0, 0, 50);
        }
		
		public override void Kill(int timeLeft)
        {
            if (!projectile.active)
            {
                return;
            }

            if(targetNPC != null)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BigBangAttackProjectile2"), 100, 30, projectile.owner, 0f, 1f);

                ExplodeEffect();
            }

            projectile.active = false;
			}
		
		public override void AI()
        {
            if(Main.myPlayer == projectile.owner)
            {
                if (targetNPC == null)
                {
                    if (Vector2.Distance(projectile.position, Main.MouseWorld) > 0.1)
                    {
                        projectile.velocity = Vector2.Normalize(Main.MouseWorld - projectile.position) * 6;
                    }
                    else
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                }


                projectile.netUpdate = true;
            }

            if(targetNPC != null)
            {
                if(targetNPC.life <= 0)
                {
                    Kill(0);
                }
                else
                {
                    //targetNPC.position = Main.MouseWorld;
                    targetNPC.velocity.Y = -2.5f;
                    targetNPC.velocity.X = 0;

                    projectile.position = targetNPC.position;
                }
            }


            //for(int i = 0; i < 10; i++)
            if(projectile.timeLeft % 2 == 0)
            {
                Dust tDust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 35, 0f, 0f, 213, Color.Red, 1.0f);
                tDust.velocity = Vector2.Zero;
                tDust.noGravity = true;
            }

        }

        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            if(targetNPC == null)
            {
                base.OnHitNPC(npc, 0, 0, crit);
                targetNPC = npc;
                projectile.damage = 0;
                projectile.hide = true;
                projectile.timeLeft = 100;
            }

            //Main.NewText("NPC HIT", Color.White);
        }

        public void ExplodeEffect()
        {
            projectile.ai[1] = 0f;
            projectile.alpha = 255;

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 22;
            projectile.height = 22;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.knockBack = 8f;
            projectile.Damage();

            Main.projectileIdentity[projectile.owner, projectile.identity] = -1;
            int num = projectile.timeLeft;
            projectile.timeLeft = 0;

            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Kiplosion").WithVolume(1.0f));

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 22;
            projectile.height = 22;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for (int num615 = 0; num615 < 30; num615++)
            {
                int num616 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num616].velocity *= 1.4f;
            }
            for (int num617 = 0; num617 < 20; num617++)
            {
                int num618 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3.5f);
                Main.dust[num618].noGravity = true;
                Main.dust[num618].velocity *= 7f;
                num618 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num618].velocity *= 3f;
            }
            for (int num619 = 0; num619 < 2; num619++)
            {
                float scaleFactor9 = 3f;
                if (num619 == 1)
                {
                    scaleFactor9 = 3f;
                }
                int num620 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore97 = Main.gore[num620];
                gore97.velocity.X = gore97.velocity.X + 1f;
                Gore gore98 = Main.gore[num620];
                gore98.velocity.Y = gore98.velocity.Y + 1f;
                num620 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore99 = Main.gore[num620];
                gore99.velocity.X = gore99.velocity.X - 1f;
                Gore gore100 = Main.gore[num620];
                gore100.velocity.Y = gore100.velocity.Y + 1f;
                num620 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore101 = Main.gore[num620];
                gore101.velocity.X = gore101.velocity.X + 1f;
                Gore gore102 = Main.gore[num620];
                gore102.velocity.Y = gore102.velocity.Y - 1f;
                num620 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore103 = Main.gore[num620];
                gore103.velocity.X = gore103.velocity.X - 1f;
                Gore gore104 = Main.gore[num620];
                gore104.velocity.Y = gore104.velocity.Y - 1f;
            }
        }
    }
}