﻿using DBZMOD.Players;
 using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class GoblinKiEnhancer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A relic of the ancient goblins.'\n+500 Max ki\nGetting hit grants massively increased ki regen for a short time.");
            DisplayName.SetDefault("Goblin Ki Enhancer");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 30000;
            item.rare = 3;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 500;
                player.GetModPlayer<MyPlayer>(mod).goblinKiEnhancer = true;
            }
        }
    }
}