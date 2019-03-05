using DBZMOD.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles.AuraProjectiles
{
    public class SSJ3LightPillar : ModProjectile
    {
        private float _sizeTimer;
        private float _blastTimer;
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
            _sizeTimer = 0f;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.Center = player.Center + new Vector2(-50, -300);
            projectile.netUpdate = true;

            if (!MyPlayer.ModPlayer(player).isTransforming)
            {
                projectile.Kill();
            }
            if (_sizeTimer < 300)
            {
                projectile.scale = _sizeTimer / 300f * 4;
                _sizeTimer++;
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
            MyPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();

            modPlayer.DoTransform(DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition);
            modPlayer.isTransforming = false;
        }
    }
}
