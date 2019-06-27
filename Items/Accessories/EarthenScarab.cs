using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class EarthenScarab : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The soul of the land seems to give it life.'\n4% Increased ki damage\nIncreased flight speed\nThe longer you charge the more ki you charge, limits at +500%.");
            DisplayName.SetDefault("Earthen Scarab");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 34;
            item.value = 360000;
            item.rare = 4;
            item.accessory = true;
            item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiDamage += 0.04f;
                player.GetModPlayer<MyPlayer>(mod).flightSpeedAdd += 0.1f;
                player.GetModPlayer<MyPlayer>(mod).earthenScarab = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 100);
            recipe.AddIngredient(null, "EarthenShard", 20);
			recipe.AddIngredient(null, "AstralEssentia", 10);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}