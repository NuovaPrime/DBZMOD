using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class SuperEnergyBarrageExplosion : ModProjectile
    {
        private float _sizeTimer;
        public override void SetDefaults()
        {
            projectile.width = 120;
            projectile.height = 120;
            projectile.aiStyle = 0;
            projectile.alpha = 70;
            projectile.timeLeft = 120;
			projectile.light = 1f;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 60;
            _sizeTimer = 120;
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
        }
		
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (_sizeTimer > 0)
            {
                projectile.scale = (_sizeTimer / 120f) * 2;
                _sizeTimer--;
            }
            else
            {
                projectile.scale = 0.5f;
            }
        }
    }
}