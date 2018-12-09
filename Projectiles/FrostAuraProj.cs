using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
    public class FrostAuraProj : KiProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Aura");
        }	
		public override void SetDefaults()
		{
            projectile.CloneDefaults(ProjectileID.SwordBeam);
            projectile.width = 560;
			projectile.height = 560;
			projectile.aiStyle = 1;
		    projectile.damage = 0;
		    aiType = 14;
			projectile.timeLeft = 2;
			projectile.friendly = true;
		    projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;    
			projectile.penetrate = -1;
		    projectile.alpha = 255;
		}

	    public override void AI()
	    {
	        //projectile.Center = player.Center + new Vector2(0, 0);
	    }
        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
             npc.AddBuff(BuffID.Frostburn, 180);
        }
    }
}