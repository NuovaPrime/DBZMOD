using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.Potions
{
    public class KiPotion3 : KiPotion
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.consumable = true;
            item.maxStack = 30;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.value = 600;
            item.rare = 3;
            item.potion = false;
            isKiPotion = true;
        }
        public override int GetKiHealAmount()
        {
            return 1080;
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Greater Ki Potion");
        }
		
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 2);
            recipe.AddIngredient(ItemID.UnicornHorn, 5);
            recipe.AddIngredient(ItemID.Waterleaf, 3);
            recipe.AddIngredient(ItemID.BottledWater, 3);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this, 3);
            recipe.AddRecipe();
        }
    }
}