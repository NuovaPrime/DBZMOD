using DBZMOD.Extensions;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

namespace DBZMOD.Projectiles.LSSJs
{
    public class LSSJAuraBall : ModProjectile
    {
        private float _sizeTimer;
        private float _blastTimer;
        private float _yoffset;
        private float _xoffset;

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
            _sizeTimer = 200f;
            _yoffset = -200f;
            _xoffset = 0f;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                projectile.position.X = player.Center.X;
                projectile.position.Y = player.Center.Y;
                projectile.Center = player.Center + new Vector2(_xoffset, _yoffset);
                projectile.netUpdate = true;

                if (!MyPlayer.ModPlayer(player).isTransforming && !MyPlayer.ModPlayer(player).isTransforming)
                {
                    projectile.Kill();
                }

                if (_sizeTimer <= 200)
                {
                    projectile.scale = _sizeTimer / 50f * 4;
                    _sizeTimer--;
                    _yoffset++;
                }
                if (_sizeTimer <= 40)
                {
                    _xoffset--;
                }
                if (projectile.active && !MyPlayer.ModPlayer(player).isTransforming)
                {
                    _blastTimer++;
                    if (_blastTimer > 1)
                    {
                        Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 30;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("LSSJAuraLaser"), 0, 0, player.whoAmI);
                        _blastTimer = 0;
                    }

                }
                else if (projectile.active && MyPlayer.ModPlayer(player).isTransforming)
                {
                    _blastTimer++;
                    if (_blastTimer > 1)
                    {
                        Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 40;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("LSSJAuraLaserBlack"), 0, 0, player.whoAmI);
                    }
                    if (_blastTimer > 1)
                    {
                        Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 40;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("LSSJBarrageProj"), 0, 0, player.whoAmI);
                        _blastTimer = 0;
                    }

                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.ModPlayer(player).isTransforming)
            {
                player.DoTransform(FormBuffHelper.lssj2, DBZMOD.instance);
                MyPlayer.ModPlayer(player).isTransforming = false;
            }
            else
            {
                player.DoTransform(FormBuffHelper.lssj, DBZMOD.instance);
                MyPlayer.ModPlayer(player).isTransforming = false;
            }
        }
    }
}
