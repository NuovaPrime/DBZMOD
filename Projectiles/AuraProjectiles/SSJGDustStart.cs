using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles.AuraProjectiles
{
    public class SSJGDustStart : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
        }

        public override void AI()
        {
            projectile.ai[1] += 3;
            if(projectile.ai[1] % 1 == 0)
            {
	            Dust dust;
                Vector2 position = projectile.Center + new Vector2(-600, 0);
	            dust = Main.dust[Terraria.Dust.NewDust(position, 1300, 236, 200, 0f, -20f, 0, new Color(255,255,255), 1.447368f)];
	            dust.fadeIn = 0.9473684f;

                Dust dust2;
                dust2 = Main.dust[Terraria.Dust.NewDust(position, 1300, 236, 182, 0f, -20f, 0, new Color(255, 255, 255), 1.447368f)];
                dust2.fadeIn = 0.9473684f;

                Dust dust3;
                dust3 = Main.dust[Terraria.Dust.NewDust(position, 1300, 236, 115, 0f, -20f, 0, new Color(255, 255, 255), 1.447368f)];
                dust3.fadeIn = 0.9473684f;
            }

            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(0, -25);
            projectile.netUpdate = true;

            if(projectile.timeLeft == 300)
            {
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJGDustCenter"), 0, 0, player.whoAmI);
            }
            if (!MyPlayer.ModPlayer(player).isTransforming)
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            Transformations.DoTransform(player, Transformations.SSJG, DBZMOD.instance);
            MyPlayer.ModPlayer(player).isTransforming = false;
            SoundHelper.PlayCustomSound("Sounds/SSJAscension");
        }
    }
}
