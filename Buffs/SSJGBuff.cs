﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJGBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan God");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            DamageMulti = 3.5f;
            SpeedMulti = 3.5f;
            KiDrainRate = 10;
            KiDrainRateWithMastery = 5;
            KiDrainBuffMulti = 1.5f;
            BaseDefenceBonus = 65;
            Description.SetDefault(AssembleTransBuffDescription() + "\nSlightly increased health regen.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 2;
            base.Update(player, ref buffIndex);
        }
    }
}

