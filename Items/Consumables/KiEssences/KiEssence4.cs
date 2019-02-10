using DBZMOD;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.KiEssences
{
    public class KiEssence4 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 48;
            item.height = 48;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 3;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enraged Ki Scroll");
            Tooltip.SetDefault("Increases your ki charge rate.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).kiChargeRate += 2;
            MyPlayer.ModPlayer(player).kiEssence4 = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).kiEssence4)
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
            recipe.AddIngredient(null, "PureKiCrystal", 30);
            recipe.AddIngredient(null, "AngerKiCrystal", 20);
            recipe.AddIngredient(ItemID.LihzahrdPowerCell);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
