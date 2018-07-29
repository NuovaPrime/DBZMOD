using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class SSJ3LightPillar : ModProjectile
    {
        private float SizeTimer;
        private float BlastTimer;
        public override void SetDefaults()
        {
            projectile.width = 120;
            projectile.height = 120;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            SizeTimer = 0f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(-50, -300);
            projectile.netUpdate = true;

            if (!MyPlayer.ModPlayer(player).IsTransformingSSJ3)
            {
                projectile.Kill();
            }
            if (SizeTimer < 300)
            {
                projectile.scale = SizeTimer / 300f * 4;
                SizeTimer++;
            }
            else
            {
                projectile.scale = 1f;
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 8)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 4)
            {
                projectile.frame = 0;
            }   
            projectile.ai[1]++;
            if (projectile.ai[1] % 7 == 0)
            Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
            Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            player.AddBuff(mod.BuffType("SSJ3Buff"), 3600);
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ3AuraProj"), 0, 0, player.whoAmI);
            MyPlayer.ModPlayer(player).IsTransformingSSJ3 = false;
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension"));
        }
    }
}
