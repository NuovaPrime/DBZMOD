using DBZMOD.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Projectiles.LSSJs
{
    public class LSSJ2PillarStart : KiProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SwordBeam);
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.width = 50;
            projectile.height = 50;
            projectile.aiStyle = 1;
            projectile.light = 1f;
            projectile.timeLeft = 180;
            projectile.alpha = 0;
            projectile.knockBack = 0f;
            projectile.damage = 0;
            aiType = 14;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Legendary Light Pillar");
        }  

        public override void AI()
        {

            projectile.ai[1]++;
            if (projectile.ai[1] == 12)
            {
                Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 800), projectile.Center.Y + 1000, 0, -16, mod.ProjectileType("FinalShineBlast"), 0, 0f, projectile.owner);
                projectile.ai[1] = 0;
                projectile.netUpdate = true;
            }

            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(0, -25);
            projectile.netUpdate = true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            SoundHelper.PlayCustomSound("Sounds/Awakening", player, 1.5f);
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("LSSJAuraBall"), 0, 0, player.whoAmI);
        }
    }
}