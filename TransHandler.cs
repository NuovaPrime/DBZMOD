﻿using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using DBZMOD;
using Terraria;

namespace DBZMOD
{
    public abstract class TransBuff : ModBuff
    {
        public float DamageMulti;
        public float SpeedMulti;
        public float KaioLightValue;
        public float KiDrainBuffMulti;
        public bool IsKaioken;
        public float SSJLightValue;
        public bool IsSSJ;
        public int HealthDrainRate;
        public int KiDrainRate;
        private int KiDrainTimer;
        private int KiDrainAddTimer;
        private int KiDrainAddition;
        public bool RealismModeOn;
        public int MasteryTimer;
        public override void Update(Player player, ref int buffIndex)
        {
            KiDrainAdd(player);
            if(IsKaioken)
            {
                MyPlayer.ModPlayer(player).hasKaioken = true;
                Lighting.AddLight(player.Center, KaioLightValue, 0f, 0f);
            }
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegenTime = 0;
            player.lifeRegen -= HealthDrainRate;
            if (IsSSJ)
            {
                MyPlayer.ModPlayer(player).IsTransformed = true;
                KiDrainTimer++;
                if(KiDrainTimer > 1)
                {
                    MyPlayer.ModPlayer(player).KiCurrent -= KiDrainRate;
                    KiDrainTimer = 0;
                }
                KiDrainAddTimer++;
                if(KiDrainAddTimer > 600)
                {
                    KiDrainAddition += 1;
                    KiDrainAddTimer = 0;
                }
                Main.NewText("Ki Drain Addition is " + KiDrainAddition);
                KiDrainRate += KiDrainAddition;
                Lighting.AddLight(player.Center, 1f, 1f, 0f);
            }
            if (MyPlayer.ModPlayer(player).speedToggled)
            {
                player.moveSpeed += SpeedMulti - 1f;
                player.maxRunSpeed += SpeedMulti - 1f;
                player.runAcceleration += SpeedMulti - 1f;
            }
            else if (!MyPlayer.ModPlayer(player).speedToggled)
            {
                player.moveSpeed += 2f;
                player.maxRunSpeed += 2f;
                player.runAcceleration += 2f;
            }
            player.meleeDamage += DamageMulti - 1;
            player.rangedDamage += DamageMulti - 1;
            player.magicDamage += DamageMulti - 1;
            player.minionDamage += DamageMulti - 1;
            player.thrownDamage += DamageMulti - 1;
            MyPlayer.ModPlayer(player).KiDamage += DamageMulti - 1;
            if (DBZMOD.instance.thoriumLoaded)
            {
                ThoriumEffects(player);
            }
            if (DBZMOD.instance.tremorLoaded)
            {
                TremorEffects(player);
            }
            if (DBZMOD.instance.enigmaLoaded)
            {
                EnigmaEffects(player);
            }
            if (DBZMOD.instance.battlerodsLoaded)
            {
                BattleRodEffects(player);
            }
            if(IsSSJ)
            {
                if(MyPlayer.ModPlayer(player).KiCurrent <= 0)
                {
                    MyPlayer.ModPlayer(player).EndTransformations();
                }
            }
            if(!IsSSJ)
            {
                KiDrainAddition = 0;
            }


        }
        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage += DamageMulti - 1;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost += DamageMulti - 1;
        }
        public void TremorEffects(Player player)
        {
            player.GetModPlayer<Tremor.MPlayer>(ModLoader.GetMod("Tremor")).alchemicalDamage += DamageMulti - 1;
        }
        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).mysticDamage += DamageMulti - 1;
        }
        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage += DamageMulti - 1;
        }
        private void KiDrainAdd(Player player)
        {
            MyPlayer.ModPlayer(player).KiDrainMulti = KiDrainBuffMulti;
        }


    }
}

