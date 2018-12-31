using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using DBZMOD.Util;

namespace DBZMOD.Projectiles
{
    public class LSSJAuraBall : ModProjectile
    {
        private float SizeTimer;
        private float BlastTimer;
        private float yoffset;
        private float xoffset;

        public override void SetDefaults()
        {
            projectile.width = 120;
            projectile.height = 120;
            projectile.aiStyle = 0;
            projectile.timeLeft = 200;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            SizeTimer = 200f;
            yoffset = -200f;
            xoffset = 0f;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                projectile.position.X = player.Center.X;
                projectile.position.Y = player.Center.Y;
                projectile.Center = player.Center + new Vector2(xoffset, yoffset);
                projectile.netUpdate = true;

                if (!MyPlayer.ModPlayer(player).IsTransforming && !MyPlayer.ModPlayer(player).IsTransforming)
                {
                    projectile.Kill();
                }

                if (SizeTimer <= 200)
                {
                    projectile.scale = SizeTimer / 50f * 4;
                    SizeTimer--;
                    yoffset++;
                }
                if (SizeTimer <= 40)
                {
                    xoffset--;
                }
                if (projectile.active && !MyPlayer.ModPlayer(player).IsTransforming)
                {
                    BlastTimer++;
                    if (BlastTimer > 1)
                    {
                        Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 30;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("LSSJAuraLaser"), 0, 0, player.whoAmI);
                        BlastTimer = 0;
                    }

                }
                else if (projectile.active && MyPlayer.ModPlayer(player).IsTransforming)
                {
                    BlastTimer++;
                    if (BlastTimer > 1)
                    {
                        Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 40;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("LSSJAuraLaserBlack"), 0, 0, player.whoAmI);
                    }
                    if (BlastTimer > 1)
                    {
                        Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 40;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("LSSJBarrageProj"), 0, 0, player.whoAmI);
                        BlastTimer = 0;
                    }

                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.ModPlayer(player).IsTransforming)
            {
                Transformations.DoTransform(player, Transformations.LSSJ2, DBZMOD.instance, false);
                MyPlayer.ModPlayer(player).IsTransforming = false;
            }
            else
            {
                Transformations.DoTransform(player, Transformations.LSSJ, DBZMOD.instance, false);
                MyPlayer.ModPlayer(player).IsTransforming = false;
            }
        }
    }
}
