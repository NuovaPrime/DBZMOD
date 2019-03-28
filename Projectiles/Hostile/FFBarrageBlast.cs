using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using DBZMOD.Utilities;

namespace DBZMOD.Projectiles.Hostile
{
    public class FFBarrageBlast : ModProjectile
    {
        private int moveTimer = 0;
        private bool directionChosen = false;
        private bool speedChosen = false;

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 40;
            projectile.timeLeft = 5000;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.friendly = false;
            projectile.magic = true;
            projectile.hostile = true;
            projectile.aiStyle = 101;
            projectile.light = 1f;
            projectile.stepSpeed = 13;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.netUpdate = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frieza Force Barrage Blast");
        }

        public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, 110);
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				Dust dust = Main.dust[Terraria.Dust.NewDust(projectile.position, 26, 26, 222, projectile.velocity.X, projectile.velocity.Y, 0, new Color(255,255,255), 1f)];
				dust.noGravity = true;
			}
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BigBangAttackProjectile2"), 1, 30, projectile.owner, 0f, 1f);

            ExplodeEffect();
        }
		public override void AI()
		{
            moveTimer++;

            if(moveTimer > 30 && !directionChosen)
            {
                ChooseDirection();
                projectile.velocity.Y = 6f;
            }

            if(directionChosen && !speedChosen)
            {
                if(projectile.direction == 1)
                {
                    projectile.velocity.X = Main.rand.NextFloat(0.3f, 18f);
                }
                else
                {
                    projectile.velocity.X = Main.rand.NextFloat(-0.3f, -18f);
                }
                speedChosen = true;
            }
			
		}

        private void ChooseDirection()
        {
            if(Main.rand.Next(2) == 0)
            {
                projectile.direction = 1;
                
            }
            else
            {
                projectile.direction = 0;
            }
            directionChosen = true;
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

            SoundHelper.PlayCustomSound("Sounds/Kiplosion", projectile.position, 1.0f);

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