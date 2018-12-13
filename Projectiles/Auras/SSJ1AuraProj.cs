﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Util;
using Projectiles.Auras;

namespace DBZMOD.Projectiles.Auras
{
    public class SSJ1AuraProj : AuraProjectile
    {
        public int BaseAuraTimer;
        private int ChargeSoundTimer;

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
            AuraOffset.Y = -30;
            IsSSJAura = true;
			projectile.light = 1f;            
        }

        public override void AI()
        {
            DebugUtil.Log("Zoom is at " + Main.zoomX + ", " + Main.zoomY);
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(Transformations.SSJ1.GetBuffId()) && !player.HasBuff(Transformations.SSJ1Kaioken.GetBuffId()) && !player.HasBuff(Transformations.ASSJ.GetBuffId()) && !player.HasBuff(Transformations.USSJ.GetBuffId()))
            {
                // Main.NewText(string.Format("Player is missing a buff! Aura proj died for {0}", player.whoAmI));
                projectile.Kill();
            }
            if (player.HasBuff(Transformations.SSJ1.GetBuffId()))
            {
                if (BaseAuraTimer > 0)
                {
                    //projectile.scale = 1f - 0.7f * (BaseAuraTimer / 5f);
                    BaseAuraTimer--;
                }
            }
            else if (player.HasBuff(Transformations.ASSJ.GetBuffId()))
            {
                if (BaseAuraTimer > 0)
                {
                    //projectile.scale = 2f - 0.7f * (BaseAuraTimer / 5f);
                    BaseAuraTimer--;
                }
            }
            else if (player.HasBuff(Transformations.USSJ.GetBuffId()))
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
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
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
            spriteBatch.Begin();
        }
    }
}