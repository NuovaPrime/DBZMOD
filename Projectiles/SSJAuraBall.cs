﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace DBZMOD.Projectiles
{
    public class SSJAuraBall : ModProjectile
    {
        private float SizeTimer;
        private float BlastTimer;
        public override void SetDefaults()
        {
            projectile.width = 176;
            projectile.height = 177;
            projectile.aiStyle = 0;
            projectile.timeLeft = 200;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            SizeTimer = 0f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                projectile.position.X = player.Center.X;
                projectile.position.Y = player.Center.Y;
                projectile.Center = player.Center + new Vector2(0, -25);
                projectile.netUpdate = true;

                if (!MyPlayer.ModPlayer(player).IsTransforming)
                {
                    projectile.Kill();
                }

                if (SizeTimer < 200)
                {
                    projectile.scale = SizeTimer / 50f * 4;
                    SizeTimer++;
                }

            }
            else if(!MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                projectile.position.X = player.Center.X;
                projectile.position.Y = player.Center.Y;
                projectile.Center = player.Center + new Vector2(0, -25);
                projectile.netUpdate = true;

                if (!MyPlayer.ModPlayer(player).IsTransforming)
                {
                    projectile.Kill();
                }
                if (SizeTimer < 300)
                {
                    projectile.scale = SizeTimer / 300f * 4;
                    SizeTimer++;
                }
                else
                {
                    projectile.scale = 1f;
                }
                projectile.frameCounter++;
                if (projectile.frameCounter > 8)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
                if (projectile.active)
                {
                    BlastTimer++;
                    if (BlastTimer > 1)
                    {
                        Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 30;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("SSJEnergyBarrageProj"), 0, 0, player.whoAmI);
                        BlastTimer = 0;
                    }

                }
            }

        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (!MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                if (!player.HasBuff(Transformations.SSJ2.BuffId))
                    player.AddBuff(Transformations.SSJ2.BuffId, 360000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ2AuraProj"), 0, 0, player.whoAmI);
                MyPlayer.ModPlayer(player).IsTransforming = false;
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension"));
            }
            if (MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("LSSJAuraBall"), 0, 0, player.whoAmI);
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/GroundRumble").WithVolume(1f));
            }
        }
    }
}
