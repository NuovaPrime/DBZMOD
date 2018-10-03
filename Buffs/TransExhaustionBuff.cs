﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class TransExhaustionBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Transformation Exhaustion");
            Description.SetDefault("Your body can't handle another transformation.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
