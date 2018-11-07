using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Projectiles
{
    public class RadialJavelinProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radial Javelin");
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 82;
            projectile.timeLeft = 280;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.aiStyle = 102;
            projectile.light = 3f;
            projectile.stepSpeed = 17f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
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
                    Dust dust = Main.dust[Terraria.Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("RadiantDust"), 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                }
            }
        }
        //For all of the NPC slots in Main.npc
        //Note, you can replace NPC with other entities such as Projectiles and Players
        public override void AI()
        {
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                    //Get the shoot trajectory from the projectile and target
                float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                float shootToY = target.position.Y - projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    //If the distance between the live targeted npc and the projectile is less than 480 pixels
                if (distance < 480f && !target.friendly && target.active)
                {
                        //Divide the factor, 3f, which is the desired velocity
                    distance = 3f / distance;

                        //Multiply the distance by a multiplier if you wish the projectile to have go faster
                    shootToX *= distance * 5;
                    shootToY *= distance * 5;

                        //Set the velocities to the shoot values
                    projectile.velocity.X = shootToX;
                    projectile.velocity.Y = shootToY;
                }
            }
        }
    }
}