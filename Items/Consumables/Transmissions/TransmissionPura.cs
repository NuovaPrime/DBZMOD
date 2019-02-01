using DBZMOD.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.Transmissions
{
    public class TransmissionPura : ModItem
    {

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 36;
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
            DisplayName.SetDefault("I.T. Tome Vol 3 - Pura Facilioris Transmissus");
            Tooltip.SetDefault("'It holds an alien power, bending space to your will with ease.'");
        }

        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).isInstantTransmission3Unlocked = true;
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You have unlocked Instant Transmission Lv3.\nInstant Transmission free roaming costs\nare significantly reduced.");
                return true;
            }
            return true;

        }

        public override bool CanUseItem(Player player)
        {
            // no if book 3 is unlocked, no if book 2 isn't read yet.
            if (MyPlayer.ModPlayer(player).isInstantTransmission3Unlocked || !MyPlayer.ModPlayer(player).isInstantTransmission2Unlocked)
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
            recipe.AddIngredient(null, "PureKiCrystal", 20);
            recipe.AddIngredient(null, "RadiantFragment", 1);
            recipe.AddIngredient(ItemID.FragmentNebula, 1);
            recipe.AddIngredient(ItemID.FragmentSolar, 1);
            recipe.AddIngredient(ItemID.FragmentStardust, 1);
            recipe.AddIngredient(ItemID.FragmentVortex, 1);
            recipe.AddIngredient(ItemID.LunarBar, 4);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
