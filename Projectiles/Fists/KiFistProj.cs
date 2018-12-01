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
    public class KiFistProj : KiProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.hide = true;
            projectile.width = 10;
            projectile.height = 42;
            projectile.alpha = 80;
            projectile.timeLeft = 2;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.penetrate = -1;
            projectile.netUpdate = true;
            projectile.aiStyle = 1;
        }
    }
}
