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
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("2.3x Damage, 2x Speed, Drains Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 2.3f;
            SpeedMulti = 2f;
            KiDrainRate = 4;
            KiDrainBuffMulti = 1.4f;
            base.Update(player, ref buffIndex);
        }
    }
}

