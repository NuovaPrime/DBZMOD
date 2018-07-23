using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class USSJItem : ModItem
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
            item.rare = 2;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Ascension Doll");
            Tooltip.SetDefault("Its erupting with energy, it might even allow you to reach a higher state of super saiyan.");
        }


        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You feel your super saiyan form evolve for the last time.");
            }
            MyPlayer.ModPlayer(player).USSJAchieved = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).USSJAchieved)
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
