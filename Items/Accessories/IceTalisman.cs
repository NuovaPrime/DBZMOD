﻿using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class IceTalisman : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A frozen talisman that seems to make even your soul cold.'\n7% Increased Ki damage\nIncreased Ki regen\nCharging grants a aura of frostburn around you.");
            DisplayName.SetDefault("Ice Energy Talisman");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 45000;
            item.rare = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.07f;
                player.GetModPlayer<MyPlayer>(mod).kiRegen += 2;
                player.GetModPlayer<MyPlayer>(mod).iceTalisman = true;
            }
        }
    }
}