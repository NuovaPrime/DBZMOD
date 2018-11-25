﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles.Auras
{
    public class FalseUIAuraProj : AuraProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 15;
        }
        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 72;
            projectile.aiStyle = 0;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            projectile.alpha = 250;
			ScaleExtra = 0.15f;
			AuraOffset.X = -1;
			AuraOffset.Y = -10;
            FrameAmount = 15;

        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
		
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);                                                                                                        //draw code here
            spriteBatch.End();
            spriteBatch.Begin();
		
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.netUpdate = true;
            if (!player.HasBuff(mod.BuffType("UIOmenBuff")))
            {
                projectile.Kill();
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 15)
            {
                projectile.frame = 0;
            }
            //blue dust
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                Vector2 position = projectile.Center + new Vector2(-15, -20);
                dust = Terraria.Dust.NewDustDirect(position, 42, 58, 187, 0f, -5.526316f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.5131579f;
            }
            //white dust
            if (Main.rand.NextFloat() < 0.5263158f)
            {
                Dust dust;
                Vector2 position = projectile.Center + new Vector2(-17, -10);
                dust = Terraria.Dust.NewDustDirect(position, 26, 52, 63, 0f, -7.368421f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.7894737f;
            }
            base.AI();
        }
    }
}