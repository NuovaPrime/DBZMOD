using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class KiLanternItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ki Diffuser");
            Tooltip.SetDefault("It radiates with the glow of ki energy."
        + "\nGives slight ki regen when placed.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 30;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 3;
            item.value = 4000;
            item.createTile = mod.TileType("KiLantern");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("DBZMOD:KiFragment");
            recipe.AddIngredient(null, "ScrapMetal", 10);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}