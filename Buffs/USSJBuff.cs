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
            Description.SetDefault("2.1x Damage, 10% less Speed, Drains alot of Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 2.1f;
            SpeedMulti = 1.9f;
            KiDrainRate = 7;
            KiDrainBuffMulti = 1.6f;
            base.Update(player, ref buffIndex);
        }
    }
}

