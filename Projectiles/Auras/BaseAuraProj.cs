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
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            projectile.width = 89;
            projectile.height = 110;
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
            AuraOffset.Y = -55;
			projectile.light = 1f;
			projectile.alpha = 70;
        }
        public override void PostAI()
        {
            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 1f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 52, 52, 63, 0f, 0f, 0, new Color(255, 255, 255), 0.7236842f);
                    dust.noGravity = true;
                }

            }
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.netUpdate = true;
            if (MyPlayer.EnergyCharge.JustReleased)
            {
                projectile.Kill();
            }
            else
            {
                projectile.rotation = 0;
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 3)
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
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);                                                                                 
            spriteBatch.End();
            spriteBatch.Begin();
			return true;
		}
    }
}