using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
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
            DisplayName.SetDefault("Kaioken 2x");
            Tooltip.SetDefault("Unlocks an ancient technique.");
        }


        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You feel your power surge.");
            }
            MyPlayer.ModPlayer(player).KaioAchieved = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).KaioAchieved)
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
