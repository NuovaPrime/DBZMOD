﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ1Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("{0}x Damage, {0}x Speed, Drains Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (DBZWorld.RealismMode)
            {
                DamageMulti = 5f;
                SpeedMulti = 5f;
                if (MyPlayer.ModPlayer(player).MasteryLevel1 >= 1)
                {
                    KiDrainRate = 0;
                }
                else
                {
                    KiDrainRate = 2;
                }
                KiDrainBuffMulti = 1f;
            }
            else if (!DBZWorld.RealismMode)
            {
                DamageMulti = 2f;
                SpeedMulti = 2f;
                if (MyPlayer.ModPlayer(player).MasteryLevel1 >= 1)
                {
                    KiDrainRate = 1;
                }
                else
                {
                    KiDrainRate = 3;
                }
                KiDrainBuffMulti = 2f;
            }

            MasteryTimer++;
            if (MasteryTimer >= 300 && MyPlayer.ModPlayer(player).MasteryMax1 <= 1)
            {
                MyPlayer.ModPlayer(player).MasteryLevel1 += 0.01f;
                MasteryTimer = 0;
            }
            base.Update(player, ref buffIndex);
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (RealismModeOn)
            {
                tip = string.Format(tip, "5");
            }
            else
            {
                tip = string.Format(tip, "2");
            }

        }
    }
}

