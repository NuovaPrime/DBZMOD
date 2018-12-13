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
    public class SSJGAuraProj : AuraProjectile
    {
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
            AuraOffset.Y = -38;
            IsSSJAura = true;
			projectile.light = 1f;
        }
		public override void PostAI()
        {
            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 1f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 113, 115, 187, 0f, 0f, 0, new Color(255, 255, 255), 0.75f);
                    dust.noGravity = true;
                }
            }
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(Transformations.SSJG.GetBuffId()))
            {
                projectile.Kill();
            }

            ChargeSoundTimer++;
            if (ChargeSoundTimer > 420 && player.whoAmI == Main.myPlayer)
            {
                if (!Main.dedServ)
                    player.GetModPlayer<MyPlayer>().transformationSound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSG").WithVolume(.7f).WithPitchVariance(.1f));
                ChargeSoundTimer = 0;
            }
            base.AI();
        }
    }
}