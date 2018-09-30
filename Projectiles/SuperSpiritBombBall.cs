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
    public class SuperSpiritBombBall : KiProjectile
    {
        public bool Released = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SuperSpiritBombBall");
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
            projectile.penetrate = 12;
            projectile.timeLeft = 400;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
            KiDrainRate = 10;
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

            Projectile proj = Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0,0), mod.ProjectileType("SuperSpiritBombExplosion"), projectile.damage, projectile.knockBack, projectile.owner);
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
                    projectile.scale += 0.04f;

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

                    if (projectile.timeLeft < 399)
                    {
                        projectile.timeLeft = 400;
                    }
                    if (MyPlayer.ModPlayer(player).KiCurrent <= 0)
                    {
                        projectile.Kill();
                    }

                    MyPlayer.ModPlayer(player).KiCurrent -= 2;
                    player.velocity = new Vector2(player.velocity.X / 3, player.velocity.Y);

                    //Rock effect
                    projectile.ai[1]++;
                    if (projectile.ai[1] % 7 == 0)
                        Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);
                }

                projectile.netUpdate = true;
            }

            //if button let go
            if (!player.channel)
            {
                //projectile.Kill();
                if (!Released)
                {
                    Released = true;
                    projectile.velocity = Vector2.Normalize(Main.MouseWorld - projectile.position) * 6;
                    projectile.tileCollide = false;
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