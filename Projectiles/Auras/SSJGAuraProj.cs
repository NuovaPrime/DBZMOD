using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles.Auras
{
    public class SSJGAuraProj : AuraProjectile
    {
        public int BaseAuraTimer;
        private int ChargeSoundTimer = 240;
        private int LightningTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
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
            projectile.alpha = 10;
            BaseAuraTimer = 5;
            AuraOffset.Y = -65;
            ScaleExtra = 0.5f;
            FrameAmount = 6;
            IsSSJAura = true;
        }
        public override void AI()
        {
            if (Main.rand.NextFloat() < 0.7236842f)
            {
                Dust dust;
                Vector2 position = projectile.Center + new Vector2(-30, 0);
                dust = Main.dust[Dust.NewDust(position, 84, 105, 187, 0f, -3.421053f, 213, new Color(255, 0, 0), 1.1f)];
                dust.noGravity = true;
                dust.noLight = true;
            }

            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(mod.BuffType("SSJGBuff")))
            {
                projectile.Kill();
            }
            if (BaseAuraTimer > 0)
            {
                projectile.scale = 1.5f - 0.7f * (BaseAuraTimer / 5f);
                BaseAuraTimer--;
            }
            ChargeSoundTimer++;
            if (ChargeSoundTimer > 420 && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSG").WithVolume(.7f).WithPitchVariance(.1f));
                ChargeSoundTimer = 0;
            }
            else
            {
                projectile.scale = 1.5f;
            }
            if (MyPlayer.ModPlayer(player).IsCharging)
            {
                projectile.scale *= 1.5f;
            }
            base.AI();
        }
    }
}