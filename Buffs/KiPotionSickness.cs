﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class KiPotionSickness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ki Potion Sickness");
            Description.SetDefault("You feel sick at the thought of another ki potion.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }
    }
}
