﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ2Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan 2");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("2.5x Damage, 2.5x Speed, Quickly Drains Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 2.5f;
            SpeedMulti = 2.5f;
            if (MyPlayer.ModPlayer(player).MasteryLevel2 >= 1)
            {
                KiDrainRate = 2;
            }
            else
            {
                KiDrainRate = 4;
            }
            KiDrainBuffMulti = 1.7f;
            MasteryTimer++;
            if (MasteryTimer >= 300 && MyPlayer.ModPlayer(player).MasteryMax2 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel2 += 0.01f;
                MasteryTimer = 0;
            }
            base.Update(player, ref buffIndex);
        }
    }
}

