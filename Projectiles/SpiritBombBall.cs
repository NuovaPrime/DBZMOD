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
    public class SpiritBombBall : KiProjectile
    {
        public bool Released = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SpiritBombBall");
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
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
            projectile.ranged = true;

        }

        public override Color? GetAlpha(Color lightColor)
        {
			if (projectile.timeLeft < 85) 
			{
				byte b2 = (byte)(projectile.timeLeft * 3);
				byte a2 = (byte)(100f * ((float)b2 / 255f));
				return new Color((int)b2, (int)b2, (int)b2, (int)a2);
			}
			return new Color(255, 255, 255, 100);
            //if (projectile.timeLeft < 85) 
            //{
            //	byte b2 = (byte)(projectile.timeLeft * 3);
            //	byte a2 = (byte)(100f * ((float)b2 / 255f));
            //	return new Color((int)b2, (int)b2, (int)b2, (int)a2);
            //}
            return new Color(255, 255, 255, 255);
        }

        public override void Kill(int timeLeft)
        {
            if (!projectile.active)
            {
                return;
            }
            projectile.tileCollide = false;
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

            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

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
            projectile.active = false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (Main.myPlayer == projectile.owner)
            {
                //Main.NewText("Projectile Size: " +  projectile.Size);

                if (!Released)
                {
                    projectile.scale += 0.03f;

                    projectile.position = player.position + new Vector2(0, -20 - (projectile.scale * 17));

                    for (int d = 0; d < 25; d++)
                    {
                        float angle = Main.rand.NextFloat(360);
                        float angleRad = MathHelper.ToRadians(angle);
                        Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                        Dust tDust = Dust.NewDustDirect(projectile.position + (position * (20 + 12.5f * projectile.scale)), projectile.width, projectile.height, 15, 0f, 0f, 213, default(Color), 2.0f);
                        tDust.velocity = Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                        tDust.noGravity = true;
                    }

                    MyPlayer.ModPlayer(player).KiCurrent -= 10;

                }

                //Rock effect
                projectile.ai[1]++;
                if (projectile.ai[1] % 7 == 0)
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
                Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);

                projectile.netUpdate = true;
            }
            if (MyPlayer.ModPlayer(player).KiCurrent <= 0)
            {
                projectile.Kill();
            }

            if (projectile.timeLeft < 10)
            {
                projectile.timeLeft = 200;
            }

            //if button let go
            if (!player.channel)
            {
                //projectile.Kill();
                if (!Released)
                {
                    Released = true;
                    projectile.velocity = Vector2.Normalize(Main.MouseWorld - projectile.position) * 6;
                    projectile.tileCollide = true;
                    projectile.damage *= (int)projectile.scale;

                }

                //Projectile p = Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.Center.Y) - (projectile.velocity * 5), new Vector2(0, 0), mod.ProjectileType("EnergyWaveTrail"), projectile.damage / 3, 4f, projectile.owner, 0, projectile.rotation);
                //p.scale = projectile.scale * 1.5f;
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.alpha <= 100);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
            //Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            //for (int k = 0; k < projectile.oldPos.Length; k++)
            //{
            //Vector2 drawPos = projectile.oldPos[0] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
            //Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
            //spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, Color.White, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			//}
			return true;	
		}   
    }
}