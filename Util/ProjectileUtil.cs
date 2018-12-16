using DBZMOD;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace Util
{
    public static class ProjectileUtil
    {
        public static void ApplyChannelingSlowdown(Player player)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            if (modPlayer.IsFlying)
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                float yVelocity = 0f;
                if (modPlayer.IsDownHeld || modPlayer.IsUpHeld)
                {
                    yVelocity = player.velocity.Y / (1.2f - chargeMoveSpeedBonus);
                }
                else
                {
                    yVelocity = Math.Min(-0.4f, player.velocity.Y / (1.2f - chargeMoveSpeedBonus));
                }
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), yVelocity);
            }
            else
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                // don't neuter falling - keep the positive Y velocity if it's greater - if the player is jumping, this reduces their height. if falling, falling is always greater.                        
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), Math.Max(player.velocity.Y, player.velocity.Y / (1.2f - chargeMoveSpeedBonus)));
            }
        }

        // find the closest projectile to a player (owned by that player) of a given type, used to "recapture" charge balls, letting the player resume charging them whenever they want.
        public static Projectile FindNearestOwnedProjectileOfType(Player player, int type)
        {
            int closestProjectile = -1;
            float distance = float.MaxValue;
            for(var i = 0; i < Main.projectile.Length; i++)
            {
                var proj = Main.projectile[i];

                // abort if the projectile is invalid, the player isn't the owner or the type doesn't match what we want.
                if (proj == null || proj.owner != player.whoAmI || proj.type != type)
                    continue;               
                
                var projDistance = proj.Distance(player.Center);
                if (projDistance < distance)
                {
                    distance = projDistance;
                    closestProjectile = i;
                }                
            }
            return closestProjectile == -1 ? null : Main.projectile[closestProjectile];
        }
    }
}
