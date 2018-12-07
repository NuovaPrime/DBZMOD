﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace DBZMOD.Projectiles.Auras
{
    public class SSJ1AuraProj : AuraProjectile
    {
        public int BaseAuraTimer;
        private int ChargeSoundTimer;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            projectile.width = 95;
            projectile.height = 89;
            projectile.aiStyle = 0;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            projectile.alpha = 20;
            BaseAuraTimer = 5;
            AuraOffset.Y = -38;
			ScaleExtra = 0.1f;
            IsSSJAura = true;
			projectile.light = 1f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(Transformations.SSJ1.BuffId) && !player.HasBuff(Transformations.SSJ1Kaioken.BuffId))
            {
                projectile.Kill();
            }
            if (player.HasBuff(Transformations.SSJ1.BuffId))
            {
                if (BaseAuraTimer > 0)
                {
                    //projectile.scale = 1f - 0.7f * (BaseAuraTimer / 5f);
                    BaseAuraTimer--;
                }
            }
            else if (player.HasBuff(Transformations.ASSJ.BuffId))
            {
                if (BaseAuraTimer > 0)
                {
                    //projectile.scale = 2f - 0.7f * (BaseAuraTimer / 5f);
                    BaseAuraTimer--;
                }
            }
            else if (player.HasBuff(Transformations.USSJ.BuffId))
            {
                if (BaseAuraTimer > 0)
                {
                    //projectile.scale = 3f - 0.7f * (BaseAuraTimer / 5f);
                    BaseAuraTimer--;
                }
            }

            ChargeSoundTimer++;
            if (ChargeSoundTimer > 22 && player.whoAmI == Main.myPlayer)
            {
                if (!Main.dedServ)
                    player.GetModPlayer<MyPlayer>().transformationSound = player.GetModPlayer<MyPlayer>().transformationSound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyCharge").WithVolume(.7f).WithPitchVariance(.2f));
                ChargeSoundTimer = 0;
            }

            base.AI();
        }
    }
}