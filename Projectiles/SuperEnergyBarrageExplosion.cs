using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class SuperEnergyBarrageExplosion : ModProjectile
    {
        private float SizeTimer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
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
            SizeTimer = 120;
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
            projectile.frameCounter++;
            if (projectile.frameCounter > 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 4)
            {
                projectile.frame = 0;
            }
            if (SizeTimer > 0)
            {
                projectile.scale = (SizeTimer / 120f) * 2;
                SizeTimer--;
            }
            else
            {
                projectile.scale = 0.5f;
            }
        }
    }
}