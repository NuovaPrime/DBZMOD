﻿using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class SpiritualEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The emblem seems to have weird writing inscribed on it."
               + "\nGives 15% Increased Ki Damage.");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 80000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {

                player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.15f;
                player.GetModPlayer<MyPlayer>(mod).spiritualEmblem = true;
            }
        }
    }
}