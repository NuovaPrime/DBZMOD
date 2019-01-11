﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ3Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan 3");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            DamageMulti = 2.9f;
            SpeedMulti = 2.9f;
            KiDrainBuffMulti = 1.95f;
            KiDrainRate = 2.65f;
            KiDrainRateWithMastery = 1.325f;
            BaseDefenceBonus = 12;
            Description.SetDefault(AssembleTransBuffDescription() + "\n(Life drains when below 30% Max Ki)");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            bool isMastered = modPlayer.MasteryLevel3 >= 1f;

            KiDrainRate = !isMastered ? KiDrainRate : KiDrainRateWithMastery;
            float kiQuotient = modPlayer.GetKi() / modPlayer.OverallKiMax();
            if (kiQuotient <= 0.3f)
            {
                HealthDrainRate = isMastered ? 10 : 20;
            } else
            {
                HealthDrainRate = 0;
            }
            base.Update(player, ref buffIndex);
        }
    }
}

