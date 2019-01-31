using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Projectiles.LSSJs
{
    public class LegendaryBlast : KiProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.width = 44;
            projectile.height = 66;
            projectile.aiStyle = 1;
            projectile.light = 1f;
            projectile.timeLeft = 150;
            projectile.damage = 40;
            aiType = 14;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            projectile.netUpdate = true;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unstable Energy Blast");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
