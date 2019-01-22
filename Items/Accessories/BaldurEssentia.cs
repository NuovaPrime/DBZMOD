﻿using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class BaldurEssentia : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The essence of strong defense." +
                "\nCharging grants a protective barrier that grants drastically increased defense");
            DisplayName.SetDefault("Baldur Essentia");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 6000;
            item.rare = 4;
            item.defense = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).baldurEssentia = true;
            }
        }
    }
}