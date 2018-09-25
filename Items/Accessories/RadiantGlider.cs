using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DBZMOD.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class RadiantGlider : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Glider");
			Tooltip.SetDefault("Allows flight and slow fall.");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 40;
			item.value = 120000;
			item.rare = 10;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 160;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.95f;
            ascentWhenRising = 0.20f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 10f;
            acceleration *= 3f;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "RadiantFragment", 14);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}