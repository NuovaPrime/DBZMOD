using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Items.Weapons.Tier_6
{
	public class CandyLaser : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("CandyLaserBlast");
			item.shootSpeed = 14f;
			item.damage = 142;
			item.knockBack = 4f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
			item.useAnimation = 140;
			item.useTime = 140;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 100000;
			item.rare = 9;
            item.channel = false;
            kiDrain = 300;
			weaponType = "Unique";
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("Turns your foes to chocolate, does not work on bosses.");
		DisplayName.SetDefault("Candy Laser");
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 20f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("CandyLaserBlast")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
