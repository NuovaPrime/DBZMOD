using System;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Projectiles.Support
{
    public class GuardianMendingProj : KiProjectile
	{
        Vector2 offset;
        float healingRange = 100f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guardian Mending");
        }	
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.aiStyle = 0;
            projectile.alpha = 120;
			projectile.timeLeft = 2;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;    
			projectile.penetrate = -1;
		}
        public override void AI()
        {

            if (player.channel)
            {
                projectile.timeLeft = 2;
                player.velocity = new Vector2(0, 0);
                MyPlayer.ModPlayer(player).AddKi(-1.667f, true, false);
                
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            return Vector2.Distance(target.Center, projectile.Center) <= healingRange;
        }
        int healAmount;
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            healAmount = (int)(10 * MyPlayer.ModPlayer(player).healMulti);
            target.statLife += healAmount;
            target.HealEffect(healAmount, true);
            float xDustCoord = target.Center.X + (target.width / 2f) - Main.rand.NextFloat(player.width); // random x offset starting at the left edge of the player texture all the way to the right
            float yDustCoord = target.Center.Y + (target.height / 2f); // player feet, the 0 line
            Dust.NewDustDirect(new Vector2(xDustCoord, yDustCoord), target.width, target.height, 163, 0, -10);
        }
    }
}