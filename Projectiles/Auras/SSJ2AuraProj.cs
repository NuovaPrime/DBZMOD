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
    public class SSJ2AuraProj : AuraProjectile
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
            if (!player.HasBuff(Transformations.SSJ2.GetBuffId()))
            {
                projectile.Kill();
            }

            ChargeSoundTimer++;
            if (ChargeSoundTimer > 480 && player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<MyPlayer>().TransformationSoundSlotId = SoundUtil.PlayCustomSound("Sounds/SSJ2", player, .7f, .1f);
                ChargeSoundTimer = 0;
            }
            base.AI();
        }
    }
}