using DBZMOD;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.KaioFragments
{
    public class KaioFragment1 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
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
            DisplayName.SetDefault("Kaioken 3x");
            Tooltip.SetDefault("Unlocks the next level of kaioken.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).kaioFragment1 = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).kaioFragment1 || !MyPlayer.ModPlayer(player).kaioAchieved)
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
