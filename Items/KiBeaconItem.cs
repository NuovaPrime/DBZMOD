﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class KiBeaconItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ki Beacon");
            Tooltip.SetDefault("'Radiates Ki you can lock on to with Instant Transmission.'");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 7;
            item.value = 120000;
            item.createTile = mod.TileType("KiBeaconTile");
        }
    }
}