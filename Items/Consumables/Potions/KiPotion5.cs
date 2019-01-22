using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.Potions
{
    public class KiPotion5 : KiPotion
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 38;
            item.consumable = true;
            item.maxStack = 30;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.value = 1200;
            item.rare = 4;
            item.potion = false;
            isKiPotion = true;
            kiHeal = 5100;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overflowing Ki Potion");
            Tooltip.SetDefault("Restores 5100 Ki.");
        }
		
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PureKiCrystal", 2);
            recipe.AddIngredient(null, "KiPotion4", 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}