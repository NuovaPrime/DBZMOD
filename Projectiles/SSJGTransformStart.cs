﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class SSJGTransformStart : ModProjectile
    {
        private float SizeTimer;
        private float BlastTimer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            projectile.width = 120;
            projectile.height = 120;
            projectile.aiStyle = 0;
            projectile.timeLeft = 24;
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
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(0, -25);
            projectile.netUpdate = true;

            if (!MyPlayer.ModPlayer(player).IsTransformingSSJG)
            {
                projectile.Kill();
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 7)
            {
                projectile.frame = 0;
            }

        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            player.AddBuff(mod.BuffType("SSJGBuff"), 3600);
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJGAuraProj"), 0, 0, player.whoAmI);
            MyPlayer.ModPlayer(player).IsTransformingSSJG = false;
            MyPlayer.ModPlayer(player).IsTransformed = true;
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension"));
        }
    }
}
