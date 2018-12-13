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
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Description.SetDefault(AssembleTransBuffDescription());
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            DamageMulti = 4f;
            SpeedMulti = 4f;
            bool isMastered = modPlayer.MasteryLevel3 >= 1f;
            KiDrainRate = isMastered ? 4 : 6;
            float kiQuotient = (float)modPlayer.GetKi() / modPlayer.OverallKiMax();
            if (kiQuotient <= 0.3f)
            {
                HealthDrainRate = isMastered ? 10 : 20;
            } else
            {
                HealthDrainRate = 0;
            }
            
            KiDrainBuffMulti = 2.1f;
            MasteryTimer++;
            if (!(MyPlayer.ModPlayer(player).playerTrait == "Prodigy") && MasteryTimer >= 300 && MyPlayer.ModPlayer(player).MasteryMax3 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel3 += 0.01f;
                MasteryTimer = 0;
            }
            else if (MyPlayer.ModPlayer(player).playerTrait == "Prodigy" && MasteryTimer >= 150 && MyPlayer.ModPlayer(player).MasteryMax3 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel3 += 0.01f;
                MasteryTimer = 0;
            }
            base.Update(player, ref buffIndex);
        }
    }
}

