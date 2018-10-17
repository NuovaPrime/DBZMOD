﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class SSJ1AuraProjStart : ModProjectile
    {
        private float SizeTimer;
        private float BeamTimer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.width = 400;
            projectile.height = 400;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
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

            if (!MyPlayer.ModPlayer(player).IsTransformingSSJ1)
            {
                projectile.Kill();
            }
            if (SizeTimer < 300)
            {
                projectile.scale = SizeTimer / 300f * 2;
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
            if(projectile.active)
            {
                BeamTimer++;
                if(BeamTimer > 90)
                {
                    BeamTimer = 0;
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y - 1000, 0, -50, ProjectileID.VortexVortexLightning, 0, 0, player.whoAmI);

                }     
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            player.AddBuff(mod.BuffType("SSJ1Buff"), 3600);
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ1AuraProj"), 0, 0, player.whoAmI);
            MyPlayer.ModPlayer(player).IsTransformingSSJ1 = false;
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension"));
        }
    }
}
