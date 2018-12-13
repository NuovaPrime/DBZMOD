﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class ASSJBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ascended Super Saiyan");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            DamageMulti = 1.9f;
            SpeedMulti = 1.9f;
            KiDrainRate = 4;
            KiDrainBuffMulti = 1.4f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

