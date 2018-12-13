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
    public class KaiokenAuraProjx100 : AuraProjectile
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
            projectile.alpha = 70;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            KaioAuraTimer = 240;
            AuraOffset.Y = -250;
            IsKaioAura = true;
            ScaleExtra = 6.0f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.netUpdate = true;
            if (!player.HasBuff(Transformations.Kaioken100.GetBuffId()))
            {
                projectile.Kill();
            }
            if (KaioAuraTimer > 0)
            {
                //projectile.scale = 7f + 6f * (KaioAuraTimer / 240f);
                KaioAuraTimer--;
            }
            base.AI();
        }
    }
}