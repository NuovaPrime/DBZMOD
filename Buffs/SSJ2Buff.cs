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
            DamageMulti = 2.5f;
            SpeedMulti = 2.5f;
            KiDrainBuffMulti = 1.7f;
            KiDrainRateWithMastery = 2;
            KiDrainRate = 4;
            Description.SetDefault(AssembleTransBuffDescription());
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).MasteryLevel2 >= 1;

            KiDrainRate = isMastered ? KiDrainRate : KiDrainRateWithMastery;

            MasteryTimer++;
            if (!(MyPlayer.ModPlayer(player).playerTrait == "Prodigy") && MasteryTimer >= 300 && MyPlayer.ModPlayer(player).MasteryMax2 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel2 += 0.01f;
                MasteryTimer = 0;
            }
            else if (MyPlayer.ModPlayer(player).playerTrait == "Prodigy" && MasteryTimer >= 150 && MyPlayer.ModPlayer(player).MasteryMax2 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel2 += 0.01f;
                MasteryTimer = 0;
            }
            base.Update(player, ref buffIndex);
        }
    }
}

