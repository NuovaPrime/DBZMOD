using System;
using System.Collections.Generic;
using DBZMOD.Buffs;
using DBZMOD.Config;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using DBZMOD.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;
using DBZMOD.Util;
using DBZMOD.Models;
using DBZMOD.Enums;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Network;
using DBZMOD.Projectiles;

namespace DBZMOD
{
    public class MyPlayer : ModPlayer
    {
        #region Variables
        //Player vars
        // regression safe property accessor for Ki Damage until web gets off his buttocks
        // intended to fix compatibility with AllDamage and Leveled, temporarily.
        public float KiDamage;

        public float kiDamage
        {
            get { return KiDamage; }
            set { KiDamage = value; }
        }

        public float kiKbAddition;
        public float kiSpeedAddition;
        public int kiCrit;
        public int kiRegenTimer;
        public int kiRegen;
        public int kaiokenLevel = 0;
        public float kiDamageMulti = 1f;

        // kiMax is now a property that gets reset when it's accessed and less than or equal to zero, to retro fix nasty bugs
        // there's no point changing this value as it only resets itself if it doesn't line up with fragment ki max.

        public int KiMax()
        {
            return GetKiMaxFromFragments();
        }

        // ki max 2 is ki max from equipment and accessories. there's no point changing this value as it gets reset each frame.
        public int kiMax2;

        // progression upgrades increase KiMax3. This is the only value that can be changed to have an impact on ki max and does not reset.
        public int kiMax3;

        // ki max mult is a multiplier for ki that stacks multiplicatively with other KiMaxMult bonuses. It resets to 1f each frame.
        public float kiMaxMult = 1f;

        // made KiCurrent private forcing everyone to use a method that syncs to clients, centralizing ki increase/decrease logic.
        // 12/24/2018 changed Ki current to a float so that you could do more elaborate ki draining things and ki drain rates can be more consistent.
        private float _kiCurrent;

        public int kiChargeRate = 1;
        public int overloadMax = 100;
        public int overloadCurrent;
        public int overloadTimer;
        public float chargeMoveSpeed;

        //Transformation vars
        public bool isTransforming;
        public int ssjAuraBeamTimer;
        public bool hasSSJ1;
        public int transformCooldown;
        public bool assjAchieved;
        public bool ussjAchieved;
        public bool ssj2Achieved;
        public bool ssj3Achieved;
        public bool lssjAchieved = false;
        public bool ssjgAchieved = false;
        public int lssj2Timer;
        public bool lssj2Achieved = false;
        public bool lssjgAchieved = false;
        public int rageCurrent = 0;
        public int rageDecreaseTimer = 0;
        public int formUnlockChance;
        public int overallFormUnlockChance;
        // public BuffInfo[] CurrentTransformations = new BuffInfo[2];

        //Input vars
        public static ModHotKey kaiokenKey;
        public static ModHotKey energyCharge;
        public static ModHotKey transform;
        public static ModHotKey powerDown;
        public static ModHotKey speedToggle;
        public static ModHotKey quickKi;
        public static ModHotKey transMenu;
        public static ModHotKey instantTransmission;
        //public static ModHotKey ProgressionMenuKey;
        public static ModHotKey flyToggle;
        public static ModHotKey armorBonus;

        //mastery vars
        public float masteryLevel1 = 0;
        public bool masteredMessage1 = false;
        public float masteryLevel2 = 0;
        public bool masteredMessage2 = false;
        public float masteryLevel3 = 0;
        public bool masteredMessage3 = false;
        public float masteryLevelGod = 0;
        public bool masteredMessageGod = false;
        public float masteryLevelBlue = 0;
        public bool masteredMessageBlue = false;
        public float masteryMaxFlight = 1;
        public float masteryLevelFlight = 0;
        public int masteryTimer = 0;

        //Wish vars
        public const int POWER_WISH_MAXIMUM = 5;
        public int powerWishesLeft = 5;
        public int immortalityWishesLeft = 1;
        public int skillWishesLeft = 3;
        public int awakeningWishesLeft = 3;
        public int immortalityRevivesLeft = 0;

        //unsorted vars
        public int drawX;
        public int drawY;
        public bool ssj1Achieved;
        public bool scouterT2;
        public bool scouterT3;
        public bool scouterT4;
        public bool scouterT5;
        public bool scouterT6;
        public bool fragment1;
        public bool fragment2;
        public bool fragment3;
        public bool fragment4;
        public bool fragment5;
        public bool kaioFragment1;
        public bool kaioFragment2;
        public bool kaioFragment3;
        public bool kaioFragment4;
        public bool chlorophyteHeadPieceActive;
        public bool kaioAchieved;
        public bool kiEssence1;
        public bool kiEssence2;
        public bool kiEssence3;
        public bool turtleShell;
        public bool kiEssence4;
        public bool kiEssence5;
        public bool demonBonusActive;
        public bool spiritualEmblem;
        public float kaiokenTimer = 0.0f;
        public bool kiLantern;
        public float bonusSpeedMultiplier = 1f;
        public float kiDrainMulti;
        public bool diamondNecklace;
        public bool emeraldNecklace;
        public bool sapphireNecklace;
        public bool topazNecklace;
        public bool amberNecklace;
        public bool amethystNecklace;
        public bool rubyNecklace;
        public bool dragongemNecklace;
        public bool isCharging;
        // bool used internally to handle managing effects
        public bool wasCharging;
        public int auraSoundTimer;
        public int chargeLimitAdd;
        //public static bool RealismMode = false;
        public bool jungleMessage = false;
        public bool hellMessage = false;
        public bool evilMessage = false;
        public bool mushroomMessage = false;
        public int kiOrbDropChance;
        public bool isHoldingKiWeapon;
        public bool wornGloves;
        public bool senzuBag;
        public bool palladiumBonus;
        public bool adamantiteBonus;
        public bool traitChecked = false;
        public string playerTrait = "";
        public bool demonBonus;
        public int orbGrabRange;
        public int orbHealAmount;
        public bool isFlying;

        public int flightUsageAdd;
        public float flightSpeedAdd;
        public bool earthenSigil;
        public bool earthenScarab;
        public bool radiantTotem;
        public int scarabChargeRateAdd;
        public int scarabChargeTimer;
        public bool flightUnlocked = false;
        public bool flightDampeningUnlocked = false;
        public bool flightUpgraded = false;
        public int demonBonusTimer;
        public bool hermitBonus;
        public bool spiritCharm;
        public bool zenkaiCharm;
        public bool zenkaiCharmActive;
        public bool majinNucleus;
        public bool baldurEssentia;
        public bool kiChip;
        public bool radiantGlider;
        public bool earthenArcanium;
        public bool legendNecklace;
        public bool legendWaistcape;
        public bool armCannon;
        public bool battleKit;
        public bool radiantBonus;
        public bool crystalliteControl;
        public bool crystalliteFlow;
        public bool crystalliteAlleviate;
        public float chargeTimerMaxAdd;
        public int kiDrainAddition;
        public float kaiokenDrainMulti;
        public bool kaioCrystal;
        public bool luminousSectum;
        public bool infuserAmber;
        public bool infuserAmethyst;
        public bool infuserDiamond;
        public bool infuserEmerald;
        public bool infuserRainbow;
        public bool infuserRuby;
        public bool infuserSapphire;
        public bool infuserTopaz;
        public bool isDashing;
        public bool canUseHeavyHit;
        public bool canUseFlurry;
        public bool canUseZanzoken;
        public int blockState;
        public bool blackDiamondShell;
        public bool buldariumSigmite;
        public bool attunementBracers;
        public bool burningEnergyAmulet;
        public bool iceTalisman;
        public bool pureEnergyCirclet;
        public bool timeRing;
        public bool bloodstainedBandana;
        public bool goblinKiEnhancer;
        public bool mechanicalAmplifier;
        public bool metamoranSash;
        public bool blackFusionBonus;
        public bool eliteSaiyanBonus;
        public float blackFusionIncrease = 1f;
        public int blackFusionBonusTimer;
        public bool firstDragonBallPickup = false;
        public bool wishActive = false;
        public bool isHoldingDragonRadarMk1 = false;
        public bool isHoldingDragonRadarMk2 = false;
        public bool isHoldingDragonRadarMk3 = false;
        public bool isInstantTransmission1Unlocked = false;
        public bool isInstantTransmission2Unlocked = false;
        public bool isInstantTransmission3Unlocked = false;        
        public KeyValuePair<uint, SoundEffectInstance> auraSoundInfo;

        // helper int tracks which player my local player is playing audio for
        // useful for preventing the mod from playing too many sounds
        public int playerIndexWithLocalAudio = -1;

        // animation helper fields
        public int lightningFrameTimer;
        public int transformationFrameTimer;
        public bool isTransformationAnimationPlaying = false;

        // bools used to apply transformation debuffs appropriately
        public bool isKaioken;
        public bool wasKaioken;
        public bool isTransformed;
        public bool wasTransformed;

        public int mouseWorldOctant = -1;

        public bool isMassiveBlastCharging;
        // public bool isMassiveBlastInUse;

        // Aura tracking to play audio and stuff.
        public AuraAnimationInfo previousAura;
        public AuraAnimationInfo currentAura;
        #endregion

        #region Syncable Controls
        public bool isMouseRightHeld = false;
        public bool isMouseLeftHeld = false;
        public bool isLeftHeld = false;
        public bool isRightHeld = false;
        public bool isUpHeld = false;
        public bool isDownHeld = false;
        #endregion

        #region Classes
        ProgressionSystem _mProgressionSystem = new ProgressionSystem();
        FistSystem _mFistSystem = new FistSystem();
        #endregion
        
        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetworkHelper.playerSync.RequestServerSendKiBeaconInitialSync(256, Main.myPlayer);
                NetworkHelper.playerSync.RequestAllDragonBallLocations(256, Main.myPlayer);
            }
        }

        public override void PlayerConnect(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (player.whoAmI != Main.myPlayer)
                {
                    NetworkHelper.playerSync.SendPlayerInfoToPlayerFromOtherPlayer(player.whoAmI, Main.myPlayer);

                    NetworkHelper.playerSync.RequestPlayerSendTheirInfo(256, Main.myPlayer, player.whoAmI);
                }
            }
        }

        // overall ki max is now just a formula representing your total ki, after all bonuses are applied.
        public int OverallKiMax()
        {
            return (int)Math.Ceiling(KiMax() * kiMaxMult + kiMax2 + kiMax3);
        }

        public const float FORM_MASTERY_GAIN_PER_TICK = 0.0000058f;

        // all changes to Ki Current are now made through this method.
        public void AddKi(float kiAmount, bool isWeaponDrain, bool isFormDrain)
        {
            HandleKiDrainMasteryContribution(kiAmount, isWeaponDrain, isFormDrain);
            SetKi(_kiCurrent + kiAmount);
        }
        
        public void HandleKiDrainMasteryContribution(float kiAmount, bool isWeaponDrain, bool isFormDrain)
        {
            if (isFormDrain)
            {
                if (TransformationHelper.IsSSJ1(player) && masteryLevel1 < 1.0f)
                {
                    masteryLevel1 = GetMasteryIncreaseFromFormDrain(masteryLevel1);
                }
                if (TransformationHelper.IsAssj(player) && masteryLevel1 < 1.0f)
                {
                    masteryLevel1 = GetMasteryIncreaseFromFormDrain(masteryLevel1);
                }
                if (TransformationHelper.IsUssj(player) && masteryLevel1 < 1.0f)
                {
                    masteryLevel1 = GetMasteryIncreaseFromFormDrain(masteryLevel1);
                }
                if (TransformationHelper.IsSSJ2(player) && masteryLevel2 < 1.0f)
                {
                    masteryLevel2 = GetMasteryIncreaseFromFormDrain(masteryLevel2);
                }
                if (TransformationHelper.IsSSJ3(player) && masteryLevel3 < 1.0f)
                {
                    masteryLevel3 = GetMasteryIncreaseFromFormDrain(masteryLevel3);
                }
            }

            if (isWeaponDrain && kiAmount < 0)
            {
                if (TransformationHelper.IsSSJ1(player) && masteryLevel1 < 1.0f)
                {
                    masteryLevel1 = GetMasteryIncreaseFromWeaponDrain(masteryLevel1, kiAmount);
                }
                if (TransformationHelper.IsAssj(player) && masteryLevel1 < 1.0f)
                {
                    masteryLevel1 = GetMasteryIncreaseFromWeaponDrain(masteryLevel1, kiAmount);
                }
                if (TransformationHelper.IsUssj(player) && masteryLevel1 < 1.0f)
                {
                    masteryLevel1 = GetMasteryIncreaseFromWeaponDrain(masteryLevel1, kiAmount);
                }
                if (TransformationHelper.IsSSJ2(player) && masteryLevel2 < 1.0f)
                {
                    masteryLevel2 = GetMasteryIncreaseFromWeaponDrain(masteryLevel2, kiAmount);
                }
                if (TransformationHelper.IsSSJ3(player) && masteryLevel3 < 1.0f)
                {
                    masteryLevel3 = GetMasteryIncreaseFromWeaponDrain(masteryLevel3, kiAmount);
                }
            }
        }

        public float GetMasteryIncreaseFromWeaponDrain(float currentMastery, float kiAmount)
        {
            return Math.Min(1.0f, currentMastery + GetWeaponDrainFormMasteryContribution(kiAmount));
        }

        public float GetMasteryIncreaseFromFormDrain(float currentMastery)
        {
            return Math.Min(1.0f, currentMastery + FORM_MASTERY_GAIN_PER_TICK * GetProdigyMasteryMultiplier());
        }

        public float GetWeaponDrainFormMasteryContribution(float kiAmount)
        {
            return (0.000001f * -kiAmount) * GetProdigyMasteryMultiplier();
        }

        public float GetProdigyMasteryMultiplier()
        {
            return IsProdigy() ? 2f : 1f;
        }

        public bool IsProdigy()
        {
            return playerTrait == "Prodigy";
        }

        public void SetKi(float kiAmount, bool isSync = false)
        {
            // this might seem weird, but remote clients aren't allowed to set eachothers ki. This prevents desync issues.
            if (player.whoAmI == Main.myPlayer || isSync)
            {
                _kiCurrent = kiAmount;
            }
        }

        // return the amount of ki the player has, readonly
        public float GetKi()
        {
            return _kiCurrent;
        }

        public bool IsKiDepleted()
        {
            return _kiCurrent <= 0;
        }

        public bool HasKi(float kiAmount)
        {
            return _kiCurrent >= kiAmount;
        }

        public const int BASE_KI_MAX = 1000;

        public int GetKiMaxFromFragments()
        {
            var kiMaxValue = BASE_KI_MAX;
            kiMaxValue += (fragment1 ? 1000 : 0);
            kiMaxValue += (fragment2 ? 2000 : 0);
            kiMaxValue += (fragment3 ? 2000 : 0);
            kiMaxValue += (fragment4 ? 2000 : 0);
            kiMaxValue += (fragment5 ? 2000 : 0);
            return kiMaxValue;
        }

        public static MyPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<MyPlayer>();
        }

        public bool IsPlayerLegendary()
        {
            return playerTrait != null && playerTrait.Equals("Legendary");
        }

        public bool IsPlayerImmobilized()
        {
            return player.frozen || player.stoned || player.HasBuff(BuffID.Cursed);
        }

        public int GetPowerWishesUsed()
        {
            return POWER_WISH_MAXIMUM - powerWishesLeft;
        }

        public float PowerWishMulti()
        {
            // 10% per level
            return 1f + (GetPowerWishesUsed() / 10f);
        }

        public bool IsOverloading()
        {
            return overloadCurrent == overloadMax;
        }

        public void HandlePowerWishMultipliers()
        {
            player.meleeDamage *= PowerWishMulti();
            player.rangedDamage *= PowerWishMulti();
            player.magicDamage *= PowerWishMulti();
            player.minionDamage *= PowerWishMulti();
            player.thrownDamage *= PowerWishMulti();
            KiDamage *= PowerWishMulti();
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

        public override void PostUpdate()
        {
            if (lssjAchieved && !lssj2Achieved && player.whoAmI == Main.myPlayer && IsPlayerLegendary() && NPC.downedFishron && player.statLife <= (player.statLifeMax2 * 0.10))
            {
                lssj2Timer++;
                if (lssj2Timer >= 300)
                {
                    if (Main.rand.Next(8) == 0)
                    {
                        Main.NewText("Something uncontrollable is coming from deep inside.", Color.Green);
                        player.statLife = player.statLifeMax2 / 2;
                        player.HealEffect(player.statLifeMax2 / 2);
                        lssj2Achieved = true;
                        isTransforming = true;
                        LSSJ2Transformation();
                        UI.TransMenu.menuSelection = MenuSelectionID.LSSJ2;
                        lssj2Timer = 0;
                        TransformationHelper.EndTransformations(player);
                    }
                    else if (lssj2Timer >= 300)
                    {
                        lssj2Timer = 0;
                        Main.NewText(LSSJ2TextSelect(), Color.Red);
                    }
                }
            }

            if (kiLantern)
            {
                player.AddBuff(mod.BuffType("KiLanternBuff"), 2);
            }
            else
            {
                player.ClearBuff(mod.BuffType("KiLanternBuff"));
            }

            if (isTransforming)
            {
                ssjAuraBeamTimer++;
            }

            if (ssj1Achieved)
            {
                UI.TransMenu.ssj1On = true;
            }

            if (ssj2Achieved)
            {
                UI.TransMenu.ssj2On = true;
            }

            if (ssj3Achieved)
            {
                UI.TransMenu.ssj3On = true;
            }

            if (TransformationHelper.IsPlayerTransformed(player))
            {
                if (!(TransformationHelper.IsKaioken(player) && kaiokenLevel == 5) && !player.HasBuff(TransformationHelper.LSSJ2.GetBuffId()))
                {
                    lightningFrameTimer++;
                }
                else
                {
                    lightningFrameTimer += 2;
                }
            }

            if (lightningFrameTimer >= 15)
            {
                lightningFrameTimer = 0;
            }

            if (!TransformationHelper.IsPlayerTransformed(player))
            {
                kiDrainAddition = 0;
            }

            if (TransformationHelper.IsAnyKaioken(player))
            {
                kaiokenTimer += 1.5f;
            }

            #region Mastery Messages
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                if (masteryLevel1 >= 0.5f && !assjAchieved)
                {
                    assjAchieved = true;
                    Main.NewText("Your SSJ1 Mastery has been upgraded." +
                        "\nHold charge and transform while in SSJ1 " +
                        "\nto ascend.", 232, 242, 50);
                }
                else if (masteryLevel1 >= 0.75f && !ussjAchieved)
                {
                    ussjAchieved = true;
                    Main.NewText("Your SSJ1 Mastery has been upgraded." +
                        "\nHold charge and transform while in ASSJ " +
                        "\nto ascend.", 232, 242, 50);
                }
                else if (masteryLevel1 >= 1f && !masteredMessage1)
                {
                    masteredMessage1 = true;
                    Main.NewText("Your SSJ1 has reached Max Mastery.", 232, 242, 50);
                }
                else if (masteryLevel2 >= 1f && !masteredMessage2)
                {
                    masteredMessage2 = true;
                    Main.NewText("Your SSJ2 has reached Max Mastery.", 232, 242, 50);
                }
                else if (masteryLevel3 >= 1f && !masteredMessage3)
                {
                    masteredMessage3 = true;
                    Main.NewText("Your SSJ3 has reached Max Mastery.", 232, 242, 50);
                }
                else if (masteryLevelGod >= 1f && !masteredMessageGod)
                {
                    masteredMessageGod = true;
                    Main.NewText("Your SSJG has reached Max Mastery.", 232, 242, 50);
                }
                else if (masteryLevelBlue >= 1f && !masteredMessageBlue)
                {
                    masteredMessageBlue = true;
                    Main.NewText("Your SSJB has reached Max Mastery.", 232, 242, 50);
                }
            }

            #endregion            

            if (adamantiteBonus)
            {
                KiDamage += 7;
            }

            if (!traitChecked)
            {
                ChooseTrait();
            }

            if (lssjAchieved)
            {
                UI.TransMenu.lssjOn = true;
            }

            if (masteryLevel1 > 1)
                masteryLevel1 = 1;

            if (masteryLevel2 > 1)
                masteryLevel2 = 1;

            if (masteryLevel3 > 1)
                masteryLevel3 = 1;

            if (IsPlayerLegendary() && !lssjAchieved && NPC.downedBoss1)
            {
                player.AddBuff(mod.BuffType("UnknownLegendary"), 3);
            }
            else if (IsPlayerLegendary() && lssjAchieved)
            {
                player.AddBuff(mod.BuffType("LegendaryTrait"), 3);
                player.ClearBuff(mod.BuffType("UnknownLegendary"));
            }

            if (playerTrait == "Prodigy" && NPC.downedBoss1)
            {
                player.AddBuff(mod.BuffType("ProdigyTrait"), 3);
            }

            if (kiRegen >= 1)
            {
                kiRegenTimer++;
            }

            if (kiRegenTimer > 2)
            {
                AddKi(kiRegen, false, false);
                kiRegenTimer = 0;
            }

            if (demonBonusActive)
            {
                demonBonusTimer++;
                if (demonBonusTimer > 300)
                {
                    demonBonusActive = false;
                    demonBonusTimer = 0;
                    player.AddBuff(mod.BuffType("ArmorCooldown"), 3600);
                }
            }

            if (player.dead && TransformationHelper.IsPlayerTransformed(player))
            {
                TransformationHelper.EndTransformations(player);
                isTransforming = false;
            }

            if (rageCurrent > 5)
            {
                rageCurrent = 5;
            }

            HandleOverloadCounters();

            overallFormUnlockChance = formUnlockChance - rageCurrent;

            if (overallFormUnlockChance < 2)
            {
                overallFormUnlockChance = 2;
            }

            if (!player.HasBuff(mod.BuffType("ZenkaiBuff")) && zenkaiCharmActive)
            {
                player.AddBuff(mod.BuffType("ZenkaiCooldown"), 7200);
            }

            if (isDashing)
            {
                player.invis = true;
            }

            HandleBlackFusionMultiplier();

            // neuters flight if the player gets immobilized. Note the lack of Katchin Feet buff.
            if (IsPlayerImmobilized() && isFlying)
            {
                isFlying = false;
            }

            HandleMouseOctantAndSyncTracking();

            // flight system moved to PostUpdate so that it can benefit from not being client sided!
            FlightSystem.Update(player);

            // charge activate and charge effects moved to post update so that they can also benefit from not being client sided.
            HandleChargeEffects();

            // aura frame effects moved out of draw pass to avoid being tied to frame rate!
            
            currentAura = AnimationHelper.GetAuraEffectOnPlayer(this);

            // save the charging aura for last, and only add it to the draw layer if no other auras are firing
            if (isCharging)
            {
                if (!wasCharging)
                {
                    var chargeAuraEffects = AuraAnimations.createChargeAura;
                    HandleAuraStartupSound(chargeAuraEffects, true);
                }
            }

            if (currentAura != previousAura)
            {
                auraSoundInfo = SoundHelper.KillTrackedSound(auraSoundInfo);
                HandleAuraStartupSound(currentAura, false);
                // reset aura frame and audio timers to 0, this is important
                auraSoundTimer = 0;
                auraFrameTimer = 0;
            }

            IncrementAuraFrameTimers(currentAura);
            HandleAuraLoopSound(currentAura);

            wasCharging = isCharging;
            previousAura = currentAura;

            CheckPlayerForTransformationStateDebuffApplication();

            ThrottleKi();

            // fires at the end of all the things and makes sure the user is synced to the server with current values, also handles initial state.
            CheckSyncState();

            // Handles nerfing player defense when in Kaioken States
            HandleKaiokenDefenseDebuff();

            // if the player is in mid-transformation, totally neuter horizontal velocity
            // also handle the frame counter advancement here.
            if (isTransformationAnimationPlaying)
            {
                player.velocity = new Vector2(0, player.velocity.Y);

                transformationFrameTimer++;
            }
            else
            {
                transformationFrameTimer = 0;
            }
            KiBar.visible = true;
            if (ItemHelper.PlayerHasAllDragonBalls(player) && !wishActive)
            {
                int soundTimer = 0;
                soundTimer++;
                if (soundTimer > 300)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/DBReady"));
                    soundTimer = 0;
                }
            }
        }

        public void HandleAuraStartupSound(AuraAnimationInfo aura, bool isCharging)
        {
            if (aura == null)
                return;
            if (aura.startupSoundName != null)
            {
                SoundHelper.PlayCustomSound(aura.startupSoundName, player, 0.7f, 0.1f);
            }
        }

        public void HandleAuraLoopSound(AuraAnimationInfo aura)
        {
            if (aura == null)
                return;
            bool shouldPlayAudio = SoundHelper.ShouldPlayPlayerAudio(player, aura.isFormAura);
            if (shouldPlayAudio)
            {
                if (auraSoundTimer == 0)
                    auraSoundInfo = SoundHelper.PlayCustomSound(aura.loopSoundName, player, .7f, 0f);
                auraSoundTimer++;
                if (auraSoundTimer > aura.loopSoundDuration)
                    auraSoundTimer = 0;
            }

            // try to update positional audio?
            SoundHelper.UpdateTrackedSound(auraSoundInfo, player.position);
        }
        private bool wasInOverloadingState = false;
        private int overloadCooldownTimer = 0;
        private float overloadBlastTimer;
        private int overloadScaleCheckTimer = 0;
        private void HandleOverloadCounters()
        {
            // clamp overload current values to 0/max
            overloadCurrent = (int)Math.Max(0, Math.Min(overloadMax, overloadCurrent));

            // does the player have the legendary trait
            if (IsPlayerLegendary())
            {
                // is the player in a legendary transform step (that isn't SSJ1)?
                if (TransformationHelper.IsLSSJ(player) && !TransformationHelper.IsSSJ1(player) && overloadCurrent <= overloadMax)
                {
                    overloadTimer++;
                    if (TransformationHelper.IsLSSJ1(player))
                    {
                        if (overloadTimer >= 45)
                        {
                            overloadCurrent += 1;
                            overloadTimer = 0;
                        }
                    }
                    if (TransformationHelper.IsLSSJ2(player))
                    {
                        if (overloadTimer >= 20)
                        {
                            overloadCurrent += 1;
                            overloadTimer = 0;
                        }
                    }

                    wasInOverloadingState = true;
                }
                else if (wasInOverloadingState || IsOverloading())
                {
                    // player isn't in legendary form, cools the player overload down
                    overloadCooldownTimer++;
                    if (overloadCooldownTimer > 180)
                    {
                        overloadTimer++;
                        if (overloadTimer >= 4)
                        {
                            overloadCurrent -= 1;
                            overloadTimer = 0;
                        }
                    }
                }
            }

            if (wasInOverloadingState && overloadCurrent == 0)
            {
                wasInOverloadingState = false;
            }

            if(TransformationHelper.IsLSSJ(player) || overloadCurrent > 0)
            {
                OverloadBar.visible = true;
            }
            else
            {
                OverloadBar.visible = false;
            }

            if (overloadCurrent >= ((float)overloadMax * .7f))
            {
                const float aurawidth = 2.0f;

                for (int i = 0; i < 20; i++)
                {
                    float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * aurawidth))).X;
                    float yPos = ((Vector2.UnitY * player.height) - (Vector2.UnitY * Main.rand.Next(0, player.height))).Y - 0.5f;

                    Dust tDust = Dust.NewDustDirect(player.position + new Vector2(xPos, yPos), 1, 1, 74, 0f, -2f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

                    if ((Math.Abs((tDust.position - (player.position + (Vector2.UnitX * 7.0f))).X)) < 10)
                    {
                        tDust.scale *= 0.75f;
                    }

                    tDust.velocity.Y++;

                    Vector2 dir = -(tDust.position - ((player.position + (Vector2.UnitX * 5.0f)) - (Vector2.UnitY * player.height)));
                    dir.Normalize();

                    tDust.velocity = new Vector2(dir.X * 2.0f, -1 * Main.rand.Next(1, 5));
                    tDust.noGravity = true;
                }
            }
            
            if (IsOverloading())
            {
                
                overloadScaleCheckTimer++;
                overloadBlastTimer++;
                if (overloadBlastTimer > 2)
                {
                    Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 25;
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y,
                        mod.ProjectileType("LegendaryBlast"), 0, 0, player.whoAmI);
                    overloadBlastTimer = 0;
                }

                if (overloadScaleCheckTimer > 20)
                {
                    kiDamageMulti = Main.rand.NextFloat(0.5f, 2f);
                    overloadScaleCheckTimer = 0;
                }

            }

            //OverloadBar.visible = false;
        }

        private void HandleBlackFusionMultiplier()
        {
            bool isAnyBossAlive = false;
            foreach (NPC npc in Main.npc)
            {
                if (npc.boss && npc.active)
                {
                    isAnyBossAlive = true;
                    if (blackFusionBonus)
                    {
                        blackFusionBonusTimer++;
                        if (blackFusionBonusTimer > 300 && blackFusionIncrease <= 3f)
                        {
                            blackFusionIncrease += 0.05f;
                            blackFusionBonusTimer = 0;
                        }
                    }
                }
            }

            if (blackFusionIncrease > 1f)
            {
                player.meleeDamage *= blackFusionIncrease;
                player.rangedDamage *= blackFusionIncrease;
                player.magicDamage *= blackFusionIncrease;
                player.minionDamage *= blackFusionIncrease;
                player.thrownDamage *= blackFusionIncrease;
                KiDamage *= blackFusionIncrease;
                player.statDefense *= (int)blackFusionIncrease;
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

            if (!isAnyBossAlive)
            {
                blackFusionIncrease = 1f;
            }
        }

        public void HandleKaiokenDefenseDebuff()
        {
            if (TransformationHelper.IsAnyKaioken(player))
            {
                float defenseMultiplier = 1f - (kaiokenLevel * 0.05f);
                player.statDefense = (int)Math.Ceiling(player.statDefense * defenseMultiplier);
            }
        }

        public void HandleMouseOctantAndSyncTracking()
        {
            // we only handle the local player's controls.
            if (Main.myPlayer != player.whoAmI)
                return;

            // this is why :p
            mouseWorldOctant = GetMouseWorldOctantFromRadians(GetMouseRadiansOrDefault());
        }

        public float GetMouseRadiansOrDefault()
        {
            var mouseVector = GetMouseVectorOrDefault();
            if (mouseVector == Vector2.Zero)
                return 0f;
            return mouseVector.ToRotation();
        }

        public Vector2 GetMouseVectorOrDefault()
        {
            if (Main.myPlayer != player.whoAmI)
                return Vector2.Zero;
            Vector2 mouseVector = Vector2.Normalize(Main.MouseWorld - player.Center);
            if (player.heldProj > -1)
            {
                // player has a projectile, check to see if it's a charge ball or beam, that hijacks the octant for style.
                var proj = Main.projectile[player.heldProj];
                if (proj != null)
                {
                    if (proj.modProjectile != null && (proj.modProjectile is BaseBeamCharge))
                    {
                        var charge = proj.modProjectile as BaseBeamCharge;
                        if (charge.IsSustainingFire)
                        {
                            mouseVector = charge.myProjectile.velocity;
                        }
                        else
                        {
                            mouseVector = proj.velocity;
                        }
                    }
                }
            }
            return mouseVector;
        }

        public int GetMouseWorldOctantFromRadians(float mouseRadians)
        {
            // threshold values for octants are 22.5 degrees in either direction of a 45 degree mark on a circle (perpendicular 90s, 180s and each midway point, in 22.5 degrees either direction).
            // to make this clear, we're setting up some offset vars to make the numbers a bit more obvious.
            float thresholdDegrees = 22.5f;
            float circumferenceSpan = 45f;
            // the 8 octants, starting at the EAST mark (0) and, presumably, rotating clockwise (positive) or counter clockwise (negative).
            // note that 4 and -4 are the same thing. It doesn't matter which you use, radian outcome is the same.
            int[] octants = { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
            foreach (int octant in octants)
            {
                float minRad = MathHelper.ToRadians((octant * circumferenceSpan) - thresholdDegrees);
                float maxRad = MathHelper.ToRadians((octant * circumferenceSpan) + thresholdDegrees);
                if (mouseRadians >= minRad && mouseRadians <= maxRad)
                {
                    // normalize octant -4 to 4, for sanity reasons. They really are the same octant, but this formula isn't good enough to figure that out for some reason.
                    return octant == -4 ? 4 : octant;
                }
            }

            // this shouldn't happen, who knows.
            return 0;
        }


        public void ThrottleKi()
        {
            SetKi(Math.Max(0, Math.Min(OverallKiMax(), GetKi())));
        }


        #region Sync
        // these initialize to null so that even bools will trigger an unset one-time sync on initialization.
        // this forces newly connected players to sync their information to everyone already connected.
        public int? syncKiMax2;
        public int? syncKiMax3;
        public float? syncKiMaxMult;
        public bool? syncIsTransforming;
        public bool? syncFragment1;
        public bool? syncFragment2;
        public bool? syncFragment3;
        public bool? syncFragment4;
        public bool? syncFragment5;
        public bool? syncIsCharging;
        public bool? syncJungleMessage;
        public bool? syncHellMessage;
        public bool? syncEvilMessage;
        public bool? syncIsHoldingKiWeapon;
        public bool? syncTraitChecked;
        public string syncPlayerTrait;
        public bool? syncIsFlying;
        public bool? syncIsTransformationAnimationPlaying;
        public float? syncKiCurrent;
        public float? syncChargeMoveSpeed;
        public float? syncBonusSpeedMultiplier;
        public bool? syncWishActive;
        public int? syncKaiokenLevel;
        public int? syncMouseWorldOctant;
        public int? syncPowerWishesLeft;
        public int? syncHeldProj;
        public bool? syncIsMassiveBlastCharging;
        public bool? syncIsMassiveBlastInUse;

        // triggerset sync has its own method, but dropping these here anyway
        public bool? syncTriggerSetMouseLeft;
        public bool? syncTriggerSetMouseRight;
        public bool? syncTriggerSetLeft;
        public bool? syncTriggerSetRight;
        public bool? syncTriggerSetUp;
        public bool? syncTriggerSetDown;

        public int? syncDir;

        public void CheckSyncState()
        {
            // if we're not in network mode, do nothing.            
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            // if this method is firing on a player who isn't me, abort. 
            if (Main.myPlayer != player.whoAmI)
                return;

            if (syncKiMax2 != kiMax2)
            {
                NetworkHelper.playerSync.SendChangedKiMax2(256, player.whoAmI, player.whoAmI, kiMax2);
                syncKiMax2 = kiMax2;
            }

            if (syncKiMax3 != kiMax3)
            {
                NetworkHelper.playerSync.SendChangedKiMax3(256, player.whoAmI, player.whoAmI, kiMax3);
                syncKiMax3 = kiMax3;
            }

            if (syncKiMaxMult != kiMaxMult)
            {
                NetworkHelper.playerSync.SendChangedKiMaxMult(256, player.whoAmI, player.whoAmI, kiMaxMult);
                syncKiMaxMult = kiMaxMult;
            }

            if (syncIsTransforming != isTransforming)
            {
                NetworkHelper.playerSync.SendChangedIsTransforming(256, player.whoAmI, player.whoAmI, isTransforming);
                syncIsTransforming = isTransforming;
            }

            if (syncFragment1 != fragment1)
            {
                NetworkHelper.playerSync.SendChangedFragment1(256, player.whoAmI, player.whoAmI, fragment1);
                syncFragment1 = fragment1;
            }

            if (syncFragment2 != fragment2)
            {
                NetworkHelper.playerSync.SendChangedFragment2(256, player.whoAmI, player.whoAmI, fragment2);
                syncFragment2 = fragment2;
            }

            if (syncFragment3 != fragment3)
            {
                NetworkHelper.playerSync.SendChangedFragment3(256, player.whoAmI, player.whoAmI, fragment3);
                syncFragment3 = fragment3;
            }

            if (syncFragment4 != fragment4)
            {
                NetworkHelper.playerSync.SendChangedFragment4(256, player.whoAmI, player.whoAmI, fragment4);
                syncFragment4 = fragment4;
            }

            if (syncFragment5 != fragment5)
            {
                NetworkHelper.playerSync.SendChangedFragment5(256, player.whoAmI, player.whoAmI, fragment5);
                syncFragment5 = fragment5;
            }

            if (syncIsCharging != isCharging)
            {
                NetworkHelper.playerSync.SendChangedIsCharging(256, player.whoAmI, player.whoAmI, isCharging);
                syncIsCharging = isCharging;
            }

            if (syncJungleMessage != jungleMessage)
            {
                NetworkHelper.playerSync.SendChangedJungleMessage(256, player.whoAmI, player.whoAmI, jungleMessage);
                syncJungleMessage = jungleMessage;
            }

            if (syncHellMessage != hellMessage)
            {
                NetworkHelper.playerSync.SendChangedHellMessage(256, player.whoAmI, player.whoAmI, hellMessage);
                syncHellMessage = hellMessage;
            }

            if (syncEvilMessage != evilMessage)
            {
                NetworkHelper.playerSync.SendChangedEvilMessage(256, player.whoAmI, player.whoAmI, evilMessage);
                syncEvilMessage = evilMessage;
            }

            if (syncIsHoldingKiWeapon != isHoldingKiWeapon)
            {
                NetworkHelper.playerSync.SendChangedIsHoldingKiWeapon(256, player.whoAmI, player.whoAmI, isHoldingKiWeapon);
                syncIsHoldingKiWeapon = isHoldingKiWeapon;
            }

            if (syncTraitChecked != traitChecked)
            {
                NetworkHelper.playerSync.SendChangedTraitChecked(256, player.whoAmI, player.whoAmI, traitChecked);
                syncTraitChecked = traitChecked;
            }

            if (syncPlayerTrait != playerTrait)
            {
                NetworkHelper.playerSync.SendChangedPlayerTrait(256, player.whoAmI, player.whoAmI, playerTrait);
                syncPlayerTrait = playerTrait;
            }

            if (syncIsFlying != isFlying)
            {
                NetworkHelper.playerSync.SendChangedIsFlying(256, player.whoAmI, player.whoAmI, isFlying);
                syncIsFlying = isFlying;
            }

            if (syncIsTransformationAnimationPlaying != isTransformationAnimationPlaying)
            {
                NetworkHelper.playerSync.SendChangedIsTransformationAnimationPlaying(256, player.whoAmI, player.whoAmI, isTransformationAnimationPlaying);
                syncIsTransformationAnimationPlaying = isTransformationAnimationPlaying;
            }

            if (syncChargeMoveSpeed != chargeMoveSpeed)
            {
                NetworkHelper.playerSync.SendChangedChargeMoveSpeed(256, player.whoAmI, player.whoAmI, chargeMoveSpeed);
                syncChargeMoveSpeed = chargeMoveSpeed;
            }

            if (syncBonusSpeedMultiplier != bonusSpeedMultiplier)
            {
                NetworkHelper.playerSync.SendChangedBonusSpeedMultiplier(256, player.whoAmI, player.whoAmI, bonusSpeedMultiplier);
                syncBonusSpeedMultiplier = bonusSpeedMultiplier;
            }

            if (syncWishActive != wishActive)
            {
                NetworkHelper.playerSync.SendChangedWishActive(256, player.whoAmI, player.whoAmI, wishActive);
                syncWishActive = wishActive;
            }

            if (syncKaiokenLevel != kaiokenLevel)
            {
                NetworkHelper.playerSync.SendChangedKaiokenLevel(256, player.whoAmI, player.whoAmI, kaiokenLevel);
                syncKaiokenLevel = kaiokenLevel;
            }

            if (syncMouseWorldOctant != mouseWorldOctant)
            {
                NetworkHelper.playerSync.SendChangedMouseWorldOctant(256, player.whoAmI, player.whoAmI, mouseWorldOctant);
                syncMouseWorldOctant = mouseWorldOctant;
            }

            if (syncPowerWishesLeft != powerWishesLeft)
            {
                NetworkHelper.playerSync.SnedChangedPowerWishesLeft(256, player.whoAmI, player.whoAmI, powerWishesLeft);
                syncPowerWishesLeft = powerWishesLeft;
            }

            if (syncKiCurrent != GetKi())
            {
                NetworkHelper.playerSync.SendChangedKiCurrent(256, player.whoAmI, player.whoAmI, GetKi());
                syncKiCurrent = GetKi();
            }

            if (syncDir != player.direction)
            {
                NetworkHelper.playerSync.SendChangedDirection(256, player.whoAmI, player.whoAmI, player.direction);
                syncDir = player.direction;
            }

            if (syncHeldProj != player.heldProj)
            {
                NetworkHelper.playerSync.SendChangedHeldProjectile(256, player.whoAmI, player.whoAmI, player.heldProj);
                syncHeldProj = player.heldProj;
            }

            if (syncIsMassiveBlastCharging != isMassiveBlastCharging)
            {
                NetworkHelper.playerSync.SendChangedMassiveBlastCharging(256, player.whoAmI, player.whoAmI, isMassiveBlastCharging);
                syncIsMassiveBlastCharging = isMassiveBlastCharging;
            }

            //if (syncIsMassiveBlastInUse != isMassiveBlastInUse)
            //{
            //    NetworkHelper.playerSync.SendChangedMassiveBlastInUse(256, player.whoAmI, player.whoAmI, isMassiveBlastInUse);
            //    syncIsMassiveBlastInUse = isMassiveBlastInUse;
            //}
        }

        public void SyncTriggerSet()
        {
            // if we're not in network mode, do nothing.            
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            // if this method is firing on a player who isn't me, abort. 
            // spammy af
            if (Main.myPlayer != player.whoAmI)
                return;

            if (syncTriggerSetLeft != isLeftHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerLeft(256, player.whoAmI, player.whoAmI, isLeftHeld);
                syncTriggerSetLeft = isLeftHeld;
            }
            if (syncTriggerSetRight != isRightHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerRight(256, player.whoAmI, player.whoAmI, isRightHeld);
                syncTriggerSetRight = isRightHeld;
            }
            if (syncTriggerSetUp != isUpHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerUp(256, player.whoAmI, player.whoAmI, isUpHeld);
                syncTriggerSetUp = isUpHeld;
            }
            if (syncTriggerSetDown != isDownHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerDown(256, player.whoAmI, player.whoAmI, isDownHeld);
                syncTriggerSetDown = isDownHeld;
            }

            if (syncTriggerSetMouseRight != isMouseRightHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerMouseRight(256, player.whoAmI, player.whoAmI, isMouseRightHeld);
                syncTriggerSetMouseRight = isMouseRightHeld;
            }

            if (syncTriggerSetMouseLeft != isMouseLeftHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerMouseLeft(256, player.whoAmI, player.whoAmI, isMouseLeftHeld);
                syncTriggerSetMouseLeft = isMouseLeftHeld;
            }
        }


        #endregion

        #region Cross-mod damage increases for player
        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= blackFusionIncrease;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= blackFusionIncrease;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= PowerWishMulti();
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= PowerWishMulti();
        }

        public void TremorEffects(Player player)
        {
            player.GetModPlayer<Tremor.MPlayer>(ModLoader.GetMod("Tremor")).alchemicalDamage *= PowerWishMulti();
        }

        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).mysticDamage *= PowerWishMulti();
        }

        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage *= PowerWishMulti();
        }

        public void ExpandedSentriesEffects(Player player)
        {
            player.GetModPlayer<ExpandedSentries.ESPlayer>(ModLoader.GetMod("ExpandedSentries")).sentryDamage *= PowerWishMulti();
        }

        #endregion

        public Color? originalEyeColor = null;
        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (TransformationHelper.IsSSJG(player))
            {
                drawInfo.hairColor = new Color(255, 57, 74);
                drawInfo.hairShader = 1;
                ChangeEyeColor(Color.Red);
            }
            else if (TransformationHelper.IsSSJ(player) || TransformationHelper.IsLSSJ(player) || TransformationHelper.IsAssj(player) || TransformationHelper.IsUssj(player))
            {
                ChangeEyeColor(Color.Turquoise);
            }
            else if (TransformationHelper.IsAnyKaioken(player))
            {
                ChangeEyeColor(Color.Red);
            }
            else if (originalEyeColor.HasValue && player.eyeColor != originalEyeColor.Value)
            {
                player.eyeColor = originalEyeColor.Value;
            }
        }

        public void ChangeEyeColor(Color eyeColor)
        {
            // only fire this when attempting to change the eye color.
            if (originalEyeColor == null)
            {
                originalEyeColor = player.eyeColor;
            }
            player.eyeColor = eyeColor;
        }

        private readonly Dictionary<string, int> _traitPool = new Dictionary<string, int>()
        {
            { "Prodigy", 4 }
            , { "Legendary", 1 }
            , { "", 15 }
        };

        public void ChooseTrait()
        {
            var traitChooser = new WeightedRandom<string>();
            foreach (KeyValuePair<string, int> traitWithWeight in _traitPool)
            {
                traitChooser.Add(traitWithWeight.Key, traitWithWeight.Value);
            }
            traitChecked = true;
            playerTrait = traitChooser;
        }

        public string ChooseTraitNoLimits(string oldTrait)
        {
            var traitChooser = new WeightedRandom<string>();
            foreach (KeyValuePair<string, int> traitWithWeight in _traitPool)
            {
                if (traitWithWeight.Key.Equals(oldTrait))
                    continue;
                if (string.IsNullOrEmpty(traitWithWeight.Key))
                    continue;
                traitChooser.Add(traitWithWeight.Key, 1);
            }
            return playerTrait = traitChooser;

        }

        public void AwakeningFormUnlock()
        {
            if (!ssj1Achieved)
            {
                Main.NewText("The humiliation of failing drives you mad.", Color.Yellow);
                ssj1Achieved = true;
                isTransforming = true;
                SSJTransformation();
                UI.TransMenu.menuSelection = MenuSelectionID.SSJ1;
                TransformationHelper.EndTransformations(player);
                rageCurrent = 0;
            }
            else if (ssj1Achieved && !ssj2Achieved && !IsPlayerLegendary())
            {
                Main.NewText("The rage of failing once more dwells deep within you.", Color.Red);
                ssj2Achieved = true;
                isTransforming = true;
                SSJ2Transformation();
                UI.TransMenu.menuSelection = MenuSelectionID.SSJ2;
                TransformationHelper.EndTransformations(player);
                rageCurrent = 0;
            }
            else if (ssj1Achieved && IsPlayerLegendary() && !lssjAchieved)
            {
                Main.NewText("Your rage is overflowing, you feel something rise up from deep inside.", Color.Green);
                lssjAchieved = true;
                isTransforming = true;
                LSSJTransformation();
                UI.TransMenu.menuSelection = MenuSelectionID.LSSJ1;
                TransformationHelper.EndTransformations(player);
                rageCurrent = 0;
            }
            else if (ssj2Achieved && !ssj3Achieved)
            {
                Main.NewText("The ancient power of the Lihzahrds seeps into you, causing your power to become unstable.", Color.Orange);
                ssj3Achieved = true;
                isTransforming = true;
                SSJ3Transformation();
                UI.TransMenu.menuSelection = MenuSelectionID.SSJ3;
                TransformationHelper.EndTransformations(player);
                rageCurrent = 0;
            }
            else if (lssjAchieved && !lssj2Achieved)
            {
                Main.NewText("Something uncontrollable is coming from deep inside.", Color.Green);
                lssj2Achieved = true;
                isTransforming = true;
                LSSJ2Transformation();
                UI.TransMenu.menuSelection = MenuSelectionID.LSSJ2;
                lssj2Timer = 0;
                TransformationHelper.EndTransformations(player);
            }
        }

        public object LSSJ2TextSelect()
        {
            switch (Main.rand.Next((4)))
            {
                case 0:
                    return "You feel infuriated.";
                case 1:
                    return "Your fury escalates.";
                case 2:
                    return "Your blood boils from rage.";
                case 3:
                    return "A deep burning lingers within.";
                default:
                    return 0;

            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (radiantBonus && _kiCurrent < OverallKiMax())
            {
                int i = Main.rand.Next(1, 6);
                AddKi(i, false, false);
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), i, false, false);
                if (Main.rand.Next(2) == 0)
                {
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 20, mod.ProjectileType("RadiantSpark"), (int)KiDamage * 100, 0, player.whoAmI);
                }
            }
            if (metamoranSash)
            {
                if (Main.rand.NextBool(15))
                {
                    damage *= 2;
                }
            }
            base.OnHitNPC(item, target, damage, knockback, crit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (radiantBonus && _kiCurrent < OverallKiMax())
            {
                int i = Main.rand.Next(1, 6);
                AddKi(i, false, false);
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), i, false, false);
                if (Main.rand.Next(3) == 0)
                {
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 20, mod.ProjectileType("RadiantSpark"), (int)KiDamage * 100, 0, player.whoAmI);
                }
            }
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }

        public override void UpdateBiomeVisuals()
        {
            //bool useGodSky = Transformations.IsGodlike(player);
            //player.ManageSpecialBiomeVisuals("DBZMOD:GodSky", useGodSky, player.Center);
            player.ManageSpecialBiomeVisuals("DBZMOD:WishSky", wishActive, player.Center);
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();

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
            tag.Add("SSJ1Achieved", ssj1Achieved);
            tag.Add("SSJ2Achieved", ssj2Achieved);
            tag.Add("ASSJAchieved", assjAchieved);
            tag.Add("USSJAchieved", ussjAchieved);
            tag.Add("SSJ3Achieved", ssj3Achieved);
            // changed save routine to save to a float, orphaning the original KiCurrent.
            tag.Add("KiCurrentFloat", _kiCurrent);
            tag.Add("RageCurrent", rageCurrent);
            tag.Add("KiRegenRate", kiChargeRate);
            tag.Add("KiEssence1", kiEssence1);
            tag.Add("KiEssence2", kiEssence2);
            tag.Add("KiEssence3", kiEssence3);
            tag.Add("KiEssence4", kiEssence4);
            tag.Add("KiEssence5", kiEssence5);
            tag.Add("MenuSelection", (int)UI.TransMenu.menuSelection);
            tag.Add("MasteryLevel1", masteryLevel1);
            tag.Add("MasteryLevel2", masteryLevel2);
            tag.Add("MasteryLevel3", masteryLevel3);
            tag.Add("MasteryLevelGod", masteryLevelGod);
            tag.Add("MasteryLevelBlue", masteryLevelBlue);
            tag.Add("MasteredMessage1", masteredMessage1);
            tag.Add("MasteredMessage2", masteredMessage2);
            tag.Add("MasteredMessage3", masteredMessage3);
            tag.Add("MasteredMessageGod", masteredMessageGod);
            tag.Add("MasteredMessageBlue", masteredMessageBlue);
            tag.Add("JungleMessage", jungleMessage);
            tag.Add("HellMessage", hellMessage);
            tag.Add("EvilMessage", evilMessage);
            tag.Add("MushroomMessage", mushroomMessage);
            tag.Add("traitChecked", traitChecked);
            tag.Add("playerTrait", playerTrait);
            tag.Add("LSSJAchieved", lssjAchieved);
            tag.Add("flightUnlocked", flightUnlocked);
            tag.Add("flightDampeningUnlocked", flightDampeningUnlocked);
            tag.Add("flightUpgraded", flightUpgraded);
            tag.Add("ssjgAchieved", ssjgAchieved);
            tag.Add("LSSJ2Achieved", lssj2Achieved);
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

        public override void Load(TagCompound tag)
        {
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
            ssj1Achieved = tag.Get<bool>("SSJ1Achieved");
            ssj2Achieved = tag.Get<bool>("SSJ2Achieved");
            assjAchieved = tag.Get<bool>("ASSJAchieved");
            ussjAchieved = tag.Get<bool>("USSJAchieved");
            ssj3Achieved = tag.Get<bool>("SSJ3Achieved");
            if (tag.ContainsKey("KiCurrentFloat"))
            {
                _kiCurrent = tag.Get<float>("KiCurrentFloat");
            } else
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
            UI.TransMenu.menuSelection = (MenuSelectionID)tag.Get<int>("MenuSelection");
            masteryLevel1 = tag.Get<float>("MasteryLevel1");
            masteryLevel2 = tag.Get<float>("MasteryLevel2");
            masteryLevel3 = tag.Get<float>("MasteryLevel3");
            masteryLevelGod = tag.Get<float>("MasteryLevelGod");
            masteryLevelBlue = tag.Get<float>("MasteryLevelBlue");
            masteredMessage1 = tag.Get<bool>("MasteredMessage1");
            masteredMessage2 = tag.Get<bool>("MasteredMessage2");
            masteredMessage3 = tag.Get<bool>("MasteredMessage3");
            masteredMessageGod = tag.Get<bool>("MasteredMessageGod");
            masteredMessageBlue = tag.Get<bool>("MasteredMessageBlue");
            jungleMessage = tag.Get<bool>("JungleMessage");
            hellMessage = tag.Get<bool>("HellMessage");
            evilMessage = tag.Get<bool>("EvilMessage");
            mushroomMessage = tag.Get<bool>("MushroomMessage");
            traitChecked = tag.Get<bool>("traitChecked");
            playerTrait = tag.Get<string>("playerTrait");
            lssjAchieved = tag.Get<bool>("LSSJAchieved");
            flightUnlocked = tag.Get<bool>("flightUnlocked");
            flightDampeningUnlocked = tag.Get<bool>("flightDampeningUnlocked");
            flightUpgraded = tag.Get<bool>("flightUpgraded");
            ssjgAchieved = tag.Get<bool>("ssjgAchieved");
            lssj2Achieved = tag.Get<bool>("LSSJ2Achieved");
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

        public ProgressionSystem GetProgressionSystem()
        {
            return _mProgressionSystem;
        }

        // notes from Prime:
        // Transform goes up
        // Charge + transform ascends(ssj1 - assj, assj - ussj)
        // Powerdown is a remove all forms
        // Charge + powerdown = go down a form

        // by default, traverses up a step in transform - but starts off at whatever you've selected (letting you go straight to SSJ2 or LSSJ2 for example) in menu
        public bool IsTransformingUpOneStep()
        {
            return transform.JustPressed;
        }

        // by default simply clears all transformation buffs from the user, including kaioken.
        public bool IsCompletelyPoweringDown()
        {
            return powerDown.JustPressed && !energyCharge.Current;
        }

        // ascends the transformation state, from ssj1 to assj, or from assj to ussj
        public bool IsAscendingTransformation()
        {
            return transform.JustPressed && energyCharge.Current;
        }

        // functions four-fold. Steps down one level in a given transformation tree: ussj -> assj -> ssj1. lssj2 -> lssj -> ssj1. ssjg -> etc
        // also steps down from ssj1 + kk to just ssj1.
        public bool IsPoweringDownOneStep()
        {
            return powerDown.JustPressed && energyCharge.Current;
        }

        public bool CanAscend()
        {
            return TransformationHelper.IsSSJ1(player) || TransformationHelper.IsAssj(player);
        }

        public void HandleTransformations()
        {
            BuffInfo targetTransformation = null;

            // player has just pressed the normal transform button one time, which serves two functions.
            if (IsTransformingUpOneStep())
            {
                if (TransformationHelper.IsPlayerTransformed(player))
                {
                    // player is ascending transformation, pushing for ASSJ or USSJ depending on what form they're in.
                    if (IsAscendingTransformation())
                    {
                        if (CanAscend())
                        {
                            targetTransformation = TransformationHelper.GetNextAscensionStep(player);
                        }
                    }
                    else
                    {
                        targetTransformation = TransformationHelper.GetNextTransformationStep(player);
                    }
                }
                else
                {
                    targetTransformation = TransformationHelper.GetBuffFromMenuSelection(UI.TransMenu.menuSelection);
                }
            }
            else if (IsPoweringDownOneStep() && !TransformationHelper.IsKaioken(player))
            {
                // player is powering down a transformation state.
                targetTransformation = TransformationHelper.GetPreviousTransformationStep(player);
            }

            // if we made it this far without a target, it means for some reason we can't change transformations.
            if (targetTransformation == null)
                return;

            // finally, check that the transformation is really valid and then do it.
            if (TransformationHelper.CanTransform(player, targetTransformation))
                TransformationHelper.DoTransform(player, targetTransformation, mod);
        }

        public bool CanIncreaseKaiokenLevel()
        {
            // immediately handle aborts from super kaioken states
            if (TransformationHelper.IsSuperKaioken(player))
                return false;

            if (TransformationHelper.IsAnythingOtherThanKaioken(player))
            {
                return TransformationHelper.IsValidKaiokenForm(player) && kaiokenLevel == 0 && kaioAchieved;
            }

            switch (kaiokenLevel)
            {
                case 0:
                    return kaioAchieved;
                case 1:
                    return kaioFragment1;
                case 2:
                    return kaioFragment2;
                case 3:
                    return kaioFragment3;
                case 4:
                    return kaioFragment4;
            }
            return false;
        }

        public void HandleKaioken()
        {
            bool canIncreaseKaiokenLevel = false;
            if (kaiokenKey.JustPressed)
            {
                canIncreaseKaiokenLevel = CanIncreaseKaiokenLevel();
                if (TransformationHelper.IsKaioken(player))
                {
                    if (canIncreaseKaiokenLevel)
                    {
                        SoundHelper.PlayCustomSound("Sounds/KaioAuraAscend", player, .7f, .1f);
                        kaiokenLevel++;
                    }
                } else
                {
                    if (canIncreaseKaiokenLevel)
                    {
                        BuffInfo transformation = TransformationHelper.IsAnythingOtherThanKaioken(player) ? TransformationHelper.SuperKaioken : TransformationHelper.Kaioken;
                        if (TransformationHelper.CanTransform(player, transformation))
                        {
                            kaiokenLevel++;
                            TransformationHelper.DoTransform(player, transformation, mod);
                        }
                    }
                }
            } else if (IsPoweringDownOneStep())
            {
                if (TransformationHelper.IsKaioken(player) && kaiokenLevel > 1)
                {
                    kaiokenLevel--;
                }
            }
        }

        public void UpdateSynchronizedControls(TriggersSet triggerSet)
        {
            // this might look weird, but terraria reads these setters as changing the collection, which is bad.
            if (triggerSet.Left)
                isLeftHeld = true;
            else
                isLeftHeld = false;

            if (triggerSet.Right)
                isRightHeld = true;
            else
                isRightHeld = false;

            if (triggerSet.Up)
                isUpHeld = true;
            else
                isUpHeld = false;

            if (triggerSet.Down)
                isDownHeld = true;
            else
                isDownHeld = false;

            if (triggerSet.MouseRight)
                isMouseRightHeld = true;
            else
                isMouseRightHeld = false;

            if (triggerSet.MouseLeft)
                isMouseLeftHeld = true;
            else
                isMouseLeftHeld = false;
        }

        public float GetNextSpeedMultiplier()
        {
            if (bonusSpeedMultiplier == 0f)
                return 0.25f;
            else if (bonusSpeedMultiplier == 0.25f)
                return 0.5f;
            else if (bonusSpeedMultiplier == 0.5f)
                return 0.75f;
            else if (bonusSpeedMultiplier == 0.75f)
                return 1.0f;
            else
                return 0f;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            UpdateSynchronizedControls(triggersSet);

            SyncTriggerSet();

            if (flyToggle.JustPressed)
            {
                if (flightUnlocked)
                {
                    isFlying = !isFlying;
                    if (!isFlying)
                    {
                        FlightSystem.AddKatchinFeetBuff(player);
                    }
                }
            }

            _mProgressionSystem.Update(player);

            // dropping the fist wireup here. Fingers crossed.
            if (player.HeldItem.Name.Equals("Fist"))
            {
                _mFistSystem.Update(triggersSet, player, mod);
            }

            if (armorBonus.JustPressed)
            {
                if (demonBonus && !player.HasBuff(mod.BuffType("ArmorCooldown")))
                {
                    player.AddBuff(mod.BuffType("DemonBonus"), 300);
                    demonBonusActive = true;
                    for (int i = 0; i < 3; i++)
                    {
                        Dust tDust = Dust.NewDustDirect(player.position - (Vector2.UnitY * 0.7f) - (Vector2.UnitX * 1.0f), 50, 50, 15, 0f, 0f, 5, default(Color), 2.0f);
                    }
                }
            }

            // handle ki charging
            if (ConfigModel.isChargeToggled)
            {
                if (energyCharge.JustPressed)
                {
                    isCharging = !isCharging;
                }
            }
            else
            {
                if (energyCharge.Current && !isCharging)
                {
                    isCharging = true;
                }
                if (!energyCharge.Current && isCharging)
                {
                    isCharging = false;
                }
            }

            // most of the forms have a default light value, but charging isn't a buff. Let there be light
            if (isCharging && !TransformationHelper.IsKaioken(player) && !TransformationHelper.IsAnythingOtherThanKaioken(player))
            {
                Lighting.AddLight(player.Center, 1.2f, 1.2f, 1.2f);
            }

            // calls to handle transformation or kaioken powerups per frame

            HandleTransformations();

            HandleKaioken();

            if (speedToggle.JustPressed)
            {
                float oldSpeedMult = bonusSpeedMultiplier;
                bonusSpeedMultiplier = GetNextSpeedMultiplier();
                CombatText.NewText(player.Hitbox, new Color(255, 255, 255), string.Format("Speed bonus {0}!", (bonusSpeedMultiplier == 0f) ? "off" : ((int)Math.Ceiling(bonusSpeedMultiplier * 100f)).ToString() + "%"), false, false);
            }

            if (transMenu.JustPressed)
            {
                UI.TransMenu.menuvisible = !UI.TransMenu.menuvisible;
            }

            /*if (ProgressionMenuKey.JustPressed)
            {
                ProgressionMenu.ToggleVisibility();
            }*/

            // power down handling
            if (IsCompletelyPoweringDown() && TransformationHelper.IsPlayerTransformed(player))
            {
                var playerWasSuperKaioken = TransformationHelper.IsSuperKaioken(player);
                TransformationHelper.EndTransformations(player);
                if (playerWasSuperKaioken)
                {
                    TransformationHelper.DoTransform(player, TransformationHelper.SSJ1, mod);
                }
                kaiokenLevel = 0;
                SoundHelper.PlayCustomSound("Sounds/PowerDown", player, .3f);
            }


            if (wishActive)
            {
                WishMenu.menuVisible = true;
            }

            if (quickKi.JustPressed)
            {
                ItemHelper.ConsumeKiPotion(player);
            }

            // freeform instant transmission requires book 2.
            if (isInstantTransmission2Unlocked)
            {
                HandleInstantTransmissionFreeform();
            }
        }

        // constants to do with instant transmission range/speed/ki cost.
        protected const float INSTANT_TRANSMISSION_FRAME_KI_COST = 0.01f;
        protected const float INSTANT_TRANSMISSION_TELEPORT_MINIMUM_KI_COST = 1000f;

        // intensity is the camera pan power being used in this frame, distance is how far the camera is from the player using the power.
        public float GetInstantTransmissionFrameKiCost(float intensity, float distance)
        {
            // INSERT THINGS THAT REDUCE INSTANT TRANSMISSION COST HERE
            float costCoefficient = isInstantTransmission3Unlocked ? 0.5f : 1f;
            return INSTANT_TRANSMISSION_FRAME_KI_COST * (float)Math.Sqrt(intensity) * (float)Math.Sqrt(distance) * costCoefficient;
        }

        public float GetInstantTransmissionTeleportKiCost()
        {
            float costCoefficient = isInstantTransmission3Unlocked ? 0.5f : 1f;
            return INSTANT_TRANSMISSION_TELEPORT_MINIMUM_KI_COST;
        }

        protected const int INSTANT_TRANSMISSION_BASE_CHAOS_DURATION = 120;
        public int GetBaseChaosDuration()
        {
            int durationReduction = isInstantTransmission3Unlocked ? 2 : 1;
            return INSTANT_TRANSMISSION_BASE_CHAOS_DURATION / durationReduction;
        }

        public int GetChaosDurationByDistance(float distance)
        {
            int baseDurationOfDebuff = GetBaseChaosDuration();
            float debuffDurationCoefficient = isInstantTransmission2Unlocked ? 0.5f : 1f;
            int debuffIncrease = (int)Math.Ceiling(distance * debuffDurationCoefficient / 2000f);
            
            return baseDurationOfDebuff + debuffIncrease;
        }

        public void AddInstantTransmissionChaosDebuff(float distance) {
            // instant transmission 3 bypasses the debuff
            if (!isInstantTransmission3Unlocked)
                player.AddBuff(BuffID.ChaosState, GetChaosDurationByDistance(distance), true);
        }

        private bool _isReturningFromInstantTransmission = false;
        private float _trackedInstantTransmissionKiLoss = 0f;
        // bool handles the game feel of instant transmission by being a limbo flag.
        // the first time you press the IT key, this is set to true, but it can be set to false in mid swing to prevent further processing on the same trigger/keypress.
        private bool _isHandlingInstantTransmissionTriggers = false;
        public void HandleInstantTransmissionFreeform()
        {
            // don't mess with stuff if the map is open
            if (Main.mapFullscreen)
                return;

            // sadly this routine has to run outside the checks, because we don't know if we're allowed to IT to a spot unless we do this first.
            Vector2 screenMiddle = Main.screenPosition + (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
            Vector2 direction = Vector2.Normalize(Main.MouseWorld - screenMiddle);
            float distance = Vector2.Distance(Main.MouseWorld, player.Center);
            // throttle intensity by a lot
            float intensity = Math.Min(128f, (float)Vector2.Distance(Main.MouseWorld, screenMiddle)) / 2f;
            float kiCost = GetInstantTransmissionFrameKiCost(intensity, distance);

            // the one frame delay on handling instant transmission is to set up the limbo var.
            // this should also theoretically prevent fullscreen map transmission from double-firing ITs.
            if (!_isHandlingInstantTransmissionTriggers && instantTransmission.JustPressed) {
                _isHandlingInstantTransmissionTriggers = true;
            }
            if (_isHandlingInstantTransmissionTriggers && instantTransmission.Current && HasKi(kiCost + GetInstantTransmissionTeleportKiCost()))
            {
                // player is trying to IT and has the ki to do so.
                // set the limbo var to true until we stop handling
                _isReturningFromInstantTransmission = true;

                _trackedInstantTransmissionKiLoss += kiCost;
                AddKi(-kiCost, false, false);

                Main.zoomX += (direction * intensity).X;
                if (Main.zoomX + player.Center.X >= Main.maxTilesX * 16f)
                    Main.zoomX = (Main.maxTilesX * 16f) - player.Center.X;
                if (Main.zoomX + player.Center.X <= 0)                
                    Main.zoomX = -player.Center.X;                

                Main.zoomY += (direction * intensity).Y;
                if (Main.zoomY + player.Center.Y >= Main.maxTilesY * 16f)
                    Main.zoomY = (Main.maxTilesY * 16f) - player.Center.Y;                
                if (Main.zoomY + player.Center.Y <= 0)                
                    Main.zoomY = -player.Center.Y;
                
            } else if (_isHandlingInstantTransmissionTriggers && ((instantTransmission.JustReleased  || (instantTransmission.Current && !HasKi(kiCost + GetInstantTransmissionTeleportKiCost()))) && HasKi(GetInstantTransmissionTeleportKiCost())))
            {
                // player has either let go of the instant transmission key or run out of ki. either way, disable further processing and try to teleport
                // if we fail, the player gets some ki back but the processing is still canceled.
                _isReturningFromInstantTransmission = true;
                _isHandlingInstantTransmissionTriggers = false;

                Vector2 target;
                target.X = Main.mouseX + Main.screenPosition.X;
                if (player.gravDir == 1f)
                {
                    target.Y = Main.mouseY + Main.screenPosition.Y - player.height;
                }
                else
                {
                    target.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
                }

                if (TryTransmission(target, distance))
                {
                    // there's no need to "return" from IT, you succeeded.
                    // make sure we don't try to give the player back their ki
                    _trackedInstantTransmissionKiLoss = 0f;
                }
            } else if (_isReturningFromInstantTransmission)
            {
                AddKi(_trackedInstantTransmissionKiLoss, false, false);
                _trackedInstantTransmissionKiLoss = 0f;
                _isReturningFromInstantTransmission = false;
                _isHandlingInstantTransmissionTriggers = false;
                Main.zoomX = 0f;
                Main.zoomY = 0f;
            }
        }

        public bool TryTransmission(Vector2 target, float distance)
        {
            Vector2 originalPosition = player.Center;
            if (!HandleInstantTransmissionExitRoutine(target, distance))
            {
                AddKi(_trackedInstantTransmissionKiLoss, false, false);
                _trackedInstantTransmissionKiLoss = 0f;
                return false;
            }
            else
            {
                Projectile.NewProjectile(originalPosition.X, originalPosition.Y, 0f, 0f, DBZMOD.instance.ProjectileType("TransmissionLinesProj"), 0, 0, player.whoAmI);
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, DBZMOD.instance.ProjectileType("TransmissionLinesProj"), 0, 0, player.whoAmI);
                
                AddKi(-GetInstantTransmissionTeleportKiCost(), false, false);
                return true;
            }
        }

        public bool HandleInstantTransmissionExitRoutine(Vector2 target, float distance)
        {
            // unabashedly stolen from decompiled source for rod of discord.
            // find a suitable place to IT to, reversing the camera pan direction if necessary.
            target.Y -= 32f;
            if (target.X > 50f && target.X < (float)(Main.maxTilesX * 16 - 50) && target.Y > 50f && target.Y < (float)(Main.maxTilesY * 16 - 50))
            {
                int tileX = (int)(target.X / 16f);
                int tileY = (int)(target.Y / 16f);
                if (((Main.tile[tileX, tileY] != null && Main.tile[tileX, tileY].wall != 87) || (double)tileY <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(target, player.width, player.height))
                {
                    if (target.X < player.Center.X)
                        player.ChangeDir(-1);
                    else
                        player.ChangeDir(1);
                    player.Teleport(target, -1);
                    NetMessage.SendData(65, -1, -1, null, 0, (float)player.whoAmI, target.X, target.Y, 1, 0, 0);
                    if (player.chaosState)
                    {
                        player.statLife -= player.statLife / 7;
                        PlayerDeathReason damageSource = PlayerDeathReason.ByOther(13);
                        if (Main.rand.Next(2) == 0)
                        {
                            damageSource = PlayerDeathReason.ByOther(player.Male ? 14 : 15);
                        }
                        player.lifeRegenCount = 0;
                        player.lifeRegenTime = 0;
                    }
                    AddInstantTransmissionChaosDebuff(distance);
                    return true;
                }
            }
            return false;
        }

        // occurs in vanilla code *just* before regen effects are applied.
        public override void PostUpdateMiscEffects()
        {
            base.PostUpdateMiscEffects();

            // pretend the player has the shiny stone here - this is just in time for the vanilla regen calls to kick in.
            if (buldariumSigmite)
                player.shinyStone = true;
        }

        public void HandleChargeEffects()
        {
            // various effects while charging
            // if the player is flying and moving, charging applies a speed boost and doesn't recharge ki, but also doesn't slow the player.
            bool isAnyKeyHeld = isLeftHeld || isRightHeld || isUpHeld || isDownHeld;
            if (isCharging)
            {
                bool isChargeBoostingFlight = isFlying && isAnyKeyHeld;
                bool shouldApplySlowdown = GetKi() < OverallKiMax() && !isChargeBoostingFlight;
                // grant defense and a protective barrier visual if charging with baldur essentia
                if (!isChargeBoostingFlight)
                {
                    if (baldurEssentia || buldariumSigmite)
                    {
                        var defenseBoost = Math.Max(baldurEssentia ? 1.3f : 1f, buldariumSigmite ? 1.5f : 1f);
                        shouldApplySlowdown = true;
                        // only create the projectile if one doesn't exist already.
                        if (player.ownedProjectileCounts[mod.ProjectileType("BaldurShell")] == 0)
                            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaldurShell"), 0, 0, player.whoAmI);
                        player.statDefense = (int)(player.statDefense * defenseBoost);
                    }
                    if (burningEnergyAmulet)
                    {
                        shouldApplySlowdown = true;
                        FireAura();
                    }
                    if (iceTalisman)
                    {
                        shouldApplySlowdown = true;
                        FrostAura();
                    }
                    if (pureEnergyCirclet)
                    {
                        shouldApplySlowdown = true;
                        PureEnergyAura();
                    }

                    // determine base regen rate and bonuses
                    AddKi(kiChargeRate + scarabChargeRateAdd, false, false);
                }

                if (shouldApplySlowdown)
                {
                    ProjectileHelper.ApplyChannelingSlowdown(player);
                }
            }

            // grant multiplicative charge bonuses that grow over time if using either earthen accessories
            if (isCharging && GetKi() < OverallKiMax() && (earthenScarab || earthenArcanium))
            {
                scarabChargeTimer++;
                if (scarabChargeTimer > 180 && scarabChargeRateAdd <= 5)
                {
                    scarabChargeRateAdd += 1;
                    scarabChargeTimer = 0;
                }
            } else { 
                // reset scarab/earthen bonuses
                scarabChargeTimer = 0;
                scarabChargeRateAdd = 0;
            }
        }

        public MyPlayer() : base()
        {
        }

        public override void ResetEffects()
        {
            KiDamage = 1f;
            kiKbAddition = 0f;
            kiChargeRate = 1;
            if (kiEssence1)            
                kiChargeRate += 1;
            if (kiEssence2)
                kiChargeRate += 1;
            if (kiEssence3)
                kiChargeRate += 2;
            if (kiEssence4)
                kiChargeRate += 2;
            if (kiEssence5)
                kiChargeRate += 3;
            scouterT2 = false;
            scouterT3 = false;
            scouterT4 = false;
            scouterT5 = false;
            scouterT6 = false;
            spiritualEmblem = false;
            turtleShell = false;
            kiDrainMulti = 1f;
            kiSpeedAddition = 1f;
            kiCrit = 5;
            chlorophyteHeadPieceActive = false;
            diamondNecklace = false;
            emeraldNecklace = false;
            rubyNecklace = false;
            earthenSigil = false;
            radiantTotem = false;
            zenkaiCharm = false;
            majinNucleus = false;
            baldurEssentia = false;
            earthenArcanium = false;
            legendNecklace = false;
            legendWaistcape = false;
            kiChip = false;
            radiantGlider = false;
            dragongemNecklace = false;
            sapphireNecklace = false;
            topazNecklace = false;
            amberNecklace = false;
            amethystNecklace = false;
            kiOrbDropChance = 3;
            isHoldingKiWeapon = false;
            wornGloves = false;
            senzuBag = false;
            palladiumBonus = false;
            adamantiteBonus = false;
            orbGrabRange = 2;
            orbHealAmount = 50;
            demonBonus = false;
            blackFusionBonus = false;
            chargeLimitAdd = 0;
            flightSpeedAdd = 0;
            flightUsageAdd = 0;
            kiRegen = 0;
            earthenScarab = false;
            hermitBonus = false;
            zenkaiCharmActive = false;
            chargeTimerMaxAdd = 0;
            spiritCharm = false;
            battleKit = false;
            armCannon = false;
            radiantBonus = false;
            crystalliteControl = false;
            crystalliteFlow = false;
            crystalliteAlleviate = false;
            chargeMoveSpeed = 0f;
            kaiokenDrainMulti = 1f;
            kaioCrystal = false;
            luminousSectum = false;
            infuserAmber = false;
            infuserAmethyst = false;
            infuserDiamond = false;
            infuserEmerald = false;
            infuserRainbow = false;
            infuserRuby = false;
            infuserSapphire = false;
            infuserTopaz = false;
            blackDiamondShell = false;
            buldariumSigmite = false;
            attunementBracers = false;
            burningEnergyAmulet = false;
            iceTalisman = false;
            pureEnergyCirclet = false;
            timeRing = false;
            bloodstainedBandana = false;
            goblinKiEnhancer = false;
            mechanicalAmplifier = false;
            metamoranSash = false;
            kiMax2 = 0;
            bool hasLegendaryBuff = player.HasBuff(mod.BuffType("LegendaryTrait")) || player.HasBuff(mod.BuffType("UnknownLegendary"));
            kiMaxMult = hasLegendaryBuff ? 2f : 1f;
            isHoldingDragonRadarMk1 = false;
            isHoldingDragonRadarMk2 = false;
            isHoldingDragonRadarMk3 = false;
            eliteSaiyanBonus = false;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            
            bool isAnyBossAlive = false;
            bool isGolemAlive = false;
            foreach (NPC npc in Main.npc)
            {
                if (npc.boss && npc.active)
                {
                    isAnyBossAlive = true;
                }
                if (npc.type == NPCID.Golem)
                {
                    isGolemAlive = true;
                }
            }
            if (zenkaiCharm && !zenkaiCharmActive && !player.HasBuff(mod.BuffType("ZenkaiCooldown")))
            {
                player.statLife = 50;
                player.HealEffect(50);
                player.AddBuff(mod.BuffType("ZenkaiBuff"), 300);
                return false;
            }
            if (eliteSaiyanBonus && !zenkaiCharmActive && !player.HasBuff(mod.BuffType("ZenkaiCooldown")))
            {
                int healamount = (player.statLifeMax + player.statLifeMax2);
                player.statLife += healamount;
                player.HealEffect(healamount);
                player.AddBuff(mod.BuffType("ZenkaiBuff"), 600);
                return false;
            }

            if (isAnyBossAlive && !ssj1Achieved && player.whoAmI == Main.myPlayer && NPC.downedBoss3)
            {
                if (rageCurrent >= 3)
                {
                    overallFormUnlockChance = 1;
                }
                else
                {
                    formUnlockChance = 20;
                }
                if ((Main.rand.Next(overallFormUnlockChance) == 0))
                {
                    Main.NewText("The humiliation of failing drives you mad.", Color.Yellow);
                    player.statLife = player.statLifeMax2 / 2;
                    player.HealEffect(player.statLifeMax2 / 2);
                    ssj1Achieved = true;
                    isTransforming = true;
                    SSJTransformation();
                    UI.TransMenu.menuSelection = MenuSelectionID.SSJ1;
                    rageCurrent = 0;
                    TransformationHelper.EndTransformations(player);
                    return false;
                }
            }

            if (isAnyBossAlive && ssj1Achieved && !ssj2Achieved && player.whoAmI == Main.myPlayer && !IsPlayerLegendary() && NPC.downedMechBossAny && (TransformationHelper.IsSSJ1(player) || TransformationHelper.IsAssj(player) || TransformationHelper.IsUssj(player)) && masteryLevel1 >= 1)
            {
                Main.NewText("The rage of failing once more dwells deep within you.", Color.Red);
                player.statLife = player.statLifeMax2 / 2;
                player.HealEffect(player.statLifeMax2 / 2);
                ssj2Achieved = true;
                isTransforming = true;
                SSJ2Transformation();
                UI.TransMenu.menuSelection = MenuSelectionID.SSJ2;
                TransformationHelper.EndTransformations(player);
                rageCurrent = 0;
                return false;
            }

            if (isAnyBossAlive && ssj1Achieved && !lssjAchieved && player.whoAmI == Main.myPlayer && IsPlayerLegendary() && NPC.downedMechBossAny && player.HasBuff(TransformationHelper.SSJ1.GetBuffId()) && masteryLevel1 >= 1)
            {
                Main.NewText("Your rage is overflowing, you feel something rise up from deep inside.", Color.Green);
                player.statLife = player.statLifeMax2 / 2;
                player.HealEffect(player.statLifeMax2 / 2);
                lssjAchieved = true;
                isTransforming = true;
                LSSJTransformation();
                UI.TransMenu.menuSelection = MenuSelectionID.LSSJ1;
                TransformationHelper.EndTransformations(player);
                rageCurrent = 0;
                return false;
            }

            if (isGolemAlive && ssj1Achieved && ssj2Achieved && !ssj3Achieved && !IsPlayerLegendary() && player.whoAmI == Main.myPlayer && player.HasBuff(TransformationHelper.SSJ2.GetBuffId()) && masteryLevel2 >= 1)
            {
                Main.NewText("The ancient power of the Lihzahrds seeps into you, causing your power to become unstable.", Color.Orange);
                player.statLife = player.statLifeMax2 / 2;
                player.HealEffect(player.statLifeMax2 / 2);
                ssj3Achieved = true;
                isTransforming = true;
                SSJ3Transformation();
                UI.TransMenu.menuSelection = MenuSelectionID.SSJ3;
                TransformationHelper.EndTransformations(player);
                rageCurrent = 0;
                return false;
            }

            if (immortalityRevivesLeft > 0)
            {
                int healamount = (player.statLifeMax + player.statLifeMax2);
                player.statLife += healamount;
                player.HealEffect(healamount);
                immortalityRevivesLeft -= 1;
                return false;
            }

            if (isAnyBossAlive && player.whoAmI == Main.myPlayer)
            {
                rageCurrent += 1;
                return true;
            }

            return true;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            // i frames during transformation
            if (isTransforming)
            {
                return false;
            }

            // PLACE DAMAGE CANCELLING EFFECTS HERE.

            // handle blocking damage reductions
            switch (blockState)
            {
                case 1:
                    return false; // damage negated
                case 2:
                    damage /= 3;
                    break;
                case 3:
                    damage /= 2;
                    break;
            }

            // handle chlorophyte regen
            if (chlorophyteHeadPieceActive && !player.HasBuff(mod.BuffType("ChlorophyteRegen")))
            {
                player.AddBuff(mod.BuffType("ChlorophyteRegen"), 180);                
            }

            // handle ki enhancer "reserves" buff
            if (goblinKiEnhancer && !player.HasBuff(mod.BuffType("EnhancedReserves")))
            {
                player.AddBuff(mod.BuffType("EnhancedReserves"), 180);                
            }

            // black diamond ki bonus
            if (blackDiamondShell)
            {
                int i = Main.rand.Next(10, 100);
                AddKi(i, false, false);
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), i, false, false);
            }

            // increment current mastery if applicable, for damage taken.
            HandleDamageReceivedMastery(damage);

            return true;
        }

        // currently doesn't use the damage received param. Included just in case.
        public void HandleDamageReceivedMastery(int damageReceived)
        {
            if (TransformationHelper.IsSSJ1(player))
            {
                masteryLevel1 = Math.Min(1.0f, masteryLevel1 + 0.00232f);
            }
            if (TransformationHelper.IsAssj(player))
            {
                masteryLevel1 = Math.Min(1.0f, masteryLevel1 + 0.00232f);
            }
            if (TransformationHelper.IsUssj(player))
            {
                masteryLevel1 = Math.Min(1.0f, masteryLevel1 + 0.00232f);
            }
            if (TransformationHelper.IsSSJ2(player))
            {
                masteryLevel2 = Math.Min(1.0f, masteryLevel2 + 0.00232f);
            }
            if (TransformationHelper.IsSSJ3(player))
            {
                masteryLevel3 = Math.Min(1.0f, masteryLevel3 + 0.00232f);
            }
        }

        public const float AURA_DUST_EFFECT_WIDTH = 2f;
        public void HandleAuraDust(int dustType)
        {
            for (int i = 0; i < 4; i++)
            {
                float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * AURA_DUST_EFFECT_WIDTH))).X;
                float yPos = ((Vector2.UnitY * player.height) - (Vector2.UnitY * Main.rand.Next(0, player.height))).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(player.position + new Vector2(xPos, yPos), 1, 1, dustType, 0f, 0f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

                if ((Math.Abs((tDust.position - (player.position + (Vector2.UnitX * 7.0f))).X)) < 10)
                {
                    tDust.scale *= 0.75f;
                }

                Vector2 dir = -(tDust.position - ((player.position + (Vector2.UnitX * 5.0f)) - (Vector2.UnitY * player.height)));
                dir.Normalize();

                tDust.velocity = new Vector2(dir.X * 2.0f, -1 * Main.rand.Next(1, 5));
                tDust.noGravity = true;
            }
        }

        public List<NPC> GetNpcsInAuraRadius(bool excludeFriendly, bool excludeHostile, float auraRadius)
        {
            var results = new List<NPC>();
            for(var i = 0; i < Main.npc.Length; i++)
            {
                var npc = Main.npc[i];
                if (npc == null)
                    continue;
                if (npc.friendly && excludeFriendly)
                    continue;
                if (!npc.friendly && excludeHostile)
                    continue;
                Vector2 closestPoint = GetClosestPoint(npc, player.Center);
                if (Vector2.Distance(closestPoint, player.Center) > auraRadius)
                    continue;
                results.Add(npc);
            }

            return results;
        }

        public Vector2 GetClosestPoint(Entity e, Vector2 targetLocation)
        {
            Vector2 halfWidth = new Vector2(e.width / 2f, 0);
            Vector2 halfHeight = new Vector2(0, e.height / 2f);
            Vector2 fullWidth = new Vector2(e.width, 0);
            Vector2 fullHeight = new Vector2(0, e.height);
            // takes 8 poll locations on the outer edge of the sprite. Is not perfect, but is good enough.
            // you can skip center, it will never be closer to another entity than one of its edges.
            Vector2[] pollLocations = new Vector2[] {
                e.position, e.Center - halfHeight, e.position + fullWidth,                
                e.Center - halfWidth, e.Center + halfWidth,
                e.position + fullHeight, e.Center + halfHeight, e.position + fullWidth + fullHeight
            };
            float shortestDistance = float.MaxValue;
            Vector2 closestPoint = Vector2.Zero;
            for (var i = 0; i < pollLocations.Length; i++)
            {
                var location = pollLocations[i];
                var distance = Vector2.Distance(location, targetLocation);
                if (distance < shortestDistance) {
                    shortestDistance = distance;
                    closestPoint = location;
                }
            }
            return closestPoint;
        }

        public List<Player> GetPlayersInAuraRadius(bool excludeFriendly, bool excludeHostile, float auraRadius)
        {
            var results = new List<Player>();
            for (var i = 0; i < Main.player.Length; i++)
            {
                var playerTarget = Main.player[i];

                // no nulls
                if (playerTarget == null)
                    continue;
                // no weird shit
                if (playerTarget.whoAmI != i)
                    continue;
                // no hostiles
                bool isSameTeam = Main.player[Main.myPlayer].team == playerTarget.team && playerTarget.team != 0;
                if ((Main.player[Main.myPlayer].hostile || playerTarget.hostile) && !isSameTeam && excludeHostile)
                    continue;
                if ((!Main.player[Main.myPlayer].hostile || !playerTarget.hostile || isSameTeam) && excludeFriendly)
                    continue;
                // no dead bodies
                if (!playerTarget.active || playerTarget.dead)
                    continue;
                // why would you do this
                if (playerTarget.whoAmI == Main.myPlayer)
                    continue;
                // too far away?
                Vector2 closestPoint = GetClosestPoint(playerTarget, player.Center);
                if (Vector2.Distance(closestPoint, player.Center) > auraRadius)
                    continue;
                results.Add(playerTarget);
            }

            return results;
        }

        public void HandleAuraBuff(int buffId, bool excludeFriendly, bool excludeHostile, float auraRadius)
        {
            List<NPC> seekNpcs = GetNpcsInAuraRadius(excludeFriendly, excludeHostile, auraRadius);
            List<Player> seekPlayers = GetPlayersInAuraRadius(excludeFriendly, excludeHostile, auraRadius);
            foreach(var npc in seekNpcs)
            {
                if (npc.HasBuff(buffId))
                    continue;
                npc.AddBuff(buffId, 10, true);
            }

            foreach(var player in seekPlayers)
            {
                if (player.HasBuff(buffId))
                    continue;
                player.AddBuff(buffId, 10, true);
            }
        }


        public const float AURA_RADIUS = 256f; // 16 tiles, pretty wide.
        public void FrostAura()
        {
            HandleAuraDust(59);
            HandleAuraBuff(BuffID.Frostburn, true, false, AURA_RADIUS);
        }

        public void FireAura()
        {
            HandleAuraDust(60);
            HandleAuraBuff(BuffID.OnFire, true, false, AURA_RADIUS);
        }

        public void PureEnergyAura()
        {
            HandleAuraDust(107);
            HandleAuraBuff(BuffID.OnFire, true, false, AURA_RADIUS);
            HandleAuraBuff(BuffID.Frostburn, true, false, AURA_RADIUS);
        }

        public void SSJTransformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJRockProjStart"), 0, 0, player.whoAmI);
            SoundHelper.PlayCustomSound("Sounds/GroundRumble", player);
        }

        public void SSJ2Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJAuraBall"), 0, 0, player.whoAmI);
            SoundHelper.PlayCustomSound("Sounds/Awakening", player);
        }

        public void SSJ3Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJ3LightPillar"), 0, 0, player.whoAmI);
            SoundHelper.PlayCustomSound("Sounds/Awakening", player);
        }

        public void LSSJTransformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJAuraBall"), 0, 0, player.whoAmI);
            SoundHelper.PlayCustomSound("Sounds/Awakening", player);
        }

        public void LSSJ2Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("LSSJ2PillarStart"), 0, 0, player.whoAmI);
            SoundHelper.PlayCustomSound("Sounds/Awakening", player);
        }

        public void SSJGTransformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJGDustStart"), 0, 0, player.whoAmI);
            SoundHelper.PlayCustomSound("Sounds/Awakening", player);
        }

        public override void SetupStartInventory(IList<Item> items)
        {
            Item item8 = new Item();
            item8.SetDefaults(mod.ItemType("EmptyNecklace"));
            item8.stack = 1;
            items.Add(item8);
        }

        public void CheckPlayerForTransformationStateDebuffApplication()
        {
            if (!DebugHelper.IsDebugModeOn())
            {
                wasKaioken = isKaioken;
                wasTransformed = isTransformed;

                isKaioken = TransformationHelper.IsAnyKaioken(player);
                isTransformed = TransformationHelper.IsAnythingOtherThanKaioken(player);
                // this way, we can apply exhaustion debuffs correctly.
                if (wasKaioken && !isKaioken)
                {
                    bool wasSsjkk = wasTransformed;
                    TransformationHelper.AddKaiokenExhaustion(player, wasSsjkk ? 2 : 1);
                    kaiokenLevel = 0; // make triple sure the Kaio level gets reset.
                }
                if (wasTransformed && !isTransformed)
                {
                    TransformationHelper.AddTransformationExhaustion(player);
                }
            }
        }

        public int auraFrameTimer = 0;
        public int auraCurrentFrame = 0;
        public void IncrementAuraFrameTimers(AuraAnimationInfo aura)
        {
            if (aura == null)
                return;
            // doubled frame timer while charging.
            if (isCharging && aura.id != (int)AuraID.Charge)
                auraFrameTimer++;

            auraFrameTimer++;
            if (auraFrameTimer >= aura.frameTimerLimit)
            {
                auraFrameTimer = 0;
                auraCurrentFrame++;
            }
            if (auraCurrentFrame >= aura.frames)
            {
                auraCurrentFrame = 0;
            }
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            //handle lightning effects
            AnimationHelper.lightningEffects.visible = true;
            layers.Add(AnimationHelper.lightningEffects);

            // handle transformation animations
            AnimationHelper.transformationEffects.visible = true;
            layers.Add(AnimationHelper.transformationEffects);

            // handle dragon radar drawing
            if (isHoldingDragonRadarMk1 || isHoldingDragonRadarMk2 || isHoldingDragonRadarMk3)
            {
                AnimationHelper.dragonRadarEffects.visible = true;
                layers.Add(AnimationHelper.dragonRadarEffects);
            }
            
            AnimationHelper.auraEffect.visible = true;
            // capture the back layer index, which should always exist before the hook fires.
            var index = layers.FindIndex(x => x.Name == "MiscEffectsBack");
            layers.Insert(index, AnimationHelper.auraEffect);

            // handle SSJ hair/etc.
            int hair = layers.FindIndex(l => l == PlayerLayer.Hair);
            if (hair < 0)
                return;
            if (this.hair != null)
            {
                layers[hair] = new PlayerLayer(mod.Name, "TransHair",
                    delegate (PlayerDrawInfo draw)
                    {
                        Player player = draw.drawPlayer;

                        Color alpha = draw.drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)(draw.position.X + draw.drawPlayer.width * 0.5) / 16, (int)((draw.position.Y + draw.drawPlayer.height * 0.25) / 16.0), Color.White), draw.shadow);
                        DrawData data = new DrawData(this.hair, new Vector2((float)((int)(draw.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(draw.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.headPosition + draw.headOrigin, player.bodyFrame, alpha, player.headRotation, draw.headOrigin, 1f, draw.spriteEffects, 0);
                        data.shader = draw.hairShader;
                        Main.playerDrawData.Add(data);
                    });
            }

            if (this.hair != null)
            {
                PlayerLayer.Head.visible = false;
                PlayerLayer.Hair.visible = false;
                PlayerLayer.HairBack.visible = false;
                PlayerHeadLayer.Hair.visible = false;
                PlayerHeadLayer.Head.visible = false;
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (victim != player && victim.whoAmI != NPCID.TargetDummy)
            {
                float expierenceToAdd = 10.0f;
                float experienceMult = 1.0f;

                if (TransformationHelper.IsPlayerTransformed(player))
                {
                    experienceMult = 2.0f;
                }

                _mProgressionSystem.AddKiExperience(expierenceToAdd * experienceMult);
            }
            base.OnHitAnything(x, y, victim);
        }

        public override void UpdateLifeRegen()
        {
            base.UpdateLifeRegen();
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            base.NaturalLifeRegen(ref regen);

            HandlePowerWishPlayerHealth();
            HandleOverloadHealthChange();
        }        

        public override void UpdateBadLifeRegen()
        {
            base.UpdateBadLifeRegen();

            // Kaioken neuters regen and drains the player
            if (TransformationHelper.IsAnyKaioken(player))
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;

                // only apply the kaio crystal benefit if this is kaioken
                bool isKaioCrystalEquipped = player.IsAccessoryEquipped("Kaio Crystal");
                float drainMult = isKaioCrystalEquipped ? 0.5f : 1f;
                
                // recalculate the final health drain rate and reduce regen by that amount
                var healthDrain = (int)Math.Ceiling(TransBuff.GetTotalHealthDrain(player) * drainMult);
                player.lifeRegen -= healthDrain;
            }
        }

        public override void PostUpdateEquips()
        {
            HandlePowerWishMultipliers();
        }

        public void HandlePowerWishPlayerHealth()
        {
            player.statLifeMax2 = player.statLifeMax2 + GetPowerWishesUsed() * 20;
        }

        private float overloadHealthChange = 0f;
        public void HandleOverloadHealthChange()
        {
            overloadHealthChange = Main.rand.NextFloat(0.3f, 1.5f);
            player.statLifeMax *= (int)overloadHealthChange;
            player.statLifeMax2 *= (int)overloadHealthChange;
        }

        public Texture2D hair;

        public override void PreUpdate()
        {
            if (TransformationHelper.IsPlayerTransformed(player))
            {
                if (!player.armor[10].vanity && player.armor[10].headSlot == -1)
                {
                    if (TransformationHelper.IsSSJ1(player))
                    {
                        hair = mod.GetTexture("Hairs/SSJ/SSJ1Hair");
                    }
                    else if (TransformationHelper.IsAssj(player))
                    {
                        hair = mod.GetTexture("Hairs/SSJ/ASSJHair");
                    }
                    else if (TransformationHelper.IsUssj(player))
                    {
                        hair = mod.GetTexture("Hairs/SSJ/USSJHair");
                    }
                    else if (TransformationHelper.IsSuperKaioken(player))
                    {
                        hair = mod.GetTexture("Hairs/SSJ/SSJ1KaiokenHair");
                    }
                    else if (TransformationHelper.IsSSJ2(player))
                    {
                        hair = mod.GetTexture("Hairs/SSJ/SSJ2Hair");
                    }
                    else if (TransformationHelper.IsSSJ3(player))
                    {
                        hair = mod.GetTexture("Hairs/SSJ/SSJ3Hair");
                    }
                    else if (TransformationHelper.IsLSSJ1(player))
                    {
                        hair = mod.GetTexture("Hairs/LSSJ/LSSJHair");
                    }
                    else if (TransformationHelper.IsLSSJ2(player))
                    {
                        hair = mod.GetTexture("Hairs/LSSJ/LSSJ2Hair");
                    }
                    else if (TransformationHelper.IsSpectrum(player))
                    {
                        hair = mod.GetTexture("Hairs/Dev/SSJSHair");
                    }
                    if(TransformationHelper.IsSSJG(player))
                    {
                        hair = null;
                    }
                }
            }
            else
            {
                hair = null;
            }
            if (player.dead)
            {
                hair = null;
            }
        }
    }
}