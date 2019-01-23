using Terraria;
using Terraria.ID;

namespace DBZMOD.Projectiles.AuraProjectiles
{
    public class SSJ1AuraProjBeamHead : KiProjectile
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
            projectile.timeLeft = 150;
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
            DisplayName.SetDefault("SSJ Aura Beam");
        }  

        public override void AI()
        {

            projectile.ai[1]++;
            if (projectile.ai[1] == 2)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 0, 0, 0, mod.ProjectileType("SSJ1AuraProjBeam"), 0, 0f, projectile.owner, 0, projectile.rotation);
                projectile.ai[1] = 0;
                projectile.netUpdate = true;
            }
        }
    }
}