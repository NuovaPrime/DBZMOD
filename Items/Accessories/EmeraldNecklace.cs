using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class EmeraldNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% Increased ranged damage and crit chance.");
            DisplayName.SetDefault("Emerald Necklace");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.value = 30000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.rangedDamage += 0.09f;
                player.rangedCrit += 9;
                player.GetModPlayer<MyPlayer>(mod).emeraldRing = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EmptyNecklace");
            recipe.AddIngredient(ItemID.Emerald, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}