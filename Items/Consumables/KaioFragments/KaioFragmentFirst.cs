using DBZMOD;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.KaioFragments
{
    public class KaioFragmentFirst : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 2;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kaioken");
            Tooltip.SetDefault("Unlocks an ancient technique.");
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You feel your power surge.");
            }
            MyPlayer.ModPlayer(player).kaioAchieved = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).kaioAchieved)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
