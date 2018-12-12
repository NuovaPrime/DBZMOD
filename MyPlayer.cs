using System;
using System.Collections.Generic;
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
using Config;
using Util;
using Enums;
using Network;

namespace DBZMOD
{
    public class MyPlayer : ModPlayer
    {
        #region Variables
        //Player vars
        public float KiDamage;
        public float KiKbAddition;
        public float KiSpeedAddition;
        public int KiCrit;
        public int KiRegenTimer;
        public int KiRegen;

        // kiMax is now a property that gets reset when it's accessed and less than or equal to zero, to retro fix nasty bugs
        // there's no point changing this value as it only resets itself if it doesn't line up with fragment ki max.

        public int KiMax()
        {
            return GetKiMaxFromFragments();
        }

        // ki max 2 is ki max from equipment and accessories. there's no point changing this value as it gets reset each frame.
        public int KiMax2;

        // progression upgrades increase KiMax3. This is the only value that can be changed to have an impact on ki max and does not reset.
        public int KiMax3;

        // ki max mult is a multiplier for ki that stacks multiplicatively with other KiMaxMult bonuses. It resets to 1f each frame.
        public float KiMaxMult = 1f;

        // made KiCurrent private forcing everyone to use a method that syncs to clients, centralizing ki increase/decrease logic.
        private int KiCurrent;

        public int KiChargeRate = 1;
        public int OverloadMax = 100;
        public int OverloadCurrent;
        public int OverloadTimer;
        public float chargeMoveSpeed;

        //Transformation vars
        public bool IsTransforming;
        public int SSJAuraBeamTimer;
        public bool hasSSJ1;
        public int TransformCooldown;
        public bool ASSJAchieved;
        public bool USSJAchieved;
        public bool SSJ2Achieved;
        public bool SSJ3Achieved;
        public bool LSSJAchieved = false;
        public bool SSJGAchieved = false;
        public int lssj2timer;
        public bool LSSJ2Achieved = false;
        public bool LSSJGAchieved = false;
        public int RageCurrent = 0;
        public int RageDecreaseTimer = 0;
        public int FormUnlockChance;
        public int OverallFormUnlockChance;
        public bool IsOverloading;

        //Input vars
        public static ModHotKey KaiokenKey;
        public static ModHotKey EnergyCharge;
        public static ModHotKey Transform;
        public static ModHotKey PowerDown;
        public static ModHotKey SpeedToggle;
        public static ModHotKey QuickKi;
        public static ModHotKey TransMenu;
        //public static ModHotKey ProgressionMenuKey;
        public static ModHotKey FlyToggle;
        public static ModHotKey ArmorBonus;

        //mastery vars
        public float MasteryLevel1 = 0;
        public float MasteryMax1 = 1;
        public bool MasteredMessage1 = false;
        public float MasteryLevel2 = 0;
        public float MasteryMax2 = 1;
        public bool MasteredMessage2 = false;
        public float MasteryLevel3 = 0;
        public float MasteryMax3 = 1;
        public bool MasteredMessage3 = false;
        public float MasteryLevelGod = 0;
        public float MasteryMaxGod = 1;
        public bool MasteredMessageGod = false;
        public float MasteryLevelBlue = 0;
        public float MasteryMaxBlue = 1;
        public bool MasteredMessageBlue = false;
        public float MasteryMaxFlight = 1;
        public float MasteryLevelFlight = 0;

        //unsorted vars
        public int drawX;
        public int drawY;
        public bool SSJ1Achieved;
        public bool scouterT2;
        public bool scouterT3;
        public bool scouterT4;
        public bool scouterT5;
        public bool scouterT6;
        public bool Fragment1;
        public bool Fragment2;
        public bool Fragment3;
        public bool Fragment4;
        public bool Fragment5;
        public bool KaioFragment1;
        public bool KaioFragment2;
        public bool KaioFragment3;
        public bool KaioFragment4;
        public bool ChlorophyteHeadPieceActive;
        public bool KaioAchieved;
        public bool KiEssence1;
        public bool KiEssence2;
        public bool KiEssence3;
        public bool turtleShell;
        public bool KiEssence4;
        public bool KiEssence5;
        public bool DemonBonusActive;
        public bool spiritualEmblem;
        public float KaiokenTimer = 0.0f;
        public bool kiLantern;
        public bool speedToggled = true;
        public float KiDrainMulti;
        public bool diamondNecklace;
        public bool emeraldNecklace;
        public bool sapphireNecklace;
        public bool topazNecklace;
        public bool amberNecklace;
        public bool amethystNecklace;
        public bool rubyNecklace;
        public bool dragongemNecklace;
        public bool IsCharging;
        // bool used internally to handle managing effects
        public bool WasCharging;
        public int ChargeSoundTimer;
        public int ChargeLimitAdd;
        //public static bool RealismMode = false;
        public bool JungleMessage = false;
        public bool HellMessage = false;
        public bool EvilMessage = false;
        public bool MushroomMessage = false;
        public int KiOrbDropChance;
        public bool IsHoldingKiWeapon;
        public bool wornGloves;
        public bool senzuBag;
        public bool palladiumBonus;
        public bool adamantiteBonus;
        public bool traitChecked = false;
        public string playerTrait = null;
        public bool DemonBonus;
        public int OrbGrabRange;
        public int OrbHealAmount;
        public bool IsFlying;

        public int FlightUsageAdd;
        public float FlightSpeedAdd;
        public bool earthenSigil;
        public bool earthenScarab;
        public bool radiantTotem;
        public int ScarabChargeRateAdd;
        public int ScarabChargeTimer;
        public bool flightUnlocked = false;
        public bool flightDampeningUnlocked = false;
        public bool flightUpgraded = false;
        public int DemonBonusTimer;
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
        public int KiDrainAddition;
        public float KaiokenDrainMulti;
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
        public bool IsDashing;
        public bool CanUseHeavyHit;
        public bool CanUseFlurry;
        public bool CanUseZanzoken;
        public int BlockState;
        public bool blackDiamondShell;
        public bool buldariumSigmite;
        public bool attunementBracers;
        public bool burningEnergyAmulet;
        public bool iceTalisman;
        public bool pureEnergyCirclet;
        public bool timeRing;
        public bool bloodstainedBandana;
        public bool blackFusionBonus;
        public float blackFusionIncrease = 1f;
        public int blackFusionBonusTimer;
        public SoundEffectInstance transformationSound;
        #endregion

        #region Syncable Controls

        public bool IsLeftHeld = false;
        public bool IsRightHeld = false;
        public bool IsUpHeld = false;
        public bool IsDownHeld = false;
        #endregion

        #region Classes
        ProgressionSystem m_progressionSystem = new ProgressionSystem();
        FistSystem m_fistSystem = new FistSystem();
        #endregion

        // overall ki max is now just a formula representing your total ki, after all bonuses are applied.
        public int OverallKiMax()
        {
            return (int)Math.Ceiling((KiMax() + KiMax2 + KiMax3) * KiMaxMult * (player.HasBuff(mod.BuffType("LegendaryTrait")) ? 2f : 1f));
        }

        // all changes to Ki Current are now made through this method.
        public void AddKi(int kiAmount)
        {
            SetKi(KiCurrent + kiAmount);
        }

        public void SetKi(int kiAmount, bool isSync = false)
        {
            // this might seem weird, but remote clients aren't allowed to set eachothers ki. This prevents desync issues.
            if (player.whoAmI == Main.myPlayer || isSync)
            {
                KiCurrent = kiAmount;
            }
        }

        // return the amount of ki the player has, readonly
        public int GetKi()
        {
            return KiCurrent;
        }

        public bool IsKiDepleted()
        {
            return KiCurrent <= 0;
        }

        public bool HasKi(int kiAmount)
        {
            return KiCurrent >= kiAmount;
        }

        public const int BASE_KI_MAX = 1000;

        public int GetKiMaxFromFragments()
        {
            var kiMaxValue = BASE_KI_MAX;
            kiMaxValue += (Fragment1 ? 1000 : 0);
            kiMaxValue += (Fragment2 ? 2000 : 0);
            kiMaxValue += (Fragment3 ? 2000 : 0);
            kiMaxValue += (Fragment4 ? 2000 : 0);
            kiMaxValue += (Fragment5 ? 2000 : 0);
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

        // tracks whether a player was flying in the previous frame. Allows Flight System to apply Katchin Feet correctly
        private bool WasFlying = false;

        public override void PostUpdate()
        {            
            if (LSSJAchieved && !LSSJ2Achieved && player.whoAmI == Main.myPlayer && IsPlayerLegendary() && NPC.downedFishron && player.statLife <= (player.statLifeMax2 * 0.10))
            {
                lssj2timer++;
                if (lssj2timer >= 300)
                {
                    if (Main.rand.Next(8) == 0)
                    {
                        Main.NewText("Something uncontrollable is coming from deep inside.", Color.Green);
                        player.statLife = player.statLifeMax2 / 2;
                        player.HealEffect(player.statLifeMax2 / 2);
                        LSSJ2Achieved = true;
                        IsTransforming = true;
                        LSSJ2Transformation();
                        UI.TransMenu.MenuSelection = MenuSelectionID.LSSJ2;
                        lssj2timer = 0;
                        Transformations.EndTransformations(player, true, false);
                    }
                    else if (lssj2timer >= 300)
                    {
                        lssj2timer = 0;
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
            if (IsTransforming)
            {
                SSJAuraBeamTimer++;
            }
            if (OverloadCurrent < 0)
            {
                OverloadCurrent = 0;
            }
            if (SSJ1Achieved)
            {
                UI.TransMenu.SSJ1On = true;
            }
            if (SSJ2Achieved)
            {
                UI.TransMenu.SSJ2On = true;
            }
            if (SSJ3Achieved)
            {
                UI.TransMenu.SSJ3On = true;
            }
            if (Transformations.IsPlayerTransformed(player))
            {
                if (!player.HasBuff(Transformations.Kaioken100.BuffId) && !player.HasBuff(Transformations.LSSJ2.BuffId))
                {
                    LightningFrameTimer++;
                }
                else
                {
                    LightningFrameTimer += 2;
                }
            }

            if (LightningFrameTimer >= 15)
            {
                LightningFrameTimer = 0;
            }

            if (!Transformations.IsPlayerTransformed(player))
            {
                KiDrainAddition = 0;
            }

            if (Transformations.IsKaioken(player) || Transformations.IsSSJ1Kaioken(player))
            {
                KaiokenTimer += 1.5f;
            }

            #region Mastery Messages
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                if (MasteryLevel1 >= 0.5f && !ASSJAchieved)
                {
                    ASSJAchieved = true;
                    Main.NewText("Your SSJ1 Mastery has been upgraded." +
                        "\nHold charge and transform while in SSJ1 " +
                        "\nto ascend.", 232, 242, 50);
                }
                else if (MasteryLevel1 >= 0.75f && !USSJAchieved)
                {
                    USSJAchieved = true;
                    Main.NewText("Your SSJ1 Mastery has been upgraded." +
                        "\nHold charge and transform while in ASSJ " +
                        "\nto ascend.", 232, 242, 50);
                }
                else if (MasteryLevel1 >= 1f && !MasteredMessage1)
                {
                    MasteredMessage1 = true;
                    Main.NewText("Your SSJ1 has reached Max Mastery.", 232, 242, 50);
                }
                else if (MasteryLevel2 >= 1f && !MasteredMessage2)
                {
                    MasteredMessage2 = true;
                    Main.NewText("Your SSJ2 has reached Max Mastery.", 232, 242, 50);
                }
                else if (MasteryLevel3 >= 1f && !MasteredMessage3)
                {
                    MasteredMessage3 = true;
                    Main.NewText("Your SSJ3 has reached Max Mastery.", 232, 242, 50);
                }
                else if (MasteryLevelGod >= 1f && !MasteredMessageGod)
                {
                    MasteredMessageGod = true;
                    Main.NewText("Your SSJG has reached Max Mastery.", 232, 242, 50);
                }
                else if (MasteryLevelBlue >= 1f && !MasteredMessageBlue)
                {
                    MasteredMessageBlue = true;
                    Main.NewText("Your SSJB has reached Max Mastery.", 232, 242, 50);
                }
            }

            if (MasteryLevel1 > MasteryMax1)
            {
                MasteryLevel1 = MasteryMax1;
            }
            else if (MasteryLevel2 > MasteryMax2)
            {
                MasteryLevel2 = MasteryMax2;
            }
            else if (MasteryLevel3 > MasteryMax3)
            {
                MasteryLevel3 = MasteryMax3;
            }
            else if (MasteryLevelGod > MasteryMaxGod)
            {
                MasteryLevelGod = MasteryMaxGod;
            }
            else if (MasteryLevelBlue > MasteryMaxBlue)
            {
                MasteryLevelBlue = MasteryMaxBlue;
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

            if (LSSJAchieved)
            {
                UI.TransMenu.LSSJOn = true;
            }

            if (IsPlayerLegendary() && !LSSJAchieved && NPC.downedBoss1)
            {
                player.AddBuff(mod.BuffType("UnknownLegendary"), 3);
            }
            else if (IsPlayerLegendary() && LSSJAchieved)
            {
                player.AddBuff(mod.BuffType("LegendaryTrait"), 3);
                player.ClearBuff(mod.BuffType("UnknownLegendary"));
            }

            if (playerTrait == "Prodigy" && NPC.downedBoss1)
            {
                player.AddBuff(mod.BuffType("ProdigyTrait"), 3);
            }

            if (KiRegen >= 1)
            {
                KiRegenTimer++;
            }

            if (KiRegenTimer > 2)
            {
                // DebugUtil.Log(string.Format("I think my ki regen is {0}", KiRegen));
                AddKi(KiRegen);
                KiRegenTimer = 0;
            }

            if (DemonBonusActive)
            {
                DemonBonusTimer++;
                if (DemonBonusTimer > 300)
                {
                    DemonBonusActive = false;
                    DemonBonusTimer = 0;
                    player.AddBuff(mod.BuffType("ArmorCooldown"), 3600);
                }
            }

            if (player.dead && Transformations.IsPlayerTransformed(player))
            {
                Transformations.EndTransformations(player, true, false);
                IsTransforming = false;
            }

            if (RageCurrent > 5)
            {
                RageCurrent = 5;
            }

            if (OverloadCurrent > OverloadMax)
            {
                OverloadCurrent = OverloadMax;
            }

            // does the player have the legendary trait
            if (IsPlayerLegendary())
            {
                // is the player in a legendary transform step (that isn't SSJ1)?
                if (Transformations.IsLSSJ(player) && !Transformations.IsSSJ1(player))
                {
                    OverloadTimer++;
                    if (OverloadTimer >= 60)
                    {
                        OverloadCurrent += 1;
                        OverloadTimer = 0;
                    }
                }
                else
                {
                    // player isn't in legendary form, cools the player overload down
                    OverloadTimer++;
                    if (OverloadTimer >= 30)
                    {
                        OverloadCurrent -= 2;
                        OverloadTimer = 0;
                    }
                }
            }

            OverallFormUnlockChance = FormUnlockChance - RageCurrent;

            /*if (!(playerTrait == null))
            {
                Main.NewText(playerTrait);
            }*/

            if (OverallFormUnlockChance < 2)
            {
                OverallFormUnlockChance = 2;
            }

            if (!player.HasBuff(mod.BuffType("ZenkaiBuff")) && zenkaiCharmActive)
            {
                player.AddBuff(mod.BuffType("ZenkaiCooldown"), 7200);
            }

            if (IsDashing)
            {
                player.invis = true;
            }

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

            /*if(LSSJAchieved)
            {
                OverloadBar.visible = true;
            }
            else
            {
                OverloadBar.visible = false;
            }*/
            OverloadBar.visible = false;
            KiBar.visible = true;
            
            // flight system moved to PostUpdate so that it can benefit from not being client sided!
            FlightSystem.Update(player, WasFlying);

            WasFlying = IsFlying;

            // charge activate and charge effects moved to post update so that they can also benefit from not being client sided.
            HandleChargeEffects();

            HandleChargeVisualAndSoundEffects();

            // update WasCharging so we can ensure we're managing state each frame
            WasCharging = IsCharging;

            ThrottleKi();

            // fires at the end of all the things and makes sure the user is synced to the server with current values, also handles initial state.
            CheckSyncState();
        }

        public void ThrottleKi()
        {
            SetKi(Math.Max(0, Math.Min(OverallKiMax(), GetKi())));
        }


        #region Sync
        // these initialize to null so that even bools will trigger an unset one-time sync on initialization.
        // this forces newly connected players to sync their information to everyone already connected.
        public int? SyncKiMax2;
        public int? SyncKiMax3;
        public float? SyncKiMaxMult;
        public bool? SyncIsTransforming;
        public bool? SyncFragment1;
        public bool? SyncFragment2;
        public bool? SyncFragment3;
        public bool? SyncFragment4;
        public bool? SyncFragment5;
        public bool? SyncIsCharging;
        public bool? SyncJungleMessage;
        public bool? SyncHellMessage;
        public bool? SyncEvilMessage;
        public bool? SyncIsHoldingKiWeapon;
        public bool? SyncTraitChecked;
        public string SyncPlayerTrait;
        public bool? SyncIsFlying;
        public int? SyncKiCurrent;
        public float? SyncChargeMoveSpeed;

        // triggerset sync has its own method, but dropping these here anyway
        public bool? SyncTriggerSetLeft;
        public bool? SyncTriggerSetRight;
        public bool? SyncTriggerSetUp;
        public bool? SyncTriggerSetDown;

        public void CheckSyncState()
        {
            // if we're not in network mode, do nothing.            
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            // if this method is firing on a player who isn't me, abort. 
            // spammy af
            // DebugUtil.Log(string.Format("Player is {0}, I am {1}. If different, abort.", player.whoAmI, Main.myPlayer));
            if (Main.myPlayer != player.whoAmI)
                return;

            if (SyncKiMax2 != KiMax2)
            {
                NetworkHelper.playerSync.SendChangedKiMax2(256, player.whoAmI, player.whoAmI, KiMax2);
                SyncKiMax2 = KiMax2;
            }

            if (SyncKiMax3 != KiMax3)
            {
                NetworkHelper.playerSync.SendChangedKiMax3(256, player.whoAmI, player.whoAmI, KiMax3);
                SyncKiMax3 = KiMax3;
            }

            if (SyncKiMaxMult != KiMaxMult)
            {
                // DebugUtil.Log(string.Format("Ki max sync: was {0} but should be {1}", SyncKiMaxMult, KiMaxMult));
                NetworkHelper.playerSync.SendChangedKiMaxMult(256, player.whoAmI, player.whoAmI, KiMaxMult);
                SyncKiMaxMult = KiMaxMult;
            }

            if (SyncIsTransforming != IsTransforming)
            {
                NetworkHelper.playerSync.SendChangedIsTransforming(256, player.whoAmI, player.whoAmI, IsTransforming);
                SyncIsTransforming = IsTransforming;
            }

            if (SyncFragment1 != Fragment1)
            {
                NetworkHelper.playerSync.SendChangedFragment1(256, player.whoAmI, player.whoAmI, Fragment1);
                SyncFragment1 = Fragment1;
            }

            if (SyncFragment2 != Fragment2)
            {
                NetworkHelper.playerSync.SendChangedFragment2(256, player.whoAmI, player.whoAmI, Fragment2);
                SyncFragment2 = Fragment2;
            }

            if (SyncFragment3 != Fragment3)
            {
                NetworkHelper.playerSync.SendChangedFragment3(256, player.whoAmI, player.whoAmI, Fragment3);
                SyncFragment3 = Fragment3;
            }

            if (SyncFragment4 != Fragment4)
            {
                NetworkHelper.playerSync.SendChangedFragment4(256, player.whoAmI, player.whoAmI, Fragment4);
                SyncFragment4 = Fragment4;
            }

            if (SyncFragment5 != Fragment5)
            {
                NetworkHelper.playerSync.SendChangedFragment5(256, player.whoAmI, player.whoAmI, Fragment5);
                SyncFragment5 = Fragment5;
            }

            if (SyncIsCharging != IsCharging)
            {
                NetworkHelper.playerSync.SendChangedIsCharging(256, player.whoAmI, player.whoAmI, IsCharging);
                SyncIsCharging = IsCharging;
            }

            if (SyncJungleMessage != JungleMessage)
            {
                NetworkHelper.playerSync.SendChangedJungleMessage(256, player.whoAmI, player.whoAmI, JungleMessage);
                SyncJungleMessage = JungleMessage;
            }

            if (SyncHellMessage != HellMessage)
            {
                NetworkHelper.playerSync.SendChangedHellMessage(256, player.whoAmI, player.whoAmI, HellMessage);
                SyncHellMessage = HellMessage;
            }

            if (SyncEvilMessage != EvilMessage)
            {
                NetworkHelper.playerSync.SendChangedEvilMessage(256, player.whoAmI, player.whoAmI, EvilMessage);
                SyncEvilMessage = EvilMessage;
            }

            if (SyncIsHoldingKiWeapon != IsHoldingKiWeapon)
            {
                NetworkHelper.playerSync.SendChangedIsHoldingKiWeapon(256, player.whoAmI, player.whoAmI, IsHoldingKiWeapon);
                SyncIsHoldingKiWeapon = IsHoldingKiWeapon;
            }

            if (SyncTraitChecked != traitChecked)
            {
                NetworkHelper.playerSync.SendChangedTraitChecked(256, player.whoAmI, player.whoAmI, traitChecked);
                SyncTraitChecked = traitChecked;
            }

            if (SyncPlayerTrait != playerTrait)
            {
                NetworkHelper.playerSync.SendChangedPlayerTrait(256, player.whoAmI, player.whoAmI, playerTrait);
                SyncPlayerTrait = playerTrait;
            }

            if (SyncIsFlying != IsFlying)
            {
                NetworkHelper.playerSync.SendChangedIsFlying(256, player.whoAmI, player.whoAmI, IsFlying);
                SyncIsFlying = IsFlying;
            }

            if (SyncChargeMoveSpeed != chargeMoveSpeed)
            {
                NetworkHelper.playerSync.SendChangedChargeMoveSpeed(256, player.whoAmI, player.whoAmI, chargeMoveSpeed);
                SyncChargeMoveSpeed = chargeMoveSpeed;
            }

            if (SyncKiCurrent != GetKi())
            {
                NetworkHelper.playerSync.SendChangedKiCurrent(256, player.whoAmI, player.whoAmI, GetKi());
                SyncKiCurrent = GetKi();
            }
        }

        public void SyncTriggerSet()
        {
            // if we're not in network mode, do nothing.            
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            // if this method is firing on a player who isn't me, abort. 
            // spammy af
            // DebugUtil.Log(string.Format("Player is {0}, I am {1}. If different, abort.", player.whoAmI, Main.myPlayer));
            if (Main.myPlayer != player.whoAmI)
                return;

            if (SyncTriggerSetLeft != IsLeftHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerLeft(256, player.whoAmI, player.whoAmI, IsLeftHeld);
                SyncTriggerSetLeft = IsLeftHeld;
            }
            if (SyncTriggerSetRight != IsRightHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerRight(256, player.whoAmI, player.whoAmI, IsRightHeld);
                SyncTriggerSetRight = IsRightHeld;
            }
            if (SyncTriggerSetUp != IsUpHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerUp(256, player.whoAmI, player.whoAmI, IsUpHeld);
                SyncTriggerSetUp = IsUpHeld;
            }
            if (SyncTriggerSetDown != IsDownHeld)
            {
                NetworkHelper.playerSync.SendChangedTriggerDown(256, player.whoAmI, player.whoAmI, IsDownHeld);
                SyncTriggerSetDown = IsDownHeld;
            }
        }


        #endregion

        #region Cross-mod damage increases for player
        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= blackFusionIncrease;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= blackFusionIncrease;
        }

        public void TremorEffects(Player player)
        {
            player.GetModPlayer<Tremor.MPlayer>(ModLoader.GetMod("Tremor")).alchemicalDamage *= blackFusionIncrease;
        }

        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).mysticDamage *= blackFusionIncrease;
        }

        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage *= blackFusionIncrease;
        }

        public void ExpandedSentriesEffects(Player player)
        {
            player.GetModPlayer<ExpandedSentries.ESPlayer>(ModLoader.GetMod("ExpandedSentries")).sentryDamage *= blackFusionIncrease;
        }

        #endregion

        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (Transformations.IsGodlike(player))
            {
                drawInfo.hairColor = new Color(183, 25, 46);
                drawInfo.hairShader = 1;
                player.eyeColor = Color.Red;
                // godlike is included in SSJ, so only make them turquoise if not in god form.
            }
            else if (Transformations.IsSSJ(player) || Transformations.IsLSSJ(player))
            {
                player.eyeColor = Color.Turquoise;
            }
            // this still works tho
            if (Transformations.IsSSJ1Kaioken(player))
            {
                player.eyeColor = Color.Red;
            }
        }

        public string ChooseTrait()
        {
            var TraitChooser = new WeightedRandom<string>();
            TraitChooser.Add("Prodigy", 4);
            TraitChooser.Add("Legendary", 1);
            TraitChooser.Add(null, 15);
            traitChecked = true;
            return playerTrait = TraitChooser;

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
            if (radiantBonus && KiCurrent < OverallKiMax())
            {
                int i = Main.rand.Next(1, 6);
                AddKi(i);
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), i, false, false);
                if (Main.rand.Next(2) == 0)
                {
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 20, mod.ProjectileType("RadiantSpark"), (int)KiDamage * 100, 0, player.whoAmI);
                }
            }
            base.OnHitNPC(item, target, damage, knockback, crit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (radiantBonus && KiCurrent < OverallKiMax())
            {
                int i = Main.rand.Next(1, 6);
                AddKi(i);
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), i, false, false);
                if (Main.rand.Next(3) == 0)
                {
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 20, mod.ProjectileType("RadiantSpark"), (int)KiDamage * 100, 0, player.whoAmI);
                }
            }
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }

        /*public override void UpdateBiomeVisuals()
        {
            bool useGodSky = Transformations.IsGodlike(player);
            player.ManageSpecialBiomeVisuals("DBZMOD:GodSky", useGodSky, player.Center);
        }*/

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();

            tag.Add("Fragment1", Fragment1);
            tag.Add("Fragment2", Fragment2);
            tag.Add("Fragment3", Fragment3);
            tag.Add("Fragment4", Fragment4);
            tag.Add("Fragment5", Fragment5);
            tag.Add("KaioFragment1", KaioFragment1);
            tag.Add("KaioFragment2", KaioFragment2);
            tag.Add("KaioFragment3", KaioFragment3);
            tag.Add("KaioFragment4", KaioFragment4);
            tag.Add("KaioAchieved", KaioAchieved);
            tag.Add("SSJ1Achieved", SSJ1Achieved);
            tag.Add("SSJ2Achieved", SSJ2Achieved);
            tag.Add("ASSJAchieved", ASSJAchieved);
            tag.Add("USSJAchieved", USSJAchieved);
            tag.Add("SSJ3Achieved", SSJ3Achieved);
            tag.Add("KiCurrent", KiCurrent);
            tag.Add("RageCurrent", RageCurrent);
            tag.Add("KiRegenRate", KiChargeRate);
            tag.Add("KiEssence1", KiEssence1);
            tag.Add("KiEssence2", KiEssence2);
            tag.Add("KiEssence3", KiEssence3);
            tag.Add("KiEssence4", KiEssence4);
            tag.Add("KiEssence5", KiEssence5);
            tag.Add("MenuSelection", (int)UI.TransMenu.MenuSelection);
            tag.Add("MasteryLevel1", MasteryLevel1);
            tag.Add("MasteryLevel2", MasteryLevel2);
            tag.Add("MasteryLevel3", MasteryLevel3);
            tag.Add("MasteryLevelGod", MasteryLevelGod);
            tag.Add("MasteryLevelBlue", MasteryLevelBlue);
            tag.Add("MasteredMessage1", MasteredMessage1);
            tag.Add("MasteredMessage2", MasteredMessage2);
            tag.Add("MasteredMessage3", MasteredMessage3);
            tag.Add("MasteredMessageGod", MasteredMessageGod);
            tag.Add("MasteredMessageBlue", MasteredMessageBlue);
            tag.Add("JungleMessage", JungleMessage);
            tag.Add("HellMessage", HellMessage);
            tag.Add("EvilMessage", EvilMessage);
            tag.Add("MushroomMessage", MushroomMessage);
            tag.Add("traitChecked", traitChecked);
            tag.Add("playerTrait", playerTrait);
            tag.Add("LSSJAchieved", LSSJAchieved);
            tag.Add("flightUnlocked", flightUnlocked);
            tag.Add("flightDampeningUnlocked", flightDampeningUnlocked);
            tag.Add("flightUpgraded", flightUpgraded);
            tag.Add("ssjgAchieved", SSJGAchieved);
            tag.Add("LSSJ2Achieved", LSSJ2Achieved);
            tag.Add("KiMax3", KiMax3);
            //tag.Add("RealismMode", RealismMode);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            Fragment1 = tag.Get<bool>("Fragment1");
            Fragment2 = tag.Get<bool>("Fragment2");
            Fragment3 = tag.Get<bool>("Fragment3");
            Fragment4 = tag.Get<bool>("Fragment4");
            Fragment5 = tag.Get<bool>("Fragment5");
            KaioFragment1 = tag.Get<bool>("KaioFragment1");
            KaioFragment2 = tag.Get<bool>("KaioFragment2");
            KaioFragment3 = tag.Get<bool>("KaioFragment3");
            KaioFragment4 = tag.Get<bool>("KaioFragment4");
            KaioAchieved = tag.Get<bool>("KaioAchieved");
            SSJ1Achieved = tag.Get<bool>("SSJ1Achieved");
            SSJ2Achieved = tag.Get<bool>("SSJ2Achieved");
            ASSJAchieved = tag.Get<bool>("ASSJAchieved");
            USSJAchieved = tag.Get<bool>("USSJAchieved");
            SSJ3Achieved = tag.Get<bool>("SSJ3Achieved");
            KiCurrent = tag.Get<int>("KiCurrent");
            RageCurrent = tag.Get<int>("RageCurrent");
            KiChargeRate = tag.Get<int>("KiRegenRate");
            KiEssence1 = tag.Get<bool>("KiEssence1");
            KiEssence2 = tag.Get<bool>("KiEssence2");
            KiEssence3 = tag.Get<bool>("KiEssence3");
            KiEssence4 = tag.Get<bool>("KiEssence4");
            KiEssence5 = tag.Get<bool>("KiEssence5");
            UI.TransMenu.MenuSelection = (MenuSelectionID)tag.Get<int>("MenuSelection");
            MasteryLevel1 = tag.Get<float>("MasteryLevel1");
            MasteryLevel2 = tag.Get<float>("MasteryLevel2");
            MasteryLevel3 = tag.Get<float>("MasteryLevel3");
            MasteryLevelGod = tag.Get<float>("MasteryLevelGod");
            MasteryLevelBlue = tag.Get<float>("MasteryLevelBlue");
            MasteredMessage1 = tag.Get<bool>("MasteredMessage1");
            MasteredMessage2 = tag.Get<bool>("MasteredMessage2");
            MasteredMessage3 = tag.Get<bool>("MasteredMessage3");
            MasteredMessageGod = tag.Get<bool>("MasteredMessageGod");
            MasteredMessageBlue = tag.Get<bool>("MasteredMessageBlue");
            JungleMessage = tag.Get<bool>("JungleMessage");
            HellMessage = tag.Get<bool>("HellMessage");
            EvilMessage = tag.Get<bool>("EvilMessage");
            MushroomMessage = tag.Get<bool>("MushroomMessage");
            traitChecked = tag.Get<bool>("traitChecked");
            playerTrait = tag.Get<string>("playerTrait");
            LSSJAchieved = tag.Get<bool>("LSSJAchieved");
            flightUnlocked = tag.Get<bool>("flightUnlocked");
            flightDampeningUnlocked = tag.Get<bool>("flightDampeningUnlocked");
            flightUpgraded = tag.Get<bool>("flightUpgraded");
            SSJGAchieved = tag.Get<bool>("ssjgAchieved");
            LSSJ2Achieved = tag.Get<bool>("LSSJ2Achieved");
            KiMax3 = tag.Get<int>("KiMax3");
            //RealismMode = tag.Get<bool>("RealismMode");
        }

        public ProgressionSystem GetProgressionSystem()
        {
            return m_progressionSystem;
        }

        public void StopTransformationSound()
        {
            if (transformationSound != null)
            {
                transformationSound.Stop();
                transformationSound = null;
            }
        }

        // notes from Prime:
        // Transform goes up
        // Charge + transform ascends(ssj1 - assj, assj - ussj)
        // Powerdown is a remove all forms
        // Charge + powerdown = go down a form

        // by default, traverses up a step in transform - but starts off at whatever you've selected (letting you go straight to SSJ2 or LSSJ2 for example) in menu
        public bool IsTransformingUpOneStep()
        {
            return Transform.JustPressed && !EnergyCharge.Current;
        }

        // by default simply clears all transformation buffs from the user, including kaioken.
        public bool IsCompletelyPoweringDown()
        {
            return PowerDown.JustPressed && !EnergyCharge.Current;
        }

        // ascends the transformation state, from ssj1 to assj, or from assj to ussj
        public bool IsAscendingTransformation()
        {
            return Transform.JustPressed && EnergyCharge.Current;
        }

        // functions four-fold. Steps down one level in a given transformation tree: ussj -> assj -> ssj1. lssj2 -> lssj -> ssj1. ssjg -> etc
        // also steps down from ssj1 + kk to just ssj1.
        public bool IsPoweringDownOneStep()
        {
            return PowerDown.JustPressed && EnergyCharge.Current;
        }

        public bool CanAscend()
        {
            return Transformations.IsSSJ1(player) || Transformations.IsASSJ(player);
        }

        public void HandleTransformations()
        {
            bool isPoweringDownOneStep = false;
            BuffInfo targetTransformation = null;

            // player has just pressed the normal transform button one time, which serves two functions.
            if (IsTransformingUpOneStep())
            {
                // if the player is already transformed, determine what the next step is.
                if (Transformations.IsPlayerTransformed(player))
                {
                    targetTransformation = Transformations.GetNextTransformationStep(player);
                }
                else
                {
                    // otherwise set them to transform to whatever their selected transformation is.
                    targetTransformation = Transformations.GetBuffFromMenuSelection(UI.TransMenu.MenuSelection);
                }
            }
            // player is ascending transformation, pushing for ASSJ or USSJ depending on what form they're in.
            else if (IsAscendingTransformation())
            {
                if (!CanAscend())
                    return;

                targetTransformation = Transformations.GetNextAscensionStep(player);
            }
            else if (IsPoweringDownOneStep() && !Transformations.IsKaioken(player) && !Transformations.IsSSJ1Kaioken(player))
            {
                // player is powering down a transformation state.
                targetTransformation = Transformations.GetPreviousTransformationStep(player);
                isPoweringDownOneStep = true;
            }

            // if we made it this far without a target, it means for some reason we can't change transformations.
            if (targetTransformation == null)
                return;

            // finally, check that the transformation is really valid and then do it.
            if (Transformations.CanTransform(player, targetTransformation))
                Transformations.DoTransform(player, targetTransformation, mod, isPoweringDownOneStep);
        }

        public void HandleKaioken()
        {
            BuffInfo targetTransformation = null;
            bool isPoweringDownOneStep = false;
            if (KaiokenKey.JustPressed)
            {
                // no possible combination of kaioken can be attained in your current state.
                if (Transformations.IsPlayerTransformed(player) && !Transformations.IsSSJ1(player) && !Transformations.IsKaioken(player))
                    return;
                // otherwise get the next kaioken step (or the first one, if untransformed)
                targetTransformation = Transformations.GetNextKaiokenStep(player);
            }
            else if (IsPoweringDownOneStep() && (Transformations.IsKaioken(player) || Transformations.IsSSJ1Kaioken(player)))
            {
                // player is powering down their kaioken state, get the previous one.
                targetTransformation = Transformations.GetPreviousKaiokenStep(player);
                isPoweringDownOneStep = true;
            }

            if (targetTransformation == null)
                return;

            // finally, check the transformation is really valid and then do it.
            if (Transformations.CanTransform(player, targetTransformation))
                Transformations.DoTransform(player, targetTransformation, mod, isPoweringDownOneStep);
        }

        public void UpdateSynchronizedControls(TriggersSet triggerSet)
        {
            // this might look weird, but terraria reads these setters as changing the collection, which is bad.
            if (triggerSet.Left)
                IsLeftHeld = true;
            else
                IsLeftHeld = false;

            if (triggerSet.Right)
                IsRightHeld = true;
            else
                IsRightHeld = false;

            if (triggerSet.Up)
                IsUpHeld = true;
            else
                IsUpHeld = false;

            if (triggerSet.Down)
                IsDownHeld = true;
            else
                IsDownHeld = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            UpdateSynchronizedControls(triggersSet);

            SyncTriggerSet();

            if (FlyToggle.JustPressed)
            {
                if (flightUnlocked)
                {
                    IsFlying = !IsFlying;
                }
            }

            m_progressionSystem.Update(player);

            // dropping the fist wireup here. Fingers crossed.
            if (player.HeldItem.Name.Equals("Fist"))
            {
                m_fistSystem.Update(triggersSet, player, mod);
            }

            if (ArmorBonus.JustPressed)
            {
                if (DemonBonus && !player.HasBuff(mod.BuffType("ArmorCooldown")))
                {
                    player.AddBuff(mod.BuffType("DemonBonus"), 300);
                    DemonBonusActive = true;
                    for (int i = 0; i < 3; i++)
                    {
                        Dust tDust = Dust.NewDustDirect(player.position - (Vector2.UnitY * 0.7f) - (Vector2.UnitX * 1.0f), 50, 50, 15, 0f, 0f, 5, default(Color), 2.0f);
                    }
                }
            }

            // handle ki charging
            if (ConfigModel.IsChargeToggled)
            {
                if (EnergyCharge.JustPressed)
                {
                    IsCharging = !IsCharging;
                }
            }
            else
            {
                if (EnergyCharge.Current && !IsCharging)
                {
                    IsCharging = true;
                }
                if (!EnergyCharge.Current && IsCharging)
                {
                    IsCharging = false;
                }
            }

            // calls to handle transformation or kaioken powerups per frame

            HandleTransformations();

            HandleKaioken();

            if (SpeedToggle.JustPressed)
            {
                speedToggled = !speedToggled;
            }

            if (TransMenu.JustPressed)
            {
                UI.TransMenu.menuvisible = !UI.TransMenu.menuvisible;
            }

            /*if (ProgressionMenuKey.JustPressed)
            {
                ProgressionMenu.ToggleVisibility();
            }*/

            // power down handling
            if (IsCompletelyPoweringDown() && Transformations.IsPlayerTransformed(player))
            {
                Transformations.EndTransformations(player, true, false);

                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
            }
            /*if(QuickKi.JustPressed && traitChecked)
            {
                traitChecked = false;
                ChooseTrait();
                Main.NewText(playerTrait);
            }*/
        }

        public void HandleChargeEffects()
        {
            if (Math.Ceiling(Main.time) % 600 == 0)
            {
                if (player.whoAmI != Main.myPlayer)
                {
                    //DebugUtil.Log(string.Format("I think player {0} is... charging? {1}, ki? {2} / {3}, channeling? {4}, flying? {5}, stationary? {6}", player.whoAmI, IsCharging, GetKi(), OverallKiMax(), player.channel, IsFlying, isPlayerMostlyStationary));
                    //DebugUtil.Log(string.Format("Frag1 {0} Frag2 {1} Frag3 {2} Frag4 {3} Frag5 {4}", Fragment1, Fragment2, Fragment3, Fragment4, Fragment5));
                }
            }

            // various effects while charging
            // if the player is flying and moving, charging applies a speed boost and doesn't recharge ki, but also doesn't slow the player.
            bool isAnyKeyHeld = IsLeftHeld || IsRightHeld || IsUpHeld || IsDownHeld;
            if (IsCharging && (GetKi() < OverallKiMax()) && !player.channel && (!IsFlying || !isAnyKeyHeld))
            {
                // determine base regen rate and bonuses
                AddKi(KiChargeRate + ScarabChargeRateAdd);

                
                // slow down the player a bunch - only when not flying
                if ((IsLeftHeld || IsRightHeld) && !IsFlying)
                {
                    if (chargeMoveSpeed > 0)
                        player.velocity = new Vector2(chargeMoveSpeed * player.direction, player.velocity.Y);
                    else
                        player.velocity = new Vector2(0, player.velocity.Y);
                }

                // DebugUtil.Log(string.Format("I think player {0} is moving at {1}, {2}", player.whoAmI, player.velocity.X, player.velocity.Y));

                // grant multiplicative charge bonuses that grow over time if using either earthen accessories
                if (earthenScarab || earthenArcanium)
                {
                    ScarabChargeTimer++;
                    if (ScarabChargeTimer > 180 && ScarabChargeRateAdd <= 5)
                    {
                        ScarabChargeRateAdd += 1;
                        ScarabChargeTimer = 0;
                    }
                }

                // grant defense and a protective barrier visual if charging with baldur essentia
                if (baldurEssentia && !buldariumSigmite)
                {
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaldurShell"), 0, 0, player.whoAmI);
                    player.statDefense = (int)(player.statDefense * 1.30f);
                }
                if (buldariumSigmite)
                {
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaldurShell"), 0, 0, player.whoAmI);
                    player.statDefense = (int)(player.statDefense * 1.50f);
                    player.shinyStone = true;
                }
                if (burningEnergyAmulet)
                {
                    FireAura();
                    Projectile.NewProjectile(player.Center.X + 10, player.Center.Y - 20, 0, 0, mod.ProjectileType("FireAuraProj"), 1, 0, player.whoAmI);
                }
                if (iceTalisman)
                {
                    FrostAura();
                    Projectile.NewProjectile(player.Center.X + 10, player.Center.Y - 20, 0, 0, mod.ProjectileType("FrostAuraProj"), 1, 0, player.whoAmI);
                }
                if (pureEnergyCirclet)
                {
                    FireAura();
                    FrostAura();
                    Projectile.NewProjectile(player.Center.X + 10, player.Center.Y - 20, 0, 0, mod.ProjectileType("FireFrostAuraProj"), 1, 0, player.whoAmI);
                }
            }
            else if (!IsCharging)
            {
                // reset scarab/earthen bonuses
                ScarabChargeTimer = 0;
                ScarabChargeRateAdd = 0;
            }
        }

        public void HandleChargeVisualAndSoundEffects()
        {
            // sound effects during rapid flight or while charging are the same.
            if (IsCharging)
            {
                ChargeSoundTimer++;
                if (ChargeSoundTimer > 22)
                {
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyCharge").WithVolume(.5f));
                    ChargeSoundTimer = 0;
                }
            }
            if (IsCharging && !WasCharging)
            { 
                if (!Transformations.IsPlayerTransformed(player))
                {
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaseAuraProj"), 0, 0, player.whoAmI);
                }
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyChargeStart").WithVolume(.7f));
            }
        }
    
        public MyPlayer() : base()
        {
        }

        public override void ResetEffects()
        {
            KiDamage = 1f;
            KiKbAddition = 0f;
            if (KiEssence1)
            {
                KiChargeRate = 2;

                if (KiEssence2)
                {
                    KiChargeRate = 3;

                    if (KiEssence3)
                    {
                        KiChargeRate = 5;

                        if (KiEssence4)
                        {
                            KiChargeRate = 7;

                            if (KiEssence5)
                            {
                                KiChargeRate = 10;
                            }
                        }
                    }
                }
            }
            if (!KiEssence1 && !KiEssence2 && !KiEssence3 && !KiEssence4 && !KiEssence5)
            {
                KiChargeRate = 1;
            }
            scouterT2 = false;
            scouterT3 = false;
            scouterT4 = false;
            scouterT5 = false;
            scouterT6 = false;
            spiritualEmblem = false;
            turtleShell = false;
            KiDrainMulti = 1f;
            KiSpeedAddition = 1f;
            KiCrit = 5;
            ChlorophyteHeadPieceActive = false;
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
            KiOrbDropChance = 3;
            IsHoldingKiWeapon = false;
            wornGloves = false;
            senzuBag = false;
            palladiumBonus = false;
            adamantiteBonus = false;
            OrbGrabRange = 2;
            OrbHealAmount = 50;
            DemonBonus = false;
            blackFusionBonus = false;
            ChargeLimitAdd = 0;
            FlightSpeedAdd = 0;
            FlightUsageAdd = 0;
            KiRegen = 0;
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
            KaiokenDrainMulti = 1f;
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
            KiMax2 = 0;
            KiMaxMult = 1f;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (zenkaiCharm && !zenkaiCharmActive && !player.HasBuff(mod.BuffType("ZenkaiCooldown")))
            {
                player.statLife = 50;
                player.HealEffect(50);
                player.AddBuff(mod.BuffType("ZenkaiBuff"), 300);
                return false;
            }

            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && !SSJ1Achieved && player.whoAmI == Main.myPlayer && NPC.downedBoss3)
                {
                    if (RageCurrent >= 3)
                    {
                        OverallFormUnlockChance = 1;
                    }
                    else
                    {
                        FormUnlockChance = 20;
                    }
                    if ((Main.rand.Next(OverallFormUnlockChance) == 0))
                    {
                        Main.NewText("The humiliation of failing drives you mad.", Color.Yellow);
                        player.statLife = player.statLifeMax2 / 2;
                        player.HealEffect(player.statLifeMax2 / 2);
                        SSJ1Achieved = true;
                        IsTransforming = true;
                        SSJTransformation();
                        UI.TransMenu.MenuSelection = MenuSelectionID.SSJ1;
                        RageCurrent = 0;
                        Transformations.EndTransformations(player, true, false);
                        return false;
                    }
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && SSJ1Achieved && !SSJ2Achieved && player.whoAmI == Main.myPlayer && !IsPlayerLegendary() && NPC.downedMechBossAny && player.HasBuff(Transformations.SSJ1.BuffId) && MasteryLevel1 >= 1)
                {
                    Main.NewText("The rage of failing once more dwells deep within you.", Color.Red);
                    player.statLife = player.statLifeMax2 / 2;
                    player.HealEffect(player.statLifeMax2 / 2);
                    SSJ2Achieved = true;
                    IsTransforming = true;
                    SSJ2Transformation();
                    UI.TransMenu.MenuSelection = MenuSelectionID.SSJ2;
                    Transformations.EndTransformations(player, true, false);
                    RageCurrent = 0;
                    return false;
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && SSJ1Achieved && !LSSJAchieved && player.whoAmI == Main.myPlayer && IsPlayerLegendary() && NPC.downedMechBossAny && player.HasBuff(Transformations.SSJ1.BuffId) && MasteryLevel1 >= 1)
                {
                    Main.NewText("Your rage is overflowing, you feel something rise up from deep inside.", Color.Green);
                    player.statLife = player.statLifeMax2 / 2;
                    player.HealEffect(player.statLifeMax2 / 2);
                    LSSJAchieved = true;
                    IsTransforming = true;
                    LSSJTransformation();
                    UI.TransMenu.MenuSelection = MenuSelectionID.LSSJ1;
                    Transformations.EndTransformations(player, true, false);
                    RageCurrent = 0;
                    return false;
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if ((culprit.type == NPCID.Golem || culprit.type == NPCID.GolemFistLeft || culprit.type == NPCID.GolemFistRight || culprit.type == NPCID.GolemHead || culprit.type == NPCID.GolemHeadFree) && SSJ1Achieved && SSJ2Achieved && !SSJ3Achieved && !IsPlayerLegendary() && player.whoAmI == Main.myPlayer && player.HasBuff(Transformations.SSJ2.BuffId) && MasteryLevel2 >= 1)
                {
                    Main.NewText("The ancient power of the Lihzahrds seeps into you, causing your power to become unstable.", Color.Orange);
                    player.statLife = player.statLifeMax2 / 2;
                    player.HealEffect(player.statLifeMax2 / 2);
                    SSJ3Achieved = true;
                    IsTransforming = true;
                    SSJ3Transformation();
                    UI.TransMenu.MenuSelection = MenuSelectionID.SSJ3;
                    Transformations.EndTransformations(player, true, false);
                    RageCurrent = 0;
                    return false;
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && player.whoAmI == Main.myPlayer)
                {
                    RageCurrent += 1;
                    return true;
                }
            }

            return true;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (IsTransforming)
            {
                return false;
            }
            if (BlockState == 1) // nearly frame-perfect block, zero damage.
            {
                damage = 0;
                return true;
            }
            if (BlockState == 2) // block not quite perfect, one third damage
            {
                damage /= 3;
                return true;
            }
            if (BlockState == 3) // block far from perfect, half damage.
            {
                damage /= 2;
                return true;
            }
            if (ChlorophyteHeadPieceActive && !player.HasBuff(mod.BuffType("ChlorophyteRegen")))
            {
                player.AddBuff(mod.BuffType("ChlorophyteRegen"), 180);
                return true;
            }
            if (blackDiamondShell)
            {
                int i = Main.rand.Next(10, 100);
                AddKi(i);
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), i, false, false);
                return true;
            }
            return true;
        }

        public void SSJDustAura()
        {
            const float AURAWIDTH = 3.0f;

            for (int i = 0; i < 20; i++)
            {
                float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * AURAWIDTH))).X;
                float yPos = ((Vector2.UnitY * player.height) - (Vector2.UnitY * Main.rand.Next(0, player.height))).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(player.position + new Vector2(xPos, yPos), 1, 1, 87, 0f, 0f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

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
        public void FrostAura()
        {
            const float AURAWIDTH = 2f;

            for (int i = 0; i < 4; i++)
            {
                float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * AURAWIDTH))).X;
                float yPos = ((Vector2.UnitY * player.height) - (Vector2.UnitY * Main.rand.Next(0, player.height))).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(player.position + new Vector2(xPos, yPos), 1, 1, 59, 0f, 0f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

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
        public void FireAura()
        {
            const float AURAWIDTH = 2f;

            for (int i = 0; i < 4; i++)
            {
                float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * AURAWIDTH))).X;
                float yPos = ((Vector2.UnitY * player.height) - (Vector2.UnitY * Main.rand.Next(0, player.height))).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(player.position + new Vector2(xPos, yPos), 1, 1, 60, 0f, 0f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

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
        public void SSJTransformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJRockProjStart"), 0, 0, player.whoAmI);
            if (!Main.dedServ)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/GroundRumble").WithVolume(1f));
        }

        public void SSJ2Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJAuraBall"), 0, 0, player.whoAmI);
            if (!Main.dedServ)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Awakening").WithVolume(1f));
        }

        public void SSJ3Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJ3LightPillar"), 0, 0, player.whoAmI);
            if (!Main.dedServ)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Awakening").WithVolume(1f));
        }

        public void LSSJTransformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJAuraBall"), 0, 0, player.whoAmI);
            if (!Main.dedServ)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Awakening").WithVolume(1f));
        }

        public void LSSJ2Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("LSSJ2PillarStart"), 0, 0, player.whoAmI);
            if (!Main.dedServ)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Awakening").WithVolume(1f));
        }

        public void SSJGTransformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJGDustStart"), 0, 0, player.whoAmI);
            if (!Main.dedServ)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Awakening").WithVolume(1f));
        }

        public override void SetupStartInventory(IList<Item> items)
        {
            Item item8 = new Item();
            item8.SetDefaults(mod.ItemType("EmptyNecklace"));
            item8.stack = 1;
            items.Add(item8);
        }

        public int LightningFrameTimer;
        public static readonly PlayerLayer LightningEffects = new PlayerLayer("DBZMOD", "LightningEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("DBZMOD");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            int frame = modPlayer.LightningFrameTimer / 5;
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            if (drawPlayer.HasBuff(Transformations.SSJ2.BuffId))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningBlue");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
            if (drawPlayer.HasBuff(Transformations.LSSJ.BuffId) || drawPlayer.HasBuff(Transformations.LSSJ2.BuffId))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningGreen");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
            if (drawPlayer.HasBuff(Transformations.SSJ3.BuffId))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningYellow");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
            if (drawPlayer.HasBuff(Transformations.Kaioken100.BuffId) || drawPlayer.HasBuff(mod.BuffType("SSJ4Buff")))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningRed");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }

        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            //handle lightning effects
            LightningEffects.visible = true;
            layers.Add(LightningEffects);

            // handle SSJ hair/etc.
            int hair = layers.FindIndex(l => l == PlayerLayer.Hair);
            if (hair < 0)
                return;
            if (Hair != null)
            {
                layers[hair] = new PlayerLayer(mod.Name, "TransHair",
                    delegate (PlayerDrawInfo draw)
                    {
                        Player player = draw.drawPlayer;

                        Color alpha = draw.drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)(draw.position.X + draw.drawPlayer.width * 0.5) / 16, (int)((draw.position.Y + draw.drawPlayer.height * 0.25) / 16.0), Color.White), draw.shadow);
                        DrawData data = new DrawData(Hair, new Vector2((float)((int)(draw.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(draw.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.headPosition + draw.headOrigin, player.bodyFrame, alpha, player.headRotation, draw.headOrigin, 1f, draw.spriteEffects, 0);
                        data.shader = draw.hairShader;
                        Main.playerDrawData.Add(data);
                    });
            }

            if (Hair != null)
            {
                PlayerLayer.Head.visible = false;
                PlayerLayer.Hair.visible = false;
                PlayerLayer.HairBack.visible = false;
                PlayerHeadLayer.Hair.visible = false;
                PlayerHeadLayer.Head.visible = false;
                PlayerLayer.Arms.visible = false;
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (victim != player && victim.whoAmI != NPCID.TargetDummy)
            {
                float expierenceToAdd = 10.0f;
                float experienceMult = 1.0f;

                if (Transformations.IsPlayerTransformed(player))
                {
                    experienceMult = 2.0f;
                }

                m_progressionSystem.AddKiExperience(expierenceToAdd * experienceMult);
            }

            base.OnHitAnything(x, y, victim);
        }

        public override void PlayerConnect(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (player.whoAmI != Main.myPlayer)
                {
                    //DebugUtil.Log(string.Format("I am player {0}", Main.myPlayer));

                    //DebugUtil.Log(string.Format("I am requesting my information be sent to player {0}", player.whoAmI));
                    NetworkHelper.playerSync.SendPlayerInfoToPlayerFromOtherPlayer(player.whoAmI, Main.myPlayer);

                    //DebugUtil.Log(string.Format("I am sending a request to the server to ship me that player's data."));
                    NetworkHelper.playerSync.RequestPlayerSendTheirInfo(256, Main.myPlayer, player.whoAmI);
                }
            }
        }

        public Texture2D Hair;

        public override void PreUpdate()
        {
            if (Transformations.IsPlayerTransformed(player))
            {
                if (!player.armor[10].vanity && player.armor[10].headSlot == -1)
                {
                    if (player.HasBuff(Transformations.SSJ1.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ1Hair");
                    }
                    else if (player.HasBuff(Transformations.ASSJ.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/ASSJHair");
                    }
                    else if (player.HasBuff(Transformations.USSJ.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/USSJHair");
                    }
                    else if (player.HasBuff(Transformations.SSJ1Kaioken.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ1KaiokenHair");
                    }
                    else if (player.HasBuff(Transformations.SSJ2.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ2Hair");
                    }
                    else if (player.HasBuff(Transformations.SSJ3.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ3Hair");
                    }
                    else if (player.HasBuff(Transformations.LSSJ.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/LSSJ/LSSJHair");
                    }
                    else if (player.HasBuff(Transformations.LSSJ2.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/LSSJ/LSSJ2Hair");
                    }
                    else if (player.HasBuff(Transformations.SPECTRUM.BuffId))
                    {
                        Hair = mod.GetTexture("Hairs/Dev/SSJSHair");
                    }
                    if(player.HasBuff(Transformations.SSJG.BuffId))
                    {
                        Hair = null;
                    }
                }
            }
            else
            {
                Hair = null;
            }
            if (player.dead)
            {
                Hair = null;
            }
        }
    }
}