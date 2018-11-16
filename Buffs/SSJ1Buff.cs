﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ1Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("75% more Damage, 75% more Speed, Drains Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 1.75f;
            SpeedMulti = 1.75f;
            if (MyPlayer.ModPlayer(player).MasteryLevel1 >= 1)
            {
                KiDrainRate = 1;
            }
            else
            {
                KiDrainRate = 3;
            }
            KiDrainBuffMulti = 1.3f;

            MasteryTimer++;
            if (MyPlayer.ModPlayer(player).playerTrait == null && MasteryTimer >= 300 && MyPlayer.ModPlayer(player).MasteryMax1 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel1 += 0.01f;
                MasteryTimer = 0;
            }
            else if(MyPlayer.ModPlayer(player).playerTrait == "Prodigy" && MasteryTimer >= 150 && MyPlayer.ModPlayer(player).MasteryMax1 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel1 += 0.01f;
                MasteryTimer = 0;
            }
            base.Update(player, ref buffIndex);
        }
    }
}

