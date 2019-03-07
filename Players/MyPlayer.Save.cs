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
            if (PlayerTransformations == null)
                PlayerTransformations = new Dictionary<TransformationDefinition, PlayerTransformation>();

            TagCompound tag = new TagCompound();

            if (SaveVersion == null)
                SaveVersion = latestVersion;

            tag.Add(nameof(SaveVersion), SaveVersion.ToString());

            foreach (PlayerTransformation playerTransformation in PlayerTransformations.Values)
            {
                playerTransformation.TransformationDefinition.OnPlayerSave(this, tag);
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

            // changed save routine to save to a float, orphaning the original KiCurrent.
            tag.Add("KiCurrentFloat", _kiCurrent);
            tag.Add("RageCurrent", rageCurrent);
            tag.Add("KiRegenRate", kiChargeRate);
            tag.Add("KiEssence1", kiEssence1);
            tag.Add("KiEssence2", kiEssence2);
            tag.Add("KiEssence3", kiEssence3);
            tag.Add("KiEssence4", kiEssence4);
            tag.Add("KiEssence5", kiEssence5);

            tag.Add(nameof(SelectedTransformation), 
                SelectedTransformation == null ? "" : SelectedTransformation.UnlocalizedName);

            tag.Add("IsMasteryRetrofitted", isMasteryRetrofitted);

            foreach (KeyValuePair<TransformationDefinition, PlayerTransformation> kvp in PlayerTransformations)
            {
                tag.Add($"MasteryLevel{kvp.Key.UnlocalizedName}", kvp.Value.Mastery);
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
            tag.Add("HairChecked", hairChecked);

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
            ActiveTransformations = new List<TransformationDefinition>();
            PlayerTransformations = new Dictionary<TransformationDefinition, PlayerTransformation>();

            if (tag.ContainsKey(nameof(SaveVersion)))
            {
                SaveVersion = new Version(tag.GetString(nameof(SaveVersion)));

                if (SaveVersion >= new Version(1, 0, 0, 0))
                {
                    for (int i = 0; i < DBZMOD.Instance.TransformationDefinitionManager.Count; i++)
                    {
                        TransformationDefinition transformationDefinition = DBZMOD.Instance.TransformationDefinitionManager[i];

                        // TODO Make sure this is OK (not losing any transformations)
                        if (tag.ContainsKey(transformationDefinition.GetUnlockedTagCompoundKey()) && !PlayerTransformations.ContainsKey(transformationDefinition))
                            PlayerTransformations.Add(transformationDefinition, new PlayerTransformation(transformationDefinition, tag.Get<float>(transformationDefinition.GetMasteryTagCompoundKey())));

                        transformationDefinition.OnPlayerLoad(this, tag);
                    }
                }
            }
            else
            {
                SaveVersion = latestVersion;

                // TODO Add mastery check.
                if (tag.Get<bool>("SSJ1Achieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.SSJ1Definition))
                    PlayerTransformations.Add(TransformationDefinitionManager.SSJ1Definition, new PlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition));
                if (tag.Get<bool>("ASSJAchieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.ASSJDefinition))
                    PlayerTransformations.Add(TransformationDefinitionManager.ASSJDefinition, new PlayerTransformation(TransformationDefinitionManager.ASSJDefinition));
                if (tag.Get<bool>("USSJAchieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.USSJDefinition))
                    PlayerTransformations.Add(TransformationDefinitionManager.USSJDefinition, new PlayerTransformation(TransformationDefinitionManager.USSJDefinition));

                if (tag.Get<bool>("SSJ2Achieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.SSJ2Definition))
                    PlayerTransformations.Add(TransformationDefinitionManager.SSJ2Definition, new PlayerTransformation(TransformationDefinitionManager.SSJ2Definition));
                if (tag.Get<bool>("SSJ3Achieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.SSJ3Definition))
                    PlayerTransformations.Add(TransformationDefinitionManager.SSJ3Definition, new PlayerTransformation(TransformationDefinitionManager.SSJ3Definition));

                if (tag.Get<bool>("LSSJAchieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.LSSJDefinition))
                    PlayerTransformations.Add(TransformationDefinitionManager.LSSJDefinition, new PlayerTransformation(TransformationDefinitionManager.LSSJDefinition));

                if (tag.Get<bool>("ssjgAchieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.SSJGDefinition))
                    PlayerTransformations.Add(TransformationDefinitionManager.SSJGDefinition, new PlayerTransformation(TransformationDefinitionManager.SSJGDefinition));

                if (tag.Get<bool>("KaioAchieved") && !PlayerTransformations.ContainsKey(TransformationDefinitionManager.KaiokenDefinition))
                    PlayerTransformations.Add(TransformationDefinitionManager.KaiokenDefinition, new PlayerTransformation(TransformationDefinitionManager.KaiokenDefinition));

                isMasteryRetrofitted = tag.ContainsKey("IsMasteryRetrofitted") && tag.Get<bool>("IsMasteryRetrofitted");
                if (!isMasteryRetrofitted)
                {
                    // TODO Endure this
                    // Retroactive mastery ported to new mastery system.
                    float masteryLevel1 = tag.Get<float>("MasteryLevel1");
                    PlayerTransformations[TransformationDefinitionManager.SSJ1Definition].Mastery = masteryLevel1;

                    float masteryLevel2 = tag.Get<float>("MasteryLevel2");
                    PlayerTransformations[TransformationDefinitionManager.SSJ2Definition].Mastery = masteryLevel2;

                    float masteryLevel3 = tag.Get<float>("MasteryLevel3");
                    PlayerTransformations[TransformationDefinitionManager.SSJ3Definition].Mastery = masteryLevel3;

                    float masteryLevelGod = tag.Get<float>("MasteryLevelGod");
                    PlayerTransformations[TransformationDefinitionManager.SSJGDefinition].Mastery = masteryLevelGod;

                    float masteryLevelBlue = tag.Get<float>("MasteryLevelBlue");
                    PlayerTransformations[TransformationDefinitionManager.SSJGDefinition].Mastery = masteryLevelBlue;

                    bool masteredMessage1 = tag.Get<bool>("MasteredMessage1");
                    masteryMessagesDisplayed[TransformationDefinitionManager.SSJ1Definition] = masteredMessage1;

                    bool masteredMessage2 = tag.Get<bool>("MasteredMessage2");
                    masteryMessagesDisplayed[TransformationDefinitionManager.SSJ2Definition] = masteredMessage2;

                    bool masteredMessage3 = tag.Get<bool>("MasteredMessage3");
                    masteryMessagesDisplayed[TransformationDefinitionManager.SSJ3Definition] = masteredMessage3;

                    bool masteredMessageGod = tag.Get<bool>("MasteredMessageGod");
                    masteryMessagesDisplayed[TransformationDefinitionManager.SSJGDefinition] = masteredMessageGod;

                    /*var masteredMessageBlue = tag.Get<bool>("MasteredMessageBlue");
                    masteryMessagesDisplayed[DBZMOD.Instance.TransformationDefinitionManager.SSJBDefinition.UnlocalizedName] = masteredMessageBlue;*/

                    // TODO Change masteryLevels to use TransformationDefinition.
                    // Prime the dictionary with any missing entries.
                    for (int i = 0; i < DBZMOD.Instance.TransformationDefinitionManager.Count; i++)
                    {
                        TransformationDefinition transformation = DBZMOD.Instance.TransformationDefinitionManager[i];
                        if (transformation == null || !transformation.HasMastery) continue;

                        if (!PlayerTransformations.ContainsKey(transformation))
                        {
                            PlayerTransformations.Add(transformation, new PlayerTransformation(transformation));
                            PlayerTransformations[transformation].Mastery = 0f;
                        }

                        if (!masteryMessagesDisplayed.ContainsKey(transformation))
                        {
                            masteryMessagesDisplayed[transformation] = false;
                        }
                    }
                }
                else
                {
                    // New mastery system/dynamic loading. Overwrites the old one if it exists.
                    for (int i = 0; i < DBZMOD.Instance.TransformationDefinitionManager.Count; i++)
                    {
                        TransformationDefinition transformation = DBZMOD.Instance.TransformationDefinitionManager[i];
                        if (transformation == null || !transformation.HasMastery) continue;

                        if (tag.ContainsKey($"MasteryLevel{transformation.UnlocalizedName}"))
                            PlayerTransformations[transformation].Mastery = tag.Get<float>($"MasteryLevel{transformation.UnlocalizedName}");
                        else
                            PlayerTransformations[transformation].Mastery = 0f;

                        if (tag.ContainsKey($"MasteryMessagesDisplayed{transformation.UnlocalizedName}"))
                        {
                            masteryMessagesDisplayed[transformation] = tag.Get<bool>($"MasteryMessagesDisplayed{transformation.UnlocalizedName}");
                        }
                        else
                        {
                            masteryMessagesDisplayed[transformation] = false;
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

            if (tag.ContainsKey(nameof(SelectedTransformation)) && !string.IsNullOrWhiteSpace(tag.Get<string>(nameof(SelectedTransformation))))
                SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager[tag.Get<string>(nameof(SelectedTransformation))];

            jungleMessage = tag.Get<bool>("JungleMessage");
            hellMessage = tag.Get<bool>("HellMessage");
            evilMessage = tag.Get<bool>("EvilMessage");
            mushroomMessage = tag.Get<bool>("MushroomMessage");

            string playerTrait = tag.Get<string>("playerTrait");
            if (string.IsNullOrWhiteSpace(playerTrait))
                PlayerTrait = TraitManager.Default;
            else if (!DBZMOD.Instance.TraitManager.Contains(playerTrait))
                PlayerTrait = TraitManager[char.ToLower(playerTrait[0]) + playerTrait.Substring(1)];
            else
                PlayerTrait = TraitManager[playerTrait];

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
            hairChecked = tag.Get<bool>("HairChecked");

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
