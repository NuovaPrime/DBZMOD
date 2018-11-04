using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DBZMOD.Items
{
    public class RadiantFragmentBlockItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Fragment Block");
        }
        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.consumable = true;
            item.useTime = 10;
            item.useStyle = 1;
            item.createTile = mod.TileType("RadiantFragmentBlock");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 5);
            recipe.AddIngredient(null, "RadiantFragment");
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}