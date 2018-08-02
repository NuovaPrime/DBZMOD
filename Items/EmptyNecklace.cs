﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class EmptyNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It seems you can attach a gem to it.");
            DisplayName.SetDefault("Empty Necklace");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = 0;
            item.rare = 2;
        }
		
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ScrapMetal", 3);
            recipe.AddIngredient(ItemID.IronBar, 5);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}