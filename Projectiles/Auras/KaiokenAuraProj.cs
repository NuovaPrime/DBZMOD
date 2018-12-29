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
            projectile.hide = true;
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            // Add this projectile to the list of projectiles that will be drawn BEFORE tiles and NPC are drawn. This makes the projectile appear to be BEHIND the tiles and NPC.
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            // kill the projectile if Kaioken is level 0
            if (modPlayer.KaiokenLevel == 0)
                projectile.Kill();

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