﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Projectiles.Auras;

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
            projectile.width = 113;
            projectile.height = 115;
            projectile.aiStyle = 0;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            BaseAuraTimer = 5;
            projectile.netUpdate = true;
            AuraOffset.Y = -30;
			projectile.light = 1f;
        }
        public override void PostAI()
        {
            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 1f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 113, 115, 63, 0f, 0f, 0, new Color(255, 255, 255), 0.75f);
                    dust.noGravity = true;
                }

            }
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            projectile.netUpdate = true;
            if (!modPlayer.IsCharging)
            {
                projectile.Kill();
            }
            else
            {
                projectile.rotation = 0;
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 10)
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
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            return base.PreDraw(spriteBatch, lightColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}