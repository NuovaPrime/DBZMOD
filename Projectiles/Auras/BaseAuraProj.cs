﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles.Auras
{
    public class BaseAuraProj : AuraProjectile
    {
        public int BaseAuraTimer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.width = 97;
            projectile.height = 89;
            projectile.aiStyle = 0;
            projectile.alpha = 70;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            BaseAuraTimer = 5;
            projectile.netUpdate = true;
            AuraOffset = -25;
        }
        public override void AI()
        {
            projectile.netUpdate = true;
            if (MyPlayer.EnergyCharge.JustReleased)
            {
                projectile.Kill();
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 4)
            {
                projectile.frame = 0;
            }
            if (BaseAuraTimer > 0)
            {
                projectile.scale = 1f - 0.7f * (BaseAuraTimer / 5f);
                BaseAuraTimer--;
            }
            else
            {
                projectile.scale = 1f;
            }
            base.AI();
        }
    }
}