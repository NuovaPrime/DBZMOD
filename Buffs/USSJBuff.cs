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
            KiDrainRate = 1.5f;
            KiDrainRateWithMastery = 0.75f;
            KiDrainBuffMulti = 1.6f;
            BaseDefenceBonus = 6;
            Description.SetDefault(AssembleTransBuffDescription());
        }

        // per Nova's design, mastery applies to ASSJ and USSJ
        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).MasteryLevel1 >= 1;

            KiDrainRate = isMastered ? KiDrainRate : KiDrainRateWithMastery;

            base.Update(player, ref buffIndex);
        }
    }
}

