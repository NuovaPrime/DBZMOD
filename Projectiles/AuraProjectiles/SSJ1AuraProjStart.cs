using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles.AuraProjectiles
{
    public class SSJ1AuraProjStart : ModProjectile
    {
        private float _sizeTimer;
        private float _beamTimer;
        public override void SetDefaults()
        {
            projectile.width = 176;
            projectile.height = 177;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            _sizeTimer = 0f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(0, -25);
            projectile.netUpdate = true;

            if (!MyPlayer.ModPlayer(player).isTransforming)
            {
                projectile.Kill();
            }
            if (_sizeTimer < 300)
            {
                projectile.scale = _sizeTimer / 300f * 2;
                _sizeTimer++;
            }
            else
            {
                projectile.scale = 1f;
            }
            projectile.frameCounter++; 
            if(projectile.active)
            {
                _beamTimer++;
                if(_beamTimer > 90)
                {
                    _beamTimer = 0;
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y - 1000, 0, -50, ProjectileID.VortexVortexLightning, 0, 0, player.whoAmI);

                }     
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            Transformations.DoTransform(player, Transformations.SSJ1, DBZMOD.instance);
            MyPlayer.ModPlayer(player).isTransforming = false;
        }
    }
}
