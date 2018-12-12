﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs.DevBuffs
{
    public class SSJSBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan Spectrum");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Description.SetDefault("69x Damage, 69x Speed, Gives infinite ki regen" +
                "\n'A form far beyond comprehension, only available to the true god Nuova.'");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            DamageMulti = 69f;
            SpeedMulti = 69f;
            
            KiDrainBuffMulti = 1f;
            modPlayer.KiRegen += 9999;
            modPlayer.KiMax2 += 999999;
            base.Update(player, ref buffIndex);
        }
    }
}

