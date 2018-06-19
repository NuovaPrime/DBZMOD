using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class SapphireRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% Increased ki damage and +100 max ki.");
            DisplayName.SetDefault("Sapphire Ring");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = 30000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.09f;
                player.GetModPlayer<MyPlayer>(mod).KiMax += 100;
                player.GetModPlayer<MyPlayer>(mod).sapphireRing = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EmptyRing");
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}