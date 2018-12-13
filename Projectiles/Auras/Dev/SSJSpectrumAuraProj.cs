using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Util;
using Projectiles.Auras;

namespace DBZMOD.Projectiles.Auras.Dev
{
    public class SSJSpectrumAuraProj : AuraProjectile
    {
        public int BaseAuraTimer;
        private int ChargeSoundTimer = 240;
        private int LightningTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
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
            BaseAuraTimer = 5;
            AuraOffset.Y = -40;
            AuraOffset.X = -10;
            IsSSJAura = true;
			projectile.light = 1f;
        }
        public override void AI()
        {
            if (Main.rand.NextFloat() < 0.2236842f)
            {
                Dust dust;
                Vector2 position = projectile.Center + new Vector2(-40, -5);
                dust = Main.dust[Dust.NewDust(position, 84, 105, 261, 0f, -3.421053f, 213, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B), 1.4f)];
                dust.noGravity = true;
                dust.noLight = true;
            }

            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(Transformations.SPECTRUM.GetBuffId()))
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
                if (!Main.dedServ)
                    player.GetModPlayer<MyPlayer>().transformationSound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSG").WithVolume(.7f).WithPitchVariance(.1f));
                ChargeSoundTimer = 0;
            }
            else
            {
                projectile.scale = 1.5f;
            }
            if (MyPlayer.ModPlayer(player).IsCharging)
            {
                projectile.scale *= 1.2f;
            }
            base.AI();
        }
    }
}