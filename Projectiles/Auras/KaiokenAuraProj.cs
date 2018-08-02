﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            AuraOffset = -25;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.netUpdate = true;
            if (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                projectile.Kill();
            }
            if (KaioAuraTimer > 0)
            {
                projectile.scale = 1f + 2f * (KaioAuraTimer / 240f);
                KaioAuraTimer--;
            }
            else
            {
                projectile.scale = 1.3f;
            }
            if (MyPlayer.ModPlayer(player).IsCharging)
            {
                projectile.scale *= 1.5f;
            }
            base.AI();
        }
    }
}