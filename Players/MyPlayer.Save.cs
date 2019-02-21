using System;
using System.Collections.Generic;
using System.Linq;
using DBZMOD.Enums;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace DBZMOD
{
    public partial class MyPlayer
    {    
        internal static readonly Version latestVersion = new Version(1, 0, 0, 0);

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();

            if (SaveVersion == null)
                SaveVersion = latestVersion;

            tag.Add(nameof(SaveVersion), SaveVersion.ToString());

            if (PlayerTransformations != null)
                foreach (PlayerTransformation playerTransformation in PlayerTransformations.Values)
                {
                    tag.Add(playerTransformation.TransformationDefinition.GetUnlockedTagCompoundKey(), true);
                    tag.Add(playerTransformation.TransformationDefinition.GetMasteryTagCompoundKey(), playerTransformation.Mastery);
                }

            tag.Add("Fragment1", fragment1);
            tag.Add("Fragment2", fragment2);
            tag.Add("Fragment3", fragment3);
            tag.Add("Fragment4", fragment4);
            tag.Add("Fragment5", fragment5);
            tag.Add("KaioFragment1", kaioFragment1);
            tag.Add("KaioFragment2", kaioFragment2);
            tag.Add("KaioFragment3", kaioFragment3);
            tag.Add("KaioFragment4", kaioFragment4);
            tag.Add("KaioAchieved", kaioAchieved);

            // changed save routine to save to a float, orphaning the original KiCurrent.
            tag.Add("KiCurrentFloat", _kiCurrent);
            tag.Add("RageCurrent", rageCurrent);
            tag.Add("KiRegenRate", kiChargeRate);
            tag.Add("KiEssence1", kiEssence1);
            tag.Add("KiEssence2", kiEssence2);
            tag.Add("KiEssence3", kiEssence3);
            tag.Add("KiEssence4", kiEssence4);
            tag.Add("KiEssence5", kiEssence5);
            tag.Add(nameof(UI.TransformationMenu.SelectedTransformation), UI.TransformationMenu.SelectedTransformation.UnlocalizedName);
            tag.Add("IsMasteryRetrofitted", isMasteryRetrofitted);

            foreach (var key in masteryLevels.Keys)
            {
                tag.Add($"MasteryLevel{key}", masteryLevels[key]);
            }

            foreach (var key in masteryMessagesDisplayed.Keys)
            {
                tag.Add($"MasteryMessagesDisplayed{key}", masteryMessagesDisplayed[key]);
            }

            tag.Add("JungleMessage", jungleMessage);
            tag.Add("HellMessage", hellMessage);
            tag.Add("EvilMessage", evilMessage);
            tag.Add("MushroomMessage", mushroomMessage);
            tag.Add("playerTrait", PlayerTrait == null ? DBZMOD.Instance.TraitManager.GetRandomTrait().UnlocalizedName : PlayerTrait.UnlocalizedName);
            tag.Add("flightUnlocked", flightUnlocked);
            tag.Add("flightDampeningUnlocked", flightDampeningUnlocked);
            tag.Add("flightUpgraded", flightUpgraded);
            
            tag.Add("KiMax3", kiMax3);
            tag.Add("FirstFourStarDBPickup", firstDragonBallPickup);
            tag.Add("PowerWishesLeft", powerWishesLeft);
            tag.Add("SkillWishesLeft", skillWishesLeft);
            tag.Add("ImmortalityWishesLeft", immortalityWishesLeft);
            tag.Add("AwakeningWishesLeft", awakeningWishesLeft);
            tag.Add("ImmortalityRevivesLeft", immortalityRevivesLeft);
            tag.Add("IsInstantTransmission1Unlocked", isInstantTransmission1Unlocked);
            tag.Add("IsInstantTransmission2Unlocked", isInstantTransmission2Unlocked);
            tag.Add("IsInstantTransmission3Unlocked", isInstantTransmission3Unlocked);
            // added to store the player's original eye color if possible
            if (originalEyeColor != null)
            {
                tag.Add("OriginalEyeColorR", originalEyeColor.Value.R);
                tag.Add("OriginalEyeColorG", originalEyeColor.Value.G);
                tag.Add("OriginalEyeColorB", originalEyeColor.Value.B);
            }

            //tag.Add("RealismMode", RealismMode);
            return tag;
        }

        public bool isMasteryRetrofitted;

        public override void Load(TagCompound tag)
        {
            PlayerTransformations = new Dictionary<TransformationDefinition, PlayerTransformation>(DBZMOD.Instance.TransformationDefinitionManager.Count, new UnlocalizedNameComparer());

            if (tag.ContainsKey(nameof(SaveVersion)))
            {
                SaveVersion = new Version(tag.GetString(nameof(SaveVersion)));

                if (SaveVersion >= new Version(1, 0, 0, 0))
                {
                    for (int i = 0; i < DBZMOD.Instance.TransformationDefinitionManager.Count; i++)
                    {
                        TransformationDefinition transformationDefinition = DBZMOD.Instance.TransformationDefinitionManager[i];

                        if (tag.ContainsKey(transformationDefinition.GetUnlockedTagCompoundKey()))
                            PlayerTransformations.Add(transformationDefinition, new PlayerTransformation(transformationDefinition, tag.Get<float>(transformationDefinition.GetMasteryTagCompoundKey())));
                    }
                }
            }
            else
            {
                SaveVersion = latestVersion;

                // TODO Add mastery check.
                if (tag.Get<bool>("SSJ1Achieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition));
                if (tag.Get<bool>("ASSJAchieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.ASSJDefinition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.ASSJDefinition));
                if (tag.Get<bool>("USSJAchieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition));

                if (tag.Get<bool>("SSJ2Achieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition));
                if (tag.Get<bool>("SSJ3Achieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition));

                if (tag.Get<bool>("LSSJAchieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition));
                if (tag.Get<bool>("LSSJ2Achieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.LSSJ2Definition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.LSSJ2Definition));

                if (tag.Get<bool>("ssjgAchieved")) PlayerTransformations.Add(DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition));

                isMasteryRetrofitted = tag.ContainsKey("IsMasteryRetrofitted") && tag.Get<bool>("IsMasteryRetrofitted");
                if (!isMasteryRetrofitted)
                {
                    // Retroactive mastery ported to new mastery system.
                    var masteryLevel1 = tag.Get<float>("MasteryLevel1");
                    masteryLevels[DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition.UnlocalizedName] = masteryLevel1;
                    var masteryLevel2 = tag.Get<float>("MasteryLevel2");
                    masteryLevels[DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition.UnlocalizedName] = masteryLevel2;
                    var masteryLevel3 = tag.Get<float>("MasteryLevel3");
                    masteryLevels[DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition.UnlocalizedName] = masteryLevel3;
                    var masteryLevelGod = tag.Get<float>("MasteryLevelGod");
                    masteryLevels[DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition.UnlocalizedName] = masteryLevelGod;
                    var masteryLevelBlue = tag.Get<float>("MasteryLevelBlue");
                    masteryLevels[DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition.UnlocalizedName] = masteryLevelBlue;
                    var masteredMessage1 = tag.Get<bool>("MasteredMessage1");
                    masteryMessagesDisplayed[DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition.UnlocalizedName] = masteredMessage1;
                    var masteredMessage2 = tag.Get<bool>("MasteredMessage2");
                    masteryMessagesDisplayed[DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition.UnlocalizedName] = masteredMessage2;
                    var masteredMessage3 = tag.Get<bool>("MasteredMessage3");
                    masteryMessagesDisplayed[DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition.UnlocalizedName] = masteredMessage3;
                    var masteredMessageGod = tag.Get<bool>("MasteredMessageGod");
                    masteryMessagesDisplayed[DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition.UnlocalizedName] = masteredMessageGod;
                    var masteredMessageBlue = tag.Get<bool>("MasteredMessageBlue");
                    masteryMessagesDisplayed[DBZMOD.Instance.TransformationDefinitionManager.SSJBDefinition.UnlocalizedName] = masteredMessageBlue;

                    // Prime the dictionary with any missing entries.
                    foreach (var key in FormBuffHelper.AllBuffs().Where(x => x.HasMastery)
                        .Select(x => x.MasteryBuffKeyName).Distinct())
                    {
                        if (!masteryLevels.ContainsKey(key))
                        {
                            masteryLevels[key] = 0f;
                        }

                        if (!masteryMessagesDisplayed.ContainsKey(key))
                        {
                            masteryMessagesDisplayed[key] = false;
                        }
                    }
                }
                else
                {
                    // New mastery system/dynamic loading. Overwrites the old one if it exists.
                    foreach (var key in FormBuffHelper.AllBuffs().Where(x => x.HasMastery)
                        .Select(x => x.MasteryBuffKeyName).Distinct())
                    {
                        if (tag.ContainsKey($"MasteryLevel{key}"))
                        {
                            masteryLevels[key] = tag.Get<float>($"MasteryLevel{key}");
                        }
                        else
                        {
                            masteryLevels[key] = 0f;
                        }

                        if (tag.ContainsKey($"MasteryMessagesDisplayed{key}"))
                        {
                            masteryMessagesDisplayed[key] = tag.Get<bool>($"MasteryMessagesDisplayed{key}");
                        }
                        else
                        {
                            masteryMessagesDisplayed[key] = false;
                        }
                    }
                }
            }

            fragment1 = tag.Get<bool>("Fragment1");
            fragment2 = tag.Get<bool>("Fragment2");
            fragment3 = tag.Get<bool>("Fragment3");
            fragment4 = tag.Get<bool>("Fragment4");
            fragment5 = tag.Get<bool>("Fragment5");

            kaioFragment1 = tag.Get<bool>("KaioFragment1");
            kaioFragment2 = tag.Get<bool>("KaioFragment2");
            kaioFragment3 = tag.Get<bool>("KaioFragment3");
            kaioFragment4 = tag.Get<bool>("KaioFragment4");
            kaioAchieved = tag.Get<bool>("KaioAchieved");

            if (tag.ContainsKey("KiCurrentFloat"))
            {
                _kiCurrent = tag.Get<float>("KiCurrentFloat");
            }
            else
            {
                _kiCurrent = (float)tag.Get<int>("KiCurrent");
            }

            rageCurrent = tag.Get<int>("RageCurrent");
            kiChargeRate = tag.Get<int>("KiRegenRate");
            kiEssence1 = tag.Get<bool>("KiEssence1");
            kiEssence2 = tag.Get<bool>("KiEssence2");
            kiEssence3 = tag.Get<bool>("KiEssence3");
            kiEssence4 = tag.Get<bool>("KiEssence4");
            kiEssence5 = tag.Get<bool>("KiEssence5");

            if (tag.ContainsKey(nameof(UI.TransformationMenu.SelectedTransformation)))
                UI.TransformationMenu.SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager[tag.Get<string>(nameof(UI.TransformationMenu.SelectedTransformation))];

            jungleMessage = tag.Get<bool>("JungleMessage");
            hellMessage = tag.Get<bool>("HellMessage");
            evilMessage = tag.Get<bool>("EvilMessage");
            mushroomMessage = tag.Get<bool>("MushroomMessage");

            string playerTrait = tag.Get<string>("playerTrait");
            if (!DBZMOD.Instance.TraitManager.Contains(playerTrait))
                PlayerTrait = DBZMOD.Instance.TraitManager[char.ToLower(playerTrait[0]) + playerTrait.Substring(1)];
            else
                PlayerTrait = DBZMOD.Instance.TraitManager[playerTrait];

            flightUnlocked = tag.Get<bool>("flightUnlocked");
            flightDampeningUnlocked = tag.Get<bool>("flightDampeningUnlocked");
            flightUpgraded = tag.Get<bool>("flightUpgraded");
            kiMax3 = tag.Get<int>("KiMax3");
            firstDragonBallPickup = tag.Get<bool>("FirstFourStarDBPickup");
            powerWishesLeft = tag.ContainsKey("PowerWishesLeft") ? tag.Get<int>("PowerWishesLeft") : 5;

            // during debug, I wanted power wishes to rest so I can figure out if the damage mults work :(
            if (DebugHelper.IsDebugModeOn())
            {
                powerWishesLeft = POWER_WISH_MAXIMUM;
            }

            skillWishesLeft = tag.ContainsKey("SkillWishesLeft") ? tag.Get<int>("SkillWishesLeft") : 3;
            immortalityWishesLeft = tag.ContainsKey("ImmortalityWishesLeft") ? tag.Get<int>("ImmortalityWishesLeft") : 1;
            awakeningWishesLeft = tag.ContainsKey("AwakeningWishesLeft") ? tag.Get<int>("AwakeningWishesLeft") : 3;
            immortalityRevivesLeft = tag.ContainsKey("ImmortalityRevivesLeft") ? tag.Get<int>("ImmortalityRevivesLeft") : 0;
            isInstantTransmission1Unlocked = tag.ContainsKey("IsInstantTransmission1Unlocked") ? tag.Get<bool>("IsInstantTransmission1Unlocked") : false;
            isInstantTransmission2Unlocked = tag.ContainsKey("IsInstantTransmission2Unlocked") ? tag.Get<bool>("IsInstantTransmission2Unlocked") : false;
            isInstantTransmission3Unlocked = tag.ContainsKey("IsInstantTransmission3Unlocked") ? tag.Get<bool>("IsInstantTransmission3Unlocked") : false;
            // load the player's original eye color if possible
            if (tag.ContainsKey("OriginalEyeColorR") && tag.ContainsKey("OriginalEyeColorG") && tag.ContainsKey("OriginalEyeColorB"))
            {
                originalEyeColor = new Color(tag.Get<byte>("OriginalEyeColorR"), tag.Get<byte>("OriginalEyeColorG"), tag.Get<byte>("OriginalEyeColorB"));
            }

            //RealismMode = tag.Get<bool>("RealismMode");
        }

        public Version SaveVersion { get; private set; }
    }
}
