﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class DivineThreads : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A unbelievably soft material that radiates a divine-like energy.'");
            DisplayName.SetDefault("Divine Threads");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = 0;
            item.rare = 8;
            item.maxStack = 9999;
        }
		
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PureKiCrystal", 2);
            recipe.AddIngredient(ItemID.Ectoplasm, 1);
            recipe.AddIngredient(ItemID.Silk, 1);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}