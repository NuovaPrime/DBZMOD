﻿using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class ArmCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An old arm blaster used by many soldiers.\n10% Reduced Ki usage\nIncreased charge speed");
            DisplayName.SetDefault("Arm Cannon");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 6000;
            item.rare = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiDrainMulti -= 0.10f;
                player.GetModPlayer<MyPlayer>(mod).kiChargeRate += 1;
                player.GetModPlayer<MyPlayer>(mod).armCannon = true;
            }
        }
    }
}