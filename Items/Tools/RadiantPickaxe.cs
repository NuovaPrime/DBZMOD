using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DBZMOD.Items.Tools
{
    public class RadiantPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Pickaxe");
        }

        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useStyle = 1;
            item.width = 42;
            item.height = 40;
            item.rare = 10;
            item.useTime = 11;
            item.useAnimation = 11;
            item.damage = 80;
            item.melee = true;
            item.value = 50000;
            item.pick = 225;
            item.knockBack = 5.5f;
            item.tileBoost = 4;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(null, "RadiantFragment", 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
