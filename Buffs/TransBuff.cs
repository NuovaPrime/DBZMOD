﻿using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using DBZMOD;
using Terraria;
using System;
using DBZMOD.Items.Accessories;
using Util;

namespace DBZMOD
{
    public abstract class TransBuff : ModBuff
    {
        public float DamageMulti;
        public float SpeedMulti;
        public float KaioLightValue;
        public float KiDrainBuffMulti;
        public float SSJLightValue;
        public int HealthDrainRate;
        public int OverallHealthDrainRate;
        public int KiDrainRate;
        private int KiDrainTimer;
        private int KiDrainAddTimer;
        public bool RealismModeOn;
        public int MasteryTimer;
        public override void Update(Player player, ref int buffIndex)
        {
            KiDrainAdd(player);
            if(Transformations.IsKaioken(player))
            {
                Lighting.AddLight(player.Center, KaioLightValue, 0f, 0f);
            }

            // only neuter the life regen if this is a draining buff.
            if (HealthDrainRate > 0)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 0;

                // only apply the kaio crystal benefit if this is kaioken
                bool isKaioCrystalEquipped = player.IsAccessoryEquipped("Kaio Crystal");
                float drainMult = (Transformations.IsKaioken(player) && isKaioCrystalEquipped ? 0.5f : 1f);

                // recalculate the final health drain rate and reduce regen by that amount
                OverallHealthDrainRate = (int)Math.Ceiling((float)HealthDrainRate * drainMult);
                player.lifeRegen -= OverallHealthDrainRate;
            }
            
            // if the player is in any ki-draining state, handles ki drain and power down when ki is depleted
            if (Transformations.IsSSJ(player) || Transformations.IsLSSJ(player) || Transformations.IsSSJ1Kaioken(player))
            {
                // player ran out of ki, so make sure they fall out of any forms they might be in.
                if (MyPlayer.ModPlayer(player).KiCurrent <= 0)
                {
                    MyPlayer.ModPlayer(player).EndTransformations();
                }
                else
                {
                    // player still has some ki, perform drain routine
                    KiDrainTimer++;
                    if (KiDrainTimer > 2)
                    {
                        MyPlayer.ModPlayer(player).KiCurrent -= KiDrainRate + MyPlayer.ModPlayer(player).KiDrainAddition;
                        KiDrainTimer = 0;
                    }
                    KiDrainAddTimer++;
                    if (KiDrainAddTimer > 600)
                    {
                        MyPlayer.ModPlayer(player).KiDrainAddition += 1;
                        KiDrainAddTimer = 0;
                    }
                    Lighting.AddLight(player.Center, 1f, 1f, 0f);
                }
            } else
            {
                // the player isn't in a ki draining state anymore, reset KiDrainAddition
                MyPlayer.ModPlayer(player).KiDrainAddition = 0;                
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

            // set player damage  mults
            player.meleeDamage += (DamageMulti - 1) * 0.5f;
            player.rangedDamage += (DamageMulti - 1) * 0.5f;
            player.magicDamage += (DamageMulti - 1) * 0.5f;
            player.minionDamage += (DamageMulti - 1) * 0.5f;
            player.thrownDamage += (DamageMulti - 1) * 0.5f;
            MyPlayer.ModPlayer(player).KiDamage += (DamageMulti - 1);

            // cross mod support stuff
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
            if (DBZMOD.instance.expandedSentriesLoaded)
            {
                ExpandedSentriesEffects(player);
            }
        }

        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage += (DamageMulti - 1) * 0.5f;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost += (DamageMulti - 1) * 0.5f;
        }

        public void TremorEffects(Player player)
        {
            player.GetModPlayer<Tremor.MPlayer>(ModLoader.GetMod("Tremor")).alchemicalDamage += (DamageMulti - 1) * 0.5f;
        }

        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).mysticDamage += (DamageMulti - 1) * 0.5f;
        }

        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage += (DamageMulti - 1) * 0.5f;
        }

        public void ExpandedSentriesEffects(Player player)
        {
            player.GetModPlayer<ExpandedSentries.ESPlayer>(ModLoader.GetMod("ExpandedSentries")).sentryDamage += (DamageMulti - 1) * 0.5f;
        }

        private void KiDrainAdd(Player player)
        {
            MyPlayer.ModPlayer(player).KiDrainMulti = KiDrainBuffMulti;
        }
    }
}

