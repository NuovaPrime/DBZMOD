using System;
using DBZMOD.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Transformations
{
    public abstract class TransformationBuff : ModBuff
    {
        private const int MINIMUM_TRANSFORMATION_FRAMES_FOR_KI_COST_UPKEEP = 1;
        private int _kiDrainAddTimer;
        public bool realismModeOn;
        public int precentDefenceBonus;

        protected TransformationBuff(TransformationDefinition transformationDefinition)
        {
            TransformationDefinition = transformationDefinition;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI != Main.LocalPlayer.whoAmI) return;

            MyPlayer modPlayer = MyPlayer.ModPlayer(player);

            modPlayer.player.lifeRegen -= (int) TransformationDefinition.GetHealthDrainRate(modPlayer);
            modPlayer.kiDrainMulti = TransformationDefinition.GetKiSkillDrainMultiplier(modPlayer);

            Vector2 lightPosition = player.Center + player.velocity * 8f;
            float lightingR = 0f, lightingB = 0f, lightingG = 0f;

            TransformationDefinition.GetPlayerLightModifier(modPlayer, ref lightingR, ref lightingG, ref lightingB);

            if (lightingR > 0f || lightingG > 0f || lightingB > 0f)
                Lighting.AddLight(lightPosition, lightingR, lightingG, lightingB);

            // Give bonus base defense
            player.statDefense += TransformationDefinition.GetDefenseBonus(player.GetModPlayer<MyPlayer>());
            
            // handle ki drain mastery
            bool isMastered = modPlayer.PlayerTransformations.ContainsKey(TransformationDefinition) &&
                              modPlayer.PlayerTransformations[TransformationDefinition].Mastery >= 1.0;

            float actualKiDrain = isMastered ? TransformationDefinition.GetKiDrainRateMastery(modPlayer) : TransformationDefinition.GetKiDrainRate(modPlayer);

            float kiDrainMultiplier = 1f;
            if (!isMastered)
            {
                if (kiDrainMultiplier < 3f)
                {
                    _kiDrainAddTimer++;
                    if (_kiDrainAddTimer >= 300)
                    {
                        kiDrainMultiplier += 0.5f;
                        _kiDrainAddTimer = 0;
                    }
                }
            }
            /*else // Mastered forms do not drain more over time.
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
            }*/

            float projectedKiDrain = actualKiDrain * kiDrainMultiplier;

            TransformationDefinition transformation = modPlayer.GetCurrentTransformation();

            // if the player is in any ki-draining state, handles ki drain and power down when ki is depleted
            if (!DBZMOD.Instance.TransformationDefinitionManager.IsKaioken(transformation))
            {
                // player can't support ten frames (arbitrary) of their current form, make them fall out of any forms they might be in.
                if (modPlayer.IsKiDepleted(projectedKiDrain * MINIMUM_TRANSFORMATION_FRAMES_FOR_KI_COST_UPKEEP))
                {
                    if (transformation == DBZMOD.Instance.TransformationDefinitionManager.SuperKaiokenDefinition)
                        modPlayer.kaiokenLevel = 0;

                    modPlayer.EndTransformations();
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

            float speedMultiplier = TransformationDefinition.GetSpeedMultiplier(modPlayer);

            player.moveSpeed *= speedMultiplier;
            player.maxRunSpeed *= speedMultiplier;
            player.runAcceleration *= speedMultiplier;

            if (player.jumpSpeedBoost < 1f)
                player.jumpSpeedBoost = 1f;

            player.jumpSpeedBoost *= speedMultiplier;

            // Set player damage mults
            float
                damageMultiplier = TransformationDefinition.GetDamageMultiplier(modPlayer),
                halvedDamageMultiplier = GetHalvedDamageBonus(damageMultiplier);

            player.meleeDamage *= halvedDamageMultiplier;
            player.rangedDamage *= halvedDamageMultiplier;
            player.magicDamage *= halvedDamageMultiplier;
            player.minionDamage *= halvedDamageMultiplier;
            player.thrownDamage *= halvedDamageMultiplier;
            modPlayer.kiDamage *= damageMultiplier;

            // cross mod support stuff
            if (DBZMOD.Instance.thoriumLoaded)
            {
                ThoriumEffects(player, damageMultiplier);
            }
            if (DBZMOD.Instance.enigmaLoaded)
            {
                EnigmaEffects(player, damageMultiplier);
            }
            if (DBZMOD.Instance.battlerodsLoaded)
            {
                BattleRodEffects(player, damageMultiplier);
            }
            if (DBZMOD.Instance.expandedSentriesLoaded)
            {
                ExpandedSentriesEffects(player, damageMultiplier);
            }

            if (player.buffTime[buffIndex] == 0)
                TransformationDefinition.OnTransformationBuffExpired(modPlayer, ref buffIndex);
        }

        public override void SetDefaults()
        {
            if (TransformationDefinition != null)
            {
                DisplayName.SetDefault(TransformationDefinition.Text);

                Main.buffNoTimeDisplay[Type] = true;
                Main.buffNoSave[Type] = true;
                Main.debuff[Type] = true;

                // TODO Make this dynamics
                Description.SetDefault("Buff Text");
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = AssembleTransBuffDescription(Main.player[Main.myPlayer].GetModPlayer<MyPlayer>()) + (TransformationDefinition.ExtraTooltipText != null ? "\n" + TransformationDefinition.ExtraTooltipText : "");
        }

        public float GetModifiedSpeedMultiplier(MyPlayer modPlayer, float speedMultiplier)
        {
            return 1f + (speedMultiplier - 1f) * modPlayer.bonusSpeedMultiplier;
        }

        public float GetHalvedDamageBonus(float damageMultiplier) => 1f + (damageMultiplier - 1f) * 0.5f;

        #region Mod Support

        public void ThoriumEffects(Player player, float damageMultiplier)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= damageMultiplier;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= damageMultiplier;
        }

        public void EnigmaEffects(Player player, float damageMultiplier)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).MysticDamage *= damageMultiplier;
        }

        public void BattleRodEffects(Player player, float damageMultiplier)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage *= damageMultiplier;
        }

        public void ExpandedSentriesEffects(Player player, float damageMultiplier)
        {
            player.GetModPlayer<ExpandedSentries.ESPlayer>(ModLoader.GetMod("ExpandedSentries")).sentryDamage *= damageMultiplier;
        }

        #endregion

        public string GetPercentForDisplay(string currentDisplayString, string text, float percent)
        {
            if (percent == 0f)
                return currentDisplayString;

            return string.Format("{0}{1} {2}{3:F}%", currentDisplayString, text, percent > 0 ? "+" : string.Empty, percent);
        }

        public virtual string AssembleTransBuffDescription(MyPlayer player)
        {
            string kaiokenName = string.Empty;

            if (Type == DBZMOD.Instance.TransformationDefinitionManager.KaiokenDefinition.GetBuffId() || Type == DBZMOD.Instance.TransformationDefinitionManager.SuperKaiokenDefinition.GetBuffId())
            {
                switch (player.kaiokenLevel)
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
                    default: break;
                }
            }
            
            float percentDamageMult =TransformationDefinition.GetDamageMultiplier(player) * 100f - 100;
            float percentSpeedMult = TransformationDefinition.GetSpeedMultiplier(player) * 100f - 100;

            float kiDrainPerSecond = 60f * TransformationDefinition.GetKiDrainRate(player);
            float kiDrainPerSecondWithMastery = 60f * TransformationDefinition.GetKiDrainRateMastery(player);
            int percentKiDrainMulti = (int)Math.Round(TransformationDefinition.GetKiSkillDrainMultiplier(player) * 100f, 0) - 100;

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

            float healthDrainRate = TransformationDefinition.GetHealthDrainRate(player);
            if (healthDrainRate > 0)
            {
                displayString = $"{displayString}\nLife Drain: -{healthDrainRate / 2}/s.";
            }

            return displayString;
        }

        public TransformationDefinition TransformationDefinition { get; }
    }
}

