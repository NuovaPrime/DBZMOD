using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class TransmissionTeleport : ModItem
    {

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 34;
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
            DisplayName.SetDefault("I.T. Tome Vol 2 - Ianuae Magicae");
            Tooltip.SetDefault("'It holds an alien power, bending space to your will.'");
        }

        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).isInstantTransmission2Unlocked = true;
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You have unlocked Instant Transmission Lv2 (Supreme Kai Teleportation)."
                + "\nUse your Instant Transmission hotkey and cursor to seek a remote destination.");
                return true;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).isInstantTransmission2Unlocked)
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
            recipe.AddIngredient(null, "AngerKiCrystal", 30);            
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.RodofDiscord, 1);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
