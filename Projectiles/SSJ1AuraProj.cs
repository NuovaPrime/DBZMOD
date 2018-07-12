using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class SSJ1AuraProj : ModProjectile
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
            BaseAuraTimer = 5;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.netUpdate = true;
            projectile.Center = player.Center + new Vector2(0, -30);

            if (!player.HasBuff(mod.BuffType("SSJ1Buff")) || !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                projectile.Kill();
            }
            if (projectile.timeLeft < 2)
            {
                projectile.timeLeft = 10;
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
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
            ChargeSoundTimer++;
            if (ChargeSoundTimer > 22 && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyCharge").WithVolume(.7f).WithPitchVariance(.2f));
                ChargeSoundTimer = 0;
            }
            else
            {
                projectile.scale = 1f;
            }
        }
    }
}