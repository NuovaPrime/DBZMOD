﻿using System;
using Microsoft.Xna.Framework;
 using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class SSJGDustCenter : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            const float aurawidth = 3.0f;
            Lighting.AddLight(player.Center, 1f, 0.3f, 0f);

            for (int i = 0; i < 60; i++)
            {
                float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * aurawidth))).X;
                float yPos = ((Vector2.UnitY * player.height) - (Vector2.UnitY * Main.rand.Next(0, player.height))).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(player.position + new Vector2(xPos, yPos), 1, 1, 200, 0f, -2f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

                if ((Math.Abs((tDust.position - (player.position + (Vector2.UnitX * 7.0f))).X)) < 10)
                {
                    tDust.scale *= 0.75f;
                }

                tDust.velocity.Y++;

                Vector2 dir = -(tDust.position - ((player.position + (Vector2.UnitX * 5.0f)) - (Vector2.UnitY * player.height)));
                dir.Normalize();

                tDust.velocity = new Vector2(dir.X * 2.0f, -1 * Main.rand.Next(1, 5));
                tDust.noGravity = true;
            }


            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(0, -25);
            projectile.netUpdate = true;

            if (!MyPlayer.ModPlayer(player).isTransforming)
            {
                projectile.Kill();
            }
        }
    }
}
