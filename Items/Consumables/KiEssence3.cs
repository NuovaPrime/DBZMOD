using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class KiEssence3 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 42;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 4;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adept Ki Scroll");
            Tooltip.SetDefault("Increases your ki charge rate.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).kiChargeRate += 2;
            MyPlayer.ModPlayer(player).kiEssence3 = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).kiEssence3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AngerKiCrystal", 20);
            recipe.AddIngredient(null, "PridefulKiCrystal", 30);
            recipe.AddIngredient(ItemID.SpiderFang, 5);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
