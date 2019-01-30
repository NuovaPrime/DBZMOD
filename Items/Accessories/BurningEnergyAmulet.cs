﻿using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class BurningEnergyAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A glowing amulet that radiates with extreme heat.'" +
                "\n5% Increased Ki damage" +
                "\n+200 Max ki" +
                "\nCharging grants a aura of fire around you.");
            DisplayName.SetDefault("Burning Energy Amulet");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 40000;
            item.rare = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.05f;
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 200;
                player.GetModPlayer<MyPlayer>(mod).burningEnergyAmulet = true;
            }
        }
    }
}