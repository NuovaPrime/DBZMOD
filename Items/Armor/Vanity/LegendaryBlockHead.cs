﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class LegendaryBlockHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The head of an extremely ancient god.");
            DisplayName.SetDefault("Legendary Blockhead");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 16;
            item.value = 30000;
            item.rare = 9;
            item.expert = true;
            item.vanity = true;
        }
    }
}