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
	public class SpiritBombExplosion : KiProjectile
	{
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SwordBeam);
            projectile.hostile = false;
            projectile.friendly = true;
			projectile.tileCollide = false;
            projectile.width = 28;
            projectile.height = 28;
			projectile.aiStyle = 1;
			projectile.light = 1f;
			projectile.timeLeft = 10;
			projectile.damage = 0;
			aiType = 14;
            projectile.ignoreWater = true;
			projectile.penetrate = -1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            projectile.netUpdate = true;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SpiritBombExplosion");
        }
		
		public override void Kill(int timeLeft)
        {
            if (!projectile.active)
            {
                return;
            }

            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.knockBack = 8f;
            projectile.Damage();

            projectile.active = false;
			}
		
        public override void AI()
        {
            projectile.netUpdate = true;
        }
	}
}