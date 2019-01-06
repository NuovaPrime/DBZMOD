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
            Main.debuff[Type] = false;
            DamageMulti = 1.50f;
            SpeedMulti = 1.50f;
            KiDrainBuffMulti = 1.25f;
            KiDrainRate = 1;
            KiDrainRateWithMastery = 0.5f;
            BaseDefenceBonus = 4;
            Description.SetDefault(AssembleTransBuffDescription());
        }

        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).MasteryLevel1 >= 1;
            
            KiDrainRate = isMastered ? KiDrainRate : KiDrainRateWithMastery;
            base.Update(player, ref buffIndex);
        }        
    }
}

