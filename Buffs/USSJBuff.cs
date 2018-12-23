﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class USSJBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ultra Super Saiyan");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            DamageMulti = 1.90f;
            SpeedMulti = 0.9f;
            KiDrainRate = 5;
            KiDrainBuffMulti = 1.6f;
            BaseDefenceBonus = 16;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

