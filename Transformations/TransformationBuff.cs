using System;
using DBZMOD.Buffs;
using DBZMOD.Buffs.SSJBuffs;
using DBZMOD.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Transformations
{
    public abstract class TransformationBuff : ModBuff
    {
        protected TransformationBuff(TransformationDefinition transformationDefinition)
        {
            TransformationDefinition = transformationDefinition;
        }

        private const int MINIMUM_TRANSFORMATION_FRAMES_FOR_KI_COST_UPKEEP = 1;
        public float damageMulti;
        public float speedMulti;
        public float kiDrainBuffMulti;
        public float ssjLightValue;
        public int healthDrainRate;
        public int overallHealthDrainRate;
        public float kiDrainRate;
        public float kiDrainRateWithMastery;
        private int _kiDrainAddTimer;
        private int _overloadDrainAddTimer;
        public bool realismModeOn;
        public int baseDefenceBonus;
        public int precentDefenceBonus;

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            
            KiDrainAdd(player);
            if(player.IsAnyKaioken() || player.IsSSJG())
            {
                Lighting.AddLight(player.Center + player.velocity * 8f, 0.2f, 0f, 0f);
            } else if (player.IsLSSJ1() || player.IsLSSJ2())
            {                
                Lighting.AddLight(player.Center + player.velocity * 8f, 0f, 0.2f, 0f);
            } else if (player.IsSSJ1() || player.IsSSJ2() || player.IsSSJ2() || player.IsAssj() || player.IsUssj())
            {
                Lighting.AddLight(player.Center + player.velocity * 8f, 0.2f, 0.2f, 0f);
            } else if (player.IsSpectrum())
            {
                var rainbow = Main.DiscoColor;
                Lighting.AddLight(player.Center + player.velocity * 8f, rainbow.R / 512f, rainbow.G / 512f, rainbow.B / 512f);
            }

            //give bonus base defense
            player.statDefense += baseDefenceBonus;
            

            // handle ki drain mastery
            bool isMastered = modPlayer.masteryLevels.ContainsKey(this.Name) &&
                              modPlayer.masteryLevels[this.Name] >= 1.0;
            float actualKiDrain = isMastered ? kiDrainRateWithMastery : kiDrainRate;
            float kiDrainMultiplier = 1f;
            if (!isMastered)
            {
                if (kiDrainMultiplier < 2.5f)
                {
                    _kiDrainAddTimer++;
                    if (_kiDrainAddTimer >= 300)
                    {
                        kiDrainMultiplier += 0.5f;
                        _kiDrainAddTimer = 0;
                    }
                }
            }
            else
            {
                if (kiDrainMultiplier < 3f)
                {
                    _kiDrainAddTimer++;
                    if (_kiDrainAddTimer >= 600)
                    {
                        kiDrainMultiplier += 1f;
                        _kiDrainAddTimer = 0;
                    }
                }
            }

            float projectedKiDrain = actualKiDrain * kiDrainMultiplier;

            // if the player is in any ki-draining state, handles ki drain and power down when ki is depleted
            if (player.IsAnythingOtherThanKaioken())
            {
                // player can't support ten frames (arbitrary) of their current form, make them fall out of any forms they might be in.
                if (modPlayer.IsKiDepleted(projectedKiDrain * MINIMUM_TRANSFORMATION_FRAMES_FOR_KI_COST_UPKEEP))
                {
                    if (player.IsSuperKaioken())
                    {
                        modPlayer.kaiokenLevel = 0;
                    }
                    player.EndTransformations();
                }
                else
                {
                    modPlayer.AddKi((projectedKiDrain) * -1, false, true);
                    Lighting.AddLight(player.Center, 1f, 1f, 0f);
                }
            }
            //else
            //{
            //    // the player isn't in a ki draining state anymore, reset KiDrainAddition
            //    modPlayer.kiDrainAddition = 0;                
            //}
            
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
            modPlayer.kiDamage *= damageMulti;

            // cross mod support stuff
            if (DBZMOD.Instance.thoriumLoaded)
            {
                ThoriumEffects(player);
            }
            if (DBZMOD.Instance.enigmaLoaded)
            {
                EnigmaEffects(player);
            }
            if (DBZMOD.Instance.battlerodsLoaded)
            {
                BattleRodEffects(player);
            }
            if (DBZMOD.Instance.expandedSentriesLoaded)
            {
                ExpandedSentriesEffects(player);
            }
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan");

            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public float GetModifiedSpeedMultiplier(MyPlayer modPlayer)
        {
            return 1f + ((speedMulti - 1f) * modPlayer.bonusSpeedMultiplier);
        }

        public float GetHalvedDamageBonus()
        {
            return 1f + ((damageMulti - 1f) * 0.5f);
        }

        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= GetHalvedDamageBonus();
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= GetHalvedDamageBonus();
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
            MyPlayer.ModPlayer(player).kiDrainMulti = kiDrainBuffMulti;
        }

        public string GetPercentForDisplay(string currentDisplayString, string text, int percent)
        {
            if (percent == 0)
                return currentDisplayString;
            return string.Format("{0}{1} {2}{3}%", currentDisplayString, text, percent > 0 ? "+" : string.Empty, percent);
        }

        public int kaiokenLevel = 0;

        public string AssembleTransBuffDescription()
        {
            string kaiokenName = string.Empty;
            if (Type == DBZMOD.Instance.TransformationDefinitionManager.KaiokenDefinition.GetBuffId() || Type == DBZMOD.Instance.TransformationDefinitionManager.SuperKaiokenDefinition.GetBuffId())
            {
                switch (kaiokenLevel)
                {
                    case 2:
                        kaiokenName = "(x3)\n";
                        break;
                    case 3:
                        kaiokenName = "(x4)\n";
                        break;
                    case 4:
                        kaiokenName = "(x10)\n";
                        break;
                    case 5:
                        kaiokenName = "(x20)\n";
                        break;
                }
            }
            int percentDamageMult = (int)Math.Round(damageMulti * 100f, 0) - 100;
            int percentSpeedMult = (int)Math.Round(speedMulti * 100f, 0) - 100;
            float kiDrainPerSecond = 60f * kiDrainRate;
            float kiDrainPerSecondWithMastery = 60f * kiDrainRateWithMastery;
            int percentKiDrainMulti = (int)Math.Round(kiDrainBuffMulti * 100f, 0) - 100;
            string displayString = kaiokenName;
            displayString = GetPercentForDisplay(displayString, "Damage", percentDamageMult);
            displayString = GetPercentForDisplay(displayString, " Speed", percentSpeedMult);
            displayString = GetPercentForDisplay(displayString, "\nKi Costs", percentKiDrainMulti);
            if (kiDrainPerSecond > 0)
            {
                displayString = $"{displayString}\nKi Drain: {(int)Math.Round(kiDrainPerSecond, 0)}/s";
                if (kiDrainPerSecondWithMastery > 0)
                {
                    displayString = $"{displayString}, {(int) Math.Round(kiDrainPerSecondWithMastery, 0)}/s when mastered";
                }
            }
            if (healthDrainRate > 0)
            {
                displayString = $"{displayString}\nLife Drain: -{healthDrainRate / 2}/s.";
            }
            return displayString;
        }

        public static int GetTotalHealthDrain(Player player)
        {
            var healthDrain = KaiokenBuff.GetHealthDrain(player.GetModPlayer<MyPlayer>()) + SuperKaiokenBuff.GetHealthDrain(player);
            return healthDrain;
        }

        public TransformationDefinition TransformationDefinition { get; }
    }
}

