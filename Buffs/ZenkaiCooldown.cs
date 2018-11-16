﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class ZenkaiCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Zenkai Cooldown");
            Description.SetDefault("Your zenkai ability is on cooldown.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
