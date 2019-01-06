﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class KiStimulantBuff : ModBuff
    {
        private int StimulantTimer;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ki Stimulance");
            Description.SetDefault("Your body is enhanced, passively regenerating ki.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer.ModPlayer(player).AddKi(0.35f, false, false);
        }
    }
}
