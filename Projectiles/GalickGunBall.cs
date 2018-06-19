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
	public class GalickGunBall : KiProjectile
	{
        private Player player;
		public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
			projectile.tileCollide = false;
            projectile.width = 10;
            projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.light = 1f;
            projectile.timeLeft = 10;
            projectile.netUpdate = true;
            projectile.damage = 0;
			aiType = 14;
            projectile.ignoreWater = true;
			projectile.penetrate = -1;
            ChargeBall = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Galick Gun Ball");
        }
   
        public override void AI()
        {
            if(projectile.timeLeft < 4)
            {
                projectile.timeLeft = 10;
            }
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X - 20;
            projectile.position.Y = player.Center.Y - 3;

            if (!player.channel && ChargeLevel < 1)
            {
                projectile.Kill();
            }
            if (player.channel && projectile.active)
            {
                ChargeTimer++;
                KiDrainTimer++;
                player.velocity = new Vector2(player.velocity.X / 3, player.velocity.Y);
            }
            if(ChargeTimer > 120)
            {
                ChargeLevel += 1;
                ChargeTimer = 0;
                for (int d = 0; d < 70; d++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 15, 0f, 0f, 62, default(Color), 1.5f);
                }
            }
            if(!player.channel && ChargeLevel >= 1)
            {
                float rot = (float)Math.Atan2((Main.mouseY + Main.screenPosition.Y) - projectile.Center.Y, (Main.mouseX + Main.screenPosition.X) - projectile.Center.X);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)((Math.Cos(rot) * 10)), (float)((Math.Sin(rot) * 10)), mod.ProjectileType("GalickGunBlast"), projectile.damage + (ChargeLevel * 60), projectile.knockBack, projectile.owner, (projectile.scale = projectile.scale * ChargeLevel));
                ChargeLevel = 0;

            }

            if (KiDrainTimer > 1 && MyPlayer.ModPlayer(player).KiCurrent >= 0)
            {
                MyPlayer.ModPlayer(player).KiCurrent -= 1;
                KiDrainTimer = 0;
            }
        }

	}
}