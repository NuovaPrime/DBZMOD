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
            projectile.width = 10;
            projectile.height = 42;
            projectile.alpha = 80;
            projectile.timeLeft = 10;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.penetrate = 1;
            projectile.netUpdate = true;
            projectile.aiStyle = 1;
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
            projectile.scale = 1.0f;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 1;
        }
    }
}
