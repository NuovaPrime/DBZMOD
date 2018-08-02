﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class KiFragment1 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 20;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 2;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Novice Ki Fragment");
            Tooltip.SetDefault("Increases your max ki by 1000.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).KiMax += 1000;
            MyPlayer.ModPlayer(player).Fragment1 = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).Fragment1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
