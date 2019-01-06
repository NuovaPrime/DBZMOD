using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class TransmissionVanish : ModItem
    {

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
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
            DisplayName.SetDefault("I.T. Tome Vol 1 - Dissipati Peribunt");
            Tooltip.SetDefault("'It holds an alien power, bending space to seek beacons of ki.'");
        }

        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).IsInstantTransmission1Unlocked = true;
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You have unlocked Instant Transmission Lv1."
                + "\nClick an NPC, player or " +
                "\nKi Beacon on the map to use it.");
                return true;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).IsInstantTransmission1Unlocked)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public override void AddRecipes()
        //{
        //    ModRecipe recipe = new ModRecipe(mod);
        //    recipe.AddIngredient(null, "StableKiCrystal", 100);
        //    recipe.AddIngredient(ItemID.ManaCrystal, 3);
        //    recipe.AddIngredient(ItemID.Book, 1);
        //    recipe.AddTile(null, "ZTable");
        //    recipe.SetResult(this);
        //    recipe.AddRecipe();
        //}
    }
}
