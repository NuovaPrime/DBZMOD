using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Util;

namespace DBZMOD.Projectiles.Auras
{
    public class KaiokenAuraProj : AuraProjectile
    {
        public float KaioAuraTimer;
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
            KaioAuraTimer = 240;
            IsKaioAura = true;   
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            // easy automatic aura offset.
            OriginalAuraOffset.Y = player.height * 0.66f - (projectile.height / 2) * projectile.scale;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            // kill the projectile if Kaioken is level 0
            if (modPlayer.KaiokenLevel == 0)
                projectile.Kill();

            // scale is based on kaioken level, which gets set to 0
            OriginalScale = 0.9f + (0.1f * modPlayer.KaiokenLevel);

            if (Transformations.IsAnythingOtherThanKaioken(player))
            {
                projectile.scale = OriginalScale * 1.2f;
            }
            else
            {
                projectile.scale = OriginalScale;
            }

            // correct scaling
            if (ScaledAuraOffset != OriginalAuraOffset)
                ScaledAuraOffset = OriginalAuraOffset;
            projectile.netUpdate = true;

            // remove the aura if the buff is removed.
            if (!Transformations.IsKaioken(player))
            {
                projectile.Kill();
            }

            // I don't know what this does.
            if (KaioAuraTimer > 0)
            {            
                KaioAuraTimer--;
            }
            base.AI();
        }
    }
}