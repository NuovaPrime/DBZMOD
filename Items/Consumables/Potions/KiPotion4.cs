using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.Potions
{
    public class KiPotion4 : KiPotion
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 34;
            item.consumable = true;
            item.maxStack = 30;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.value = 800;
            item.rare = 3;
            item.potion = false;
            isKiPotion = true;
            kiHeal = 2180;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Ki Potion");
            Tooltip.SetDefault("Restores 2180 Ki.");
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AngerKiCrystal", 3);
            recipe.AddIngredient(null, "KiPotion3", 4);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}