using System;
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
    public class LSSJAuraProj : AuraProjectile
    {
        private int ChargeSoundTimer = 480;
        private int LightningTimer = 0;

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
            AuraOffset.Y = -30;
            IsSSJAura = true;
			projectile.light = 1f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(Transformations.LSSJ.GetBuffId()))
            {
                projectile.Kill();
            }
			
            ChargeSoundTimer++;
            if (ChargeSoundTimer > 480 && player.whoAmI == Main.myPlayer)
            {
                if (!Main.dedServ)
                    player.GetModPlayer<MyPlayer>().transformationSound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJ2").WithVolume(.7f).WithPitchVariance(.1f));
                ChargeSoundTimer = 0;
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