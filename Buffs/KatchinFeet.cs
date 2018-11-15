﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class KatchinFeet : ModBuff
    {
        private int StimulantTimer;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Katchin Feet");
            Description.SetDefault("Your feet are infused with ki, making them as hard as katchin.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noFallDmg = true;
        }
    }
}
