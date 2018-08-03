﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace DBZMOD.Items.Consumables
{
    public class KiAggravationStone : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.expert = true;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ki Aggravation Stone");
            Tooltip.SetDefault("Activates Realistic Mode."
        + "\nRealistic Mode makes some content more true to Dragon Ball Z."
        + "\nTransformations get proper multipliers."
        + "\nBosses get massive health boosts and damage boosts to compensate."
        + "\nTransformations don't increase ki usage."
        + "\nUnlocks god level transformations.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.RealismMode = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.RealismMode)
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
