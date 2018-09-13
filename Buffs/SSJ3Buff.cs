﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ3Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan 3");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("4x Damage, 4x Speed, Rapidly Drains Ki and slightly drains life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 4f;
            SpeedMulti = 4f;
            if (MyPlayer.ModPlayer(player).MasteryLevel3 >= 1)
            {
                KiDrainRate = 4;
                HealthDrainRate = 10;
            }
            else
            {
                KiDrainRate = 6;
                HealthDrainRate = 20;
            }
            KiDrainBuffMulti = 2.1f;
            MasteryTimer++;
            if (MasteryTimer >= 300 && MyPlayer.ModPlayer(player).MasteryMax3 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel3 += 0.01f;
                MasteryTimer = 0;
            }
            base.Update(player, ref buffIndex);
        }
    }
}

