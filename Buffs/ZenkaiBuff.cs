﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class ZenkaiBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Zenkai");
            Description.SetDefault("Your inherent saiyan ability is active.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti *= 2f;
            MyPlayer.ModPlayer(player).zenkaiCharmActive = true;
        }
    }
}
