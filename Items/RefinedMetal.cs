﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class RefinedMetal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Refined Metal");
            Tooltip.SetDefault("'A high quality refined piece of metal, it is incredibly durable.'");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 9999;
            item.value = 1000;
            item.rare = 4;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ScrapMetal", 1);
            recipe.AddIngredient(ItemID.CobaltBar, 1);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(null, "ScrapMetal", 1);
            recipe2.AddIngredient(ItemID.PalladiumBar, 1);
            recipe2.AddTile(TileID.Hellforge);
            recipe2.SetResult(this, 2);
            recipe2.AddRecipe();
        }
    }
}