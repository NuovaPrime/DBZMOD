﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class SatanicCloth : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The cloth radiates with animosity.");
            DisplayName.SetDefault("Satanic Cloth");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = 0;
            item.rare = 8;
        }
		
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DemonicSoul", 3);
            recipe.AddIngredient(ItemID.Silk, 1);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}