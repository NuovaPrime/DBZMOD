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
            Main.debuff[Type] = true;
            DamageMulti = 2.1f;
            SpeedMulti = 0.9f;
            KiDrainRate = 7;
            KiDrainBuffMulti = 1.6f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

