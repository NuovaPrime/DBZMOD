﻿using Terraria.ModLoader;
using Terraria;
using System;
using DBZMOD.Util;
using DBZMOD.Buffs;

namespace DBZMOD
{
    public abstract class TransBuff : ModBuff
    {
        public float DamageMulti;
        public float SpeedMulti;
        public float KiDrainBuffMulti;
        public float SSJLightValue;
        public int HealthDrainRate;
        public int OverallHealthDrainRate;
        public float KiDrainRate;
        public float KiDrainRateWithMastery;
        private int KiDrainAddTimer;
        public bool RealismModeOn;
        public int BaseDefenceBonus;
        public int PrecentDefenceBonus;

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            
            KiDrainAdd(player);
            if(Transformations.IsAnyKaioken(player) || Transformations.IsSSJG(player))
            {
                Lighting.AddLight(player.Center + player.velocity * 8f, 0.2f, 0f, 0f);
            } else if (Transformations.IsLSSJ1(player) || Transformations.IsLSSJ2(player))
            {                
                Lighting.AddLight(player.Center + player.velocity * 8f, 0f, 0.2f, 0f);
            } else if (Transformations.IsSSJ1(player) || Transformations.IsSSJ2(player) || Transformations.IsSSJ2(player) || Transformations.IsASSJ(player) || Transformations.IsUSSJ(player))
            {
                Lighting.AddLight(player.Center + player.velocity * 8f, 0.2f, 0.2f, 0f);
            } else if (Transformations.IsSpectrum(player))
            {
                var rainbow = Main.DiscoColor;
                Lighting.AddLight(player.Center + player.velocity * 8f, rainbow.R / 512f, rainbow.G / 512f, rainbow.B / 512f);
            }

            //give bonus base defense
            player.statDefense += BaseDefenceBonus;
            
            // if the player is in any ki-draining state, handles ki drain and power down when ki is depleted
            if (Transformations.IsAnythingOtherThanKaioken(player))
            {
                // player ran out of ki, so make sure they fall out of any forms they might be in.
                if (modPlayer.IsKiDepleted())
                {
                    if (Transformations.IsSuperKaioken(player))
                    {
                        modPlayer.KaiokenLevel = 0;
                    }
                    Transformations.EndTransformations(player, true, false);
                }
                else
                {
                    modPlayer.AddKi((KiDrainRate + modPlayer.KiDrainAddition) * -1, false, true);
                    KiDrainAddTimer++;
                    if (KiDrainAddTimer > 600)
                    {
                        modPlayer.KiDrainAddition += 1;
                        KiDrainAddTimer = 0;
                    }
                    Lighting.AddLight(player.Center, 1f, 1f, 0f);
                }
            } else
            {
                // the player isn't in a ki draining state anymore, reset KiDrainAddition
                modPlayer.KiDrainAddition = 0;                
            }
            
            player.moveSpeed *= GetModifiedSpeedMultiplier(modPlayer);
            player.maxRunSpeed *= GetModifiedSpeedMultiplier(modPlayer);
            player.runAcceleration *= GetModifiedSpeedMultiplier(modPlayer);
            if (player.jumpSpeedBoost < 1f)
                player.jumpSpeedBoost = 1f;
            player.jumpSpeedBoost *= GetModifiedSpeedMultiplier(modPlayer);

            // set player damage  mults
            player.meleeDamage *= GetHalvedDamageBonus();
            player.rangedDamage *= GetHalvedDamageBonus();
            player.magicDamage *= GetHalvedDamageBonus();
            player.minionDamage *= GetHalvedDamageBonus();
            player.thrownDamage *= GetHalvedDamageBonus();
            modPlayer.KiDamage *= DamageMulti;

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

        public float GetModifiedSpeedMultiplier(MyPlayer modPlayer)
        {
            return 1f + ((SpeedMulti - 1f) * modPlayer.bonusSpeedMultiplier);
        }

        public float GetHalvedDamageBonus()
        {
            return 1f + ((DamageMulti - 1f) * 0.5f);
        }

        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= GetHalvedDamageBonus();
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= GetHalvedDamageBonus();
        }

        public void TremorEffects(Player player)
        {
            player.GetModPlayer<Tremor.MPlayer>(ModLoader.GetMod("Tremor")).alchemicalDamage *= GetHalvedDamageBonus();
        }

        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).mysticDamage *= GetHalvedDamageBonus();
        }

        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage *= GetHalvedDamageBonus();
        }

        public void ExpandedSentriesEffects(Player player)
        {
            player.GetModPlayer<ExpandedSentries.ESPlayer>(ModLoader.GetMod("ExpandedSentries")).sentryDamage *= GetHalvedDamageBonus();
        }

        private void KiDrainAdd(Player player)
        {
            MyPlayer.ModPlayer(player).KiDrainMulti = KiDrainBuffMulti;
        }

        public string GetPercentForDisplay(string currentDisplayString, string text, int percent)
        {
            if (percent == 0)
                return currentDisplayString;
            return string.Format("{0}{1} {2}{3}%", currentDisplayString, text, percent > 0 ? "+" : string.Empty, percent);
        }

        public int KaiokenLevel = 0;
        public string AssembleTransBuffDescription()
        {
            string KaiokenName = string.Empty;
            if (Type == Transformations.Kaioken.GetBuffId() || Type == Transformations.SuperKaioken.GetBuffId())
            {
                switch (KaiokenLevel)
                {
                    case 2:
                        KaiokenName = "(x3)\n";
                        break;
                    case 3:
                        KaiokenName = "(x4)\n";
                        break;
                    case 4:
                        KaiokenName = "(x10)\n";
                        break;
                    case 5:
                        KaiokenName = "(x20)\n";
                        break;
                }
            }
            int percentDamageMult = (int)Math.Round(DamageMulti * 100f, 0) - 100;
            int percentSpeedMult = (int)Math.Round(SpeedMulti * 100f, 0) - 100;
            float kiDrainPerSecond = 60f * KiDrainRate;
            float kiDrainPerSecondWithMastery = 60f * KiDrainRateWithMastery;
            int percentKiDrainMulti = (int)Math.Round(KiDrainBuffMulti * 100f, 0) - 100;
            string displayString = KaiokenName;
            displayString = GetPercentForDisplay(displayString, "Damage", percentDamageMult);
            displayString = GetPercentForDisplay(displayString, " Speed", percentSpeedMult);
            displayString = GetPercentForDisplay(displayString, "\nKi Costs", percentKiDrainMulti);
            if (kiDrainPerSecond > 0)
            {
                displayString = string.Format("{0}\nKi Drain: {1}/s", displayString, (int)Math.Round(kiDrainPerSecond, 0));
                if (kiDrainPerSecondWithMastery > 0)
                {
                    displayString = string.Format("{0}, {1}/s when mastered", displayString, (int)Math.Round(kiDrainPerSecondWithMastery, 0));
                }
            }
            if (HealthDrainRate > 0)
            {
                displayString = string.Format("{0}\nLife Drain: -{1}/s.", displayString, HealthDrainRate / 2);
            }
            return displayString;
        }

        public static int GetTotalHealthDrain(Player player)
        {
            var healthDrain = KaiokenBuff.GetHealthDrain(player.GetModPlayer<MyPlayer>()) + SuperKaiokenBuff.GetHealthDrain(player);
            return healthDrain;
        }
    }
}

