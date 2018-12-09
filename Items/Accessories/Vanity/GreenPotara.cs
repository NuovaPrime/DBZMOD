﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DBZMOD.Items.Accessories.Vanity
{
    [AutoloadEquip(EquipType.Face)]
    public class GreenPotara : PatreonItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A useful pair of earrings used by the kais.'");
            DisplayName.SetDefault("Green Potaras");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 110000;
            item.rare = 5;
            item.accessory = true;
            item.vanity = true;
            PatreonName = "FullNovaAlchemist";
        }
    }
}