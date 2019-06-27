using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class AmethystNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Damage taken reduced by 9% and +4 defense.");
            DisplayName.SetDefault("Amethyst Necklace");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 36;
            item.value = 30000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.endurance += 0.09f;
                player.statDefense += 4;
                player.GetModPlayer<MyPlayer>(mod).amethystNecklace = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EmptyNecklace");
            recipe.AddIngredient(ItemID.Amethyst, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}