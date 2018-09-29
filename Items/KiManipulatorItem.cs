﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class ZTable : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Z Table");
            Tooltip.SetDefault("Looks like it could bend the essence of ki itself.");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 2;
            item.value = 3000;
            item.createTile = mod.TileType("ZTable");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ScrapMetal", 15);
            recipe.AddIngredient(null, "StableKiCrystal", 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}