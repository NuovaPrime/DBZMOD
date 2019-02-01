using DBZMOD.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class BaldurShell : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 97;
            projectile.height = 102;
            projectile.aiStyle = 0;
            projectile.alpha = 120;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            projectile.netUpdate = true;
            projectile.scale = 1.1f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            projectile.netUpdate = true;
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(34, 30);
            if (!modPlayer.isCharging)
            {
                projectile.Kill();
            }
            else
            {
                projectile.timeLeft = 2;
                projectile.rotation = 0;
            }
            
            base.AI();
        }
    }
}