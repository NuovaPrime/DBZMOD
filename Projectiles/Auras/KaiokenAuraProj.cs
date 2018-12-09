﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles.Auras;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace DBZMOD.Projectiles.Auras
{
    public class KaiokenAuraProj : AuraProjectile
    {
        public float KaioAuraTimer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.width = 97;
            projectile.height = 102;
            projectile.aiStyle = 0;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            KaioAuraTimer = 240;
            IsKaioAura = true;
            AuraOffset.Y = -24;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.netUpdate = true;
            if (!player.HasBuff(Transformations.Kaioken.BuffId) && !player.HasBuff(Transformations.SSJ1Kaioken.BuffId))
            {
                projectile.Kill();
            }
            if (KaioAuraTimer > 0)
            {
                //projectile.scale = 1f + 2f * (KaioAuraTimer / 240f);
                KaioAuraTimer--;
            }
            if(player.HasBuff(Transformations.SSJ1Kaioken.BuffId))
            {
                ScaleExtra = 0.5f;
                AuraOffset.Y = -40;
            }
            else
            {
                AuraOffset.Y = -20;
            }
            base.AI();
        }
    }
}