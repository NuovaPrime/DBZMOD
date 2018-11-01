﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class DisgustingGoopBuff : ModBuff
    {
        private int GoopTimer;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Disgusting Goop");
            Description.SetDefault("Your ki seems stablized, but you also feel sick.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 -= 25;
            GoopTimer++;
            if (GoopTimer > 3 && MyPlayer.ModPlayer(player).KiCurrent >= 0)
            {
                MyPlayer.ModPlayer(player).KiCurrent += 1;
                GoopTimer = 0;
            }
        }
    }
}
