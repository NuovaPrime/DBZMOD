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
            Main.debuff[Type] = true;
            Description.SetDefault(AssembleTransBuffDescription() + "\nSlightly increased health regen.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 5f;
            SpeedMulti = 5f;
            KiDrainRate = 8;
            KiDrainBuffMulti = 1.5f;
            player.lifeRegen += 2;
            base.Update(player, ref buffIndex);
        }
    }
}

