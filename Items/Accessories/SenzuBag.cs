using DBZMOD.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class SenzuBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reduces cooldown on senzu bean consumption");
            DisplayName.SetDefault("Senzu Bean Bag");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 8000;
            item.rare = 3;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).senzuBag = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SenzuBean", 3);
            recipe.AddIngredient(ItemID.Leather, 15);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}