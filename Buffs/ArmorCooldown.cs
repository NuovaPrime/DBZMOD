﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class ArmorCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Armor Cooldown");
            Description.SetDefault("You must wait to use your armor bonus again.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
