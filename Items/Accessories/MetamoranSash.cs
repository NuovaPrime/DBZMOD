﻿using DBZMOD;
 using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class MetamoranSash : PatreonItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Your own bad energy will be your undoing!'\n10% Increased Ki damage\n30% Reduced Ki usage\n15% chance to do double damage.");
            DisplayName.SetDefault("Metamoran Sash");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 40000;
            item.rare = -12;
            item.defense = 3;
            item.accessory = true;
            patreonName = "Chese780";
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.1f;
				player.GetModPlayer<MyPlayer>(mod).kiDrainMulti -= 0.3f;
                player.GetModPlayer<MyPlayer>(mod).metamoranSash = true;
            }
        }
    }
}