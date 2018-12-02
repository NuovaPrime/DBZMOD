using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;

namespace DBZMOD.Projectiles.Fists
{
    public class KiFistProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Player player = Main.player[projectile.owner];
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.hide = false;
            // projectile is wider than it looks
            projectile.width = 10;
            projectile.height = 42;
            projectile.alpha = 80;
            projectile.timeLeft = 4;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.penetrate = 2;
            projectile.netUpdate = true;
            projectile.aiStyle = 0;
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void AI()
        {
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;

            projectile.rotation = Vector2.Normalize(projectile.velocity).ToRotation() + (float)(Math.PI / (Main.rand.NextBool() ? -2 : 2));

            projectile.alpha = Main.rand.Next(80, 160);
            projectile.scale = 0.8f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive); //draw code here
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 1;
        }
    }
}
