﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class KiChip : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A piece of a ki fragment.'\nIncreased ki charge rate");
            DisplayName.SetDefault("Ki Chip");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 12500;
            item.rare = 3;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiChargeRate += 1;
                player.GetModPlayer<MyPlayer>(mod).kiChip = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StableKiCrystal", 12);
            recipe.AddIngredient(ItemID.GraniteBlock, 8);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}