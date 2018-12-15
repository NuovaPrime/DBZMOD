﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class EndlessKi : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The hearts of the gods above gods.'" +
                "\n+1500000 Max Ki" +
                "\nNear Infinite Ki Regen");
            DisplayName.SetDefault("Endless Ki");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 1200000;
            item.rare = -12;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiMax2 += 1500000;
                player.GetModPlayer<MyPlayer>(mod).KiRegen += 1000000;
            }
        }
    }
}