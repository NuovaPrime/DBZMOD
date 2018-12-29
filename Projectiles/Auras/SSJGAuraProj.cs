using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace DBZMOD.Projectiles.Auras
{
    public class SSJGAuraProj : AuraProjectile
    {
        private int ChargeSoundTimer = 240;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
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
            projectile.netUpdate = true;
            ScaledAuraOffset.Y = -30;
			projectile.light = 1f;
            HasComplexBlendStates = true;
        }

		public override void PostAI()
        {
            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 0.5f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 113, 115, 63, 0f, 0f, 0, new Color(150, 255, Main.DiscoColor.B), 0.5f);
                    dust.noGravity = true;
                }
				if (Main.rand.NextFloat() < 0.25f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 113, 115, 63, 0f, 0f, 0, new Color(Main.DiscoColor.R, 150, 150), 0.75f);
                    dust.noGravity = true;
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            projectile.netUpdate = true;
            if (!Transformations.IsGodlike(player))
            {
                projectile.Kill();
            }
			bool shouldPlayAudio = SoundUtil.ShouldPlayPlayerAudio(player, true);
            if (shouldPlayAudio)
            {
                ChargeSoundTimer++;
                if (ChargeSoundTimer > 340)
                {
                    player.GetModPlayer<MyPlayer>().TransformationSoundInfo = SoundUtil.PlayCustomSound("Sounds/SSG", player, 0.7f, 0.1f);
                    ChargeSoundTimer = 0;
                }
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