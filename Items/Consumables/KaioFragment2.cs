using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class KaioFragment2 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 5;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kaioken 4x");
            Tooltip.SetDefault("Unlocks the next level of kaioken.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).kaioFragment2 = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).kaioFragment2 || !MyPlayer.ModPlayer(player).kaioFragment1)
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
