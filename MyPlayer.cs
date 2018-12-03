using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using DBZMOD.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics;
using Microsoft.Xna.Framework;
using DBZMOD.Projectiles;
using Terraria.ModLoader.IO;
using Terraria.ID;
using DBZMOD;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;
using Config;

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
        private int _kiMax;
        public int KiMax
        {
            get
            {
                if (_kiMax <= GetKiMaxFromFragments())
                {
                    _kiMax = GetKiMaxFromFragments();
                }
                return _kiMax;
            }
            set
            {
                _kiMax = value;
            }
        }

        // ki max 2 is ki max from equipment and accessories. there's no point changing this value as it gets reset each frame.
        public int KiMax2;

        // progression upgrades increase KiMax3. This is the only value that can be changed to have an impact on ki max and does not reset.
        public int KiMax3;

        // ki max mult is a multiplier for ki that stacks multiplicatively with other KiMaxMult bonuses. It resets to 1f each frame.
        public float KiMaxMult;

        public int KiCurrent;
        public int KiRegenRate = 1;
        public int OverloadMax = 100;
        public int OverloadCurrent;
        public float chargeMoveSpeed;

        //Transformation vars
        public bool IsTransforming;
        public int SSJAuraBeamTimer;
        public bool IsTransformed;
        public bool hasSSJ1;
        public int TransformCooldown;
        public bool ASSJAchieved;
        public bool USSJAchieved;
        public bool SSJ2Achieved;
        public bool SSJ3Achieved;
        public bool LSSJAchieved = false;
        public bool SSJGAchieved = false;
        private int lssj2timer;
        public bool LSSJ2Achieved = false;
        public bool LSSJGAchieved = false;
        public bool IsKaioken;
        public bool IsSSJ;
        public bool IsGodform;
        public bool IsLSSJ;
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
        private bool DemonBonusActive;
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
        public int ChargeSoundTimer;
        public int ChargeLimitAdd;
        //public static bool RealismMode = false;
        public static bool JungleMessage = false;
        public static bool HellMessage = false;
        public static bool EvilMessage = false;
        public static bool MushroomMessage = false;
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
        private int ScarabChargeRateAdd;
        private int ScarabChargeTimer;
        public bool flightUnlocked = false;
        public bool flightDampeningUnlocked = false;
        public bool flightUpgraded = false;
        private int DemonBonusTimer;
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
        public SoundEffectInstance transformationSound;
        #endregion

        #region Classes
        FlightSystem m_flightSystem = new FlightSystem();
        ProgressionSystem m_progressionSystem = new ProgressionSystem();
        FistSystem m_fistSystem = new FistSystem();
        #endregion

        // overall ki max is now just a formula representing your total ki, after all bonuses are applied.
        public int OverallKiMax()
        {
            return (int)Math.Ceiling((KiMax + KiMax2 + KiMax3) * KiMaxMult * (player.HasBuff(mod.BuffType("LegendaryTrait")) ? 2f : 1f));
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

        public override void PostUpdate()
        {
            if (LSSJAchieved && !LSSJ2Achieved && player.whoAmI == Main.myPlayer && playerTrait == "Legendary" && NPC.downedFishron && player.statLife <= (player.statLifeMax2 * 0.10))
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
                        UI.TransMenu.MenuSelection = 6;
                        lssj2timer = 0;
                        EndTransformations();
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
                player.AddBuff(mod.BuffType("KiLanternBuff"), 18000);
            }
            else
            {
                player.ClearBuff(mod.BuffType("KiLanternBuff"));
            }
            if (IsTransforming)
            {
                SSJAuraBeamTimer++;
            }
            if (KiCurrent < 0)
            {
                KiCurrent = 0;
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
            if (IsTransformed && !player.HasBuff(mod.BuffType("LSSJ2Buff")) || player.HasBuff(mod.BuffType("KaiokenBuffX100")))
            {
                LightningFrameTimer++;
            }
            if (player.HasBuff(mod.BuffType("LSSJ2Buff")))
            {
                LightningFrameTimer += 2;
            }
            if (LightningFrameTimer >= 15)
            {
                LightningFrameTimer = 0;
            }
            if (!IsTransformed)
            {
                KiDrainAddition = 0;
            }
            if (IsKaioken)
            {
                KaiokenTimer += 1.5f;
            }
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


            if ((player.HasBuff(mod.BuffType("SSJ1Buff"))
                &&
                (player.HasBuff(mod.BuffType("KaiokenBuffX3")) || player.HasBuff(mod.BuffType("KaiokenBuffX10")) || player.HasBuff(mod.BuffType("KaiokenBuffX20")) || player.HasBuff(mod.BuffType("KaiokenBuffX100")))))
            {
                player.ClearBuff(mod.BuffType("SSJ1Buff"));
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX3"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX10"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX20"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX100"));
                IsTransformed = false;
                Main.NewText("Your body can't sustain that combination.", new Color(255, 25, 79));
            }
            if ((player.HasBuff(mod.BuffType("SSJ2Buff"))
                || player.HasBuff(mod.BuffType("SSJ3Buff"))
                || player.HasBuff(mod.BuffType("ASSJBuff"))
                || player.HasBuff(mod.BuffType("USSJBuff"))
                || player.HasBuff(mod.BuffType("LSSJBuff"))
                || player.HasBuff(mod.BuffType("LSSJ2Buff"))
                || player.HasBuff(mod.BuffType("SSJGBuff")))
                &&
                (player.HasBuff(mod.BuffType("KaiokenBuff")) || player.HasBuff(mod.BuffType("KaiokenBuffX3")) || player.HasBuff(mod.BuffType("KaiokenBuffX10")) || player.HasBuff(mod.BuffType("KaiokenBuffX20")) || player.HasBuff(mod.BuffType("KaiokenBuffX100"))))
            {
                EndTransformations();
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX3"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX10"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX20"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX100"));
                Main.NewText("Your body can't sustain that combination.", new Color(255, 25, 79));
            }
            if (adamantiteBonus)
            {
                KiDamage += 7;
            }

            if (!traitChecked)
            {
                ChooseTrait();
            }
            
            #region Transformational Checks
            //kaioken
            if (player.HasBuff(mod.BuffType("KaiokenBuff")) || player.HasBuff(mod.BuffType("KaiokenBuffX3")) || player.HasBuff(mod.BuffType("KaiokenBuffX10")) || player.HasBuff(mod.BuffType("KaiokenBuffX20")) || player.HasBuff(mod.BuffType("KaiokenBuffX100")))
            {
                IsKaioken = true;
            }
            else
            {
                IsKaioken = false;
            }
            //SSJ1-3-G
            if (player.HasBuff(mod.BuffType("SSJ1Buff")) || player.HasBuff(mod.BuffType("SSJ2Buff")) || player.HasBuff(mod.BuffType("ASSJBuff")) || player.HasBuff(mod.BuffType("USSJBuff")) || player.HasBuff(mod.BuffType("SSJ3Buff")))
            {
                IsSSJ = true;
            }
            else
            {
                IsSSJ = false;
            }
            if (player.HasBuff(mod.BuffType("LSSJBuff")) || player.HasBuff(mod.BuffType("LSSJ2Buff")))
            {
                IsLSSJ = true;
            }
            else
            {
                IsLSSJ = false;
            }

            if (player.HasBuff(mod.BuffType("SSJGBuff")))
            {
                IsGodform = true;
            }
            else
            {
                IsGodform = false;
            }
            if (!player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                if (IsSSJ && IsKaioken)
                {
                    EndTransformations();
                    Main.NewText("Your body can't sustain that combination.", new Color(255, 25, 79));
                }
            }
            if (player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                if (player.HasBuff(mod.BuffType("SSJ2Buff")) || player.HasBuff(mod.BuffType("ASSJBuff")) || player.HasBuff(mod.BuffType("USSJBuff")) || player.HasBuff(mod.BuffType("SSJ3Buff")) || player.HasBuff(mod.BuffType("SSJGBuff")) || IsLSSJ)
                {
                    EndTransformations();
                    Main.NewText("Your body can't sustain that combination.", new Color(255, 25, 79));
                }
            }
            if (IsSSJ && IsLSSJ)
            {
                EndTransformations();
                Main.NewText("Your body can't sustain that combination.", new Color(255, 25, 79));
            }
            if (IsLSSJ && IsKaioken)
            {
                EndTransformations();
                Main.NewText("Your body can't sustain that combination.", new Color(255, 25, 79));
            }
            #endregion

            if (LSSJAchieved)
            {
                UI.TransMenu.LSSJOn = true;
            }
            if (playerTrait == "Legendary" && !LSSJAchieved && NPC.downedBoss1)
            {
                player.AddBuff(mod.BuffType("UnknownLegendary"), 3);
            }
            else if (playerTrait == "Legendary" && LSSJAchieved)
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
                if (KiCurrent != OverallKiMax())
                {
                    KiCurrent += KiRegen;
                }
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
            if (player.dead)
            {
                EndTransformations();
                IsTransformed = false;
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
            if(IsLSSJ)
            {
                OverloadCurrent++;
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
        }

        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (player.HasBuff(mod.BuffType("SSJGBuff")))
            {
                drawInfo.hairColor = new Color(183, 25, 46);
                drawInfo.hairShader = 1;
                player.eyeColor = Color.Red;
            }
            if (IsSSJ || IsLSSJ)
            {
                player.eyeColor = Color.Turquoise;
            }
            if (player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
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
                KiCurrent += i;
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
                KiCurrent += i;
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), i, false, false);
                if (Main.rand.Next(3) == 0)
                {
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 20, mod.ProjectileType("RadiantSpark"), (int)KiDamage * 100, 0, player.whoAmI);
                }
            }
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }

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
            tag.Add("KiRegenRate", KiRegenRate);
            tag.Add("KiEssence1", KiEssence1);
            tag.Add("KiEssence2", KiEssence2);
            tag.Add("KiEssence3", KiEssence3);
            tag.Add("KiEssence4", KiEssence4);
            tag.Add("KiEssence5", KiEssence5);
            tag.Add("MenuSelection", UI.TransMenu.MenuSelection);
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
            KiRegenRate = tag.Get<int>("KiRegenRate");
            KiEssence1 = tag.Get<bool>("KiEssence1");
            KiEssence2 = tag.Get<bool>("KiEssence2");
            KiEssence3 = tag.Get<bool>("KiEssence3");
            KiEssence4 = tag.Get<bool>("KiEssence4");
            KiEssence5 = tag.Get<bool>("KiEssence5");
            UI.TransMenu.MenuSelection = tag.Get<int>("MenuSelection");
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

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            m_flightSystem.Update(triggersSet, player);

            m_progressionSystem.Update(triggersSet, player);

            // dropping the fist wireup here. Fingers crossed.
            if (player.HeldItem.Name.Equals("Fist"))
            {
                m_fistSystem.Update(triggersSet, player, mod);
            }

            if (FlyToggle.JustPressed)
            {
                if (flightUnlocked)
                {
                    m_flightSystem.ToggleFlight(player, mod);
                }
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

            if (Transform.JustPressed)//Needs to be reworked, something method based
            {
                if (transformationSound != null)
                {
                    transformationSound.Stop();
                    transformationSound = null;
                }

                if (!IsSSJ && !IsGodform && !IsLSSJ && !IsKaioken && SSJ1Achieved && UI.TransMenu.MenuSelection == 1 && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    SSJDustAura();
                    player.AddBuff(mod.BuffType("SSJ1Buff"), 666666, false);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ1AuraProjStart"), 0, 0, player.whoAmI);
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Super Saiyan 1", false, false);
                }
                else if (player.HasBuff(mod.BuffType("SSJ1Buff")) && IsCharging && ASSJAchieved && !IsTransforming && !player.channel && (UI.TransMenu.MenuSelection == 1))
                {
                    SSJDustAura();
                    player.AddBuff(mod.BuffType("ASSJBuff"), 666666, false);
                    player.ClearBuff(mod.BuffType("SSJ1Buff"));
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(1.0f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Ascended Super Saiyan", false, false);
                }
                else if (player.HasBuff(mod.BuffType("ASSJBuff")) && IsCharging && USSJAchieved && !IsTransforming && !player.channel && (UI.TransMenu.MenuSelection == 1))
                {
                    SSJDustAura();
                    player.AddBuff(mod.BuffType("USSJBuff"), 666666, false);
                    player.ClearBuff(mod.BuffType("ASSJBuff"));
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Ultra Super Saiyan", false, false);
                }
                else if (!IsSSJ && !IsLSSJ && !IsGodform && !IsKaioken && SSJ2Achieved && UI.TransMenu.MenuSelection == 2 && !IsTransforming && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    SSJDustAura();
                    player.AddBuff(mod.BuffType("SSJ2Buff"), 666666, false);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ2AuraProj"), 0, 0, player.whoAmI);
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Super Saiyan 2", false, false);
                }
                else if (!IsLSSJ && !IsSSJ && !IsGodform && !IsKaioken && LSSJAchieved && UI.TransMenu.MenuSelection == 4 && !IsTransforming && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    player.AddBuff(mod.BuffType("LSSJBuff"), 666666, false);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("LSSJAuraProj"), 0, 0, player.whoAmI);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Legendary Super Saiyan", false, false);
                }
                else if (!IsSSJ && !IsLSSJ && !IsGodform && !IsKaioken && SSJ3Achieved && UI.TransMenu.MenuSelection == 3 && !IsTransforming && !IsTransforming && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    SSJDustAura();
                    player.AddBuff(mod.BuffType("SSJ3Buff"), 666666, false);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ3AuraProj"), 0, 0, player.whoAmI);

                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Super Saiyan 3", false, false);
                }
                else if (player.HasBuff(mod.BuffType("SSJ1Buff")) && !IsLSSJ && !IsGodform && !IsKaioken && !IsCharging && SSJ2Achieved && !IsTransforming && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    SSJDustAura();
                    player.AddBuff(mod.BuffType("SSJ2Buff"), 666666, false);
                    player.ClearBuff(mod.BuffType("SSJ1Buff"));
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ2AuraProj"), 0, 0, player.whoAmI);
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Super Saiyan 2", false, false);
                }
                else if (player.HasBuff(mod.BuffType("SSJ2Buff")) && !IsKaioken && !IsGodform && !IsLSSJ && !IsCharging && SSJ3Achieved && !IsTransforming && !IsTransforming && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    SSJDustAura();
                    player.AddBuff(mod.BuffType("SSJ3Buff"), 666666, false);
                    player.ClearBuff(mod.BuffType("SSJ2Buff"));
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ3AuraProj"), 0, 0, player.whoAmI);
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Super Saiyan 3", false, false);
                }
                if (!IsSSJ && !IsLSSJ && !IsGodform && !IsKaioken && SSJGAchieved && UI.TransMenu.MenuSelection == 5 && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    player.AddBuff(mod.BuffType("SSJGBuff"), 666666, false);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJGTransformStart"), 0, 0, player.whoAmI);
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(229, 20, 51), "Super Saiyan God", false, false);
                }
                if (!IsLSSJ && !IsKaioken && !IsGodform && !IsSSJ && LSSJ2Achieved && UI.TransMenu.MenuSelection == 6 && !IsTransforming && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("TransExhaustionBuff")))
                {
                    player.AddBuff(mod.BuffType("LSSJ2Buff"), 666666, false);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("LSSJ2AuraProj"), 0, 0, player.whoAmI);
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Legendary Super Saiyan 2", false, false);
                }
                /*else if (player.HasBuff(mod.BuffType("LSSJBuff")) && LSSJ2Achieved && !IsTransformingSSJ1 && !IsTransformingLSSJ2 && !player.channel && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("KaiokenBuffX3")) && !player.HasBuff(mod.BuffType("KaiokenBuffX10")) && !player.HasBuff(mod.BuffType("KaiokenBuffX20")) && !player.HasBuff(mod.BuffType("KaiokenBuffX100")) && !player.HasBuff(mod.BuffType("ASSJBuff")) && (!player.HasBuff(mod.BuffType("SSJGBuff"))) && !player.HasBuff(mod.BuffType("USSJBuff")) && !player.HasBuff(mod.BuffType("TransExhaustionBuff"))))
                {
                    player.AddBuff(mod.BuffType("LSSJ2Buff"), 666666, false);
                    player.ClearBuff(mod.BuffType("LSSJBuff"));
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("LSSJ2AuraProj"), 0, 0, player.whoAmI);
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                    CombatText.NewText(player.Hitbox, new Color(219, 219, 48), "Legendary Super Saiyan 2", false, false);
                }*/
            }

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

            if (KaiokenKey.JustPressed && (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("KaiokenBuffX3")) && !player.HasBuff(mod.BuffType("KaiokenBuffX10")) && !player.HasBuff(mod.BuffType("KaiokenBuffX20")) && !player.HasBuff(mod.BuffType("KaiokenBuffX100"))) && !player.HasBuff(mod.BuffType("TiredDebuff")) && !player.HasBuff(mod.BuffType("SSJ1Buff")) && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && !player.HasBuff(mod.BuffType("SSJ2Buff")) && !player.HasBuff(mod.BuffType("SSJ3Buff")) && !player.HasBuff(mod.BuffType("LSSJBuff")) && KaioAchieved && !player.channel)
            {
                player.AddBuff(mod.BuffType("KaiokenBuff"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProj"), 0, 0, player.whoAmI);
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraStart").WithVolume(.5f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuff"))) && KaioFragment1)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.AddBuff(mod.BuffType("KaiokenBuffX3"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx3"), 0, 0, player.whoAmI);
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.6f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuffX3"))) && KaioFragment2)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuffX3"));
                player.AddBuff(mod.BuffType("KaiokenBuffX10"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx10"), 0, 0, player.whoAmI);
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.6f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuffX10"))) && KaioFragment3)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuffX10"));
                player.AddBuff(mod.BuffType("KaiokenBuffX20"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx20"), 0, 0, player.whoAmI);
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.7f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuffX20"))) && KaioFragment4)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuffX20"));
                player.AddBuff(mod.BuffType("KaiokenBuffX100"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx100"), 0, 0, player.whoAmI);
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.8f));
            }
            else if (KaiokenKey.JustPressed && player.HasBuff(mod.BuffType("SSJ1Buff")) && !IsLSSJ && !IsGodform && KaioAchieved && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && !player.HasBuff(mod.BuffType("TiredDebuff")))
            {
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.ClearBuff(mod.BuffType("SSJ1Buff"));
                player.AddBuff(mod.BuffType("SSJ1KaiokenBuff"), 1800);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProj"), 0, 0, player.whoAmI);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ1AuraProj"), 0, 0, player.whoAmI);
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.8f));
            }
            bool isPlayerMostlyStationary = Math.Abs(player.velocity.X) <= 6F && Math.Abs(player.velocity.Y) <= 6F;
            if (IsCharging && (KiCurrent < OverallKiMax()) && !player.channel && (!IsFlying || isPlayerMostlyStationary))
            {
                KiCurrent += KiRegenRate + ScarabChargeRateAdd;
                if (chargeMoveSpeed > 0 && (triggersSet.Left || triggersSet.Right))
                    player.velocity = new Vector2(chargeMoveSpeed * player.direction, player.velocity.Y);
                else
                {
                    player.velocity = new Vector2(0, player.velocity.Y);
                }
                ChargeSoundTimer++;
                if (ChargeSoundTimer > 22)
                {
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyCharge").WithVolume(.5f));
                    ChargeSoundTimer = 0;
                }
                if (earthenScarab || earthenArcanium)
                {
                    ScarabChargeTimer++;
                    if (ScarabChargeTimer > 180 && ScarabChargeRateAdd <= 5)
                    {
                        ScarabChargeRateAdd += 1;
                        ScarabChargeTimer = 0;
                    }
                }
                if (baldurEssentia)
                {
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaldurShell"), 0, 0, player.whoAmI);
                    player.statDefense = (int)(player.statDefense * 1.30f);
                }

            }
            else if (!IsCharging)
            {
                ScarabChargeTimer = 0;
                ScarabChargeRateAdd = 0;
            }
            if (IsCharging && IsFlying)
            {
                ChargeSoundTimer++;
                if (ChargeSoundTimer > 22)
                {
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyCharge").WithVolume(.5f));
                    ChargeSoundTimer = 0;
                }
            }
            if (KiCurrent > OverallKiMax())
            {
                KiCurrent = OverallKiMax();
            }
            if (ConfigModel.IsChargeToggled)
            {
                if (EnergyCharge.JustPressed)
                {
                    IsCharging = !IsCharging;
                    if (IsCharging)
                    {
                        if (!IsTransformed)
                        {
                            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaseAuraProj"), 0, 0, player.whoAmI);
                        }
                        if (!Main.dedServ)
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyChargeStart").WithVolume(.7f));                        
                    }
                }
            }
            else
            {
                if (EnergyCharge.JustPressed)
                {
                    if (!IsTransformed)
                    {
                        Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaseAuraProj"), 0, 0, player.whoAmI);
                    }
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyChargeStart").WithVolume(.7f));
                    IsCharging = true;
                }
                if (EnergyCharge.JustReleased)
                {
                    IsCharging = false;
                }
            }

            if (PowerDown.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuff")) || player.HasBuff(mod.BuffType("KaiokenBuffX3")) || player.HasBuff(mod.BuffType("KaiokenBuffX10")) || player.HasBuff(mod.BuffType("KaiokenBuffX20")) || player.HasBuff(mod.BuffType("KaiokenBuffX100"))))
            {
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX3"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX10"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX20"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX100"));
                player.AddBuff(mod.BuffType("TiredDebuff"), (int)KaiokenTimer);
                KaiokenTimer = 0.0f;
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
                IsTransformed = false;
            }
            if (PowerDown.JustPressed && player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                player.ClearBuff(mod.BuffType("SSJ1KaiokenBuff"));
                player.AddBuff(mod.BuffType("TiredDebuff"), (int)(KaiokenTimer * 2));
                player.AddBuff(mod.BuffType("TransExhaustionBuff"), 600);
                KaiokenTimer = 0.0f;
                if (!Main.dedServ)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
                IsTransformed = false;
            }
            if (PowerDown.JustPressed && (player.HasBuff(mod.BuffType("SSJ1Buff")) || player.HasBuff(mod.BuffType("SSJ2Buff")) || player.HasBuff(mod.BuffType("LSSJBuff")) || player.HasBuff(mod.BuffType("ASSJBuff")) || player.HasBuff(mod.BuffType("USSJBuff")) || player.HasBuff(mod.BuffType("SSJ3Buff")) || player.HasBuff(mod.BuffType("SSJGBuff")) || player.HasBuff(mod.BuffType("LSSJ2Buff"))))
            {
                EndTransformations();
            }
            /*if(QuickKi.JustPressed && traitChecked)
            {
                traitChecked = false;
                ChooseTrait();
                Main.NewText(playerTrait);
            }*/
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
                KiRegenRate = 2;

                if (KiEssence2)
                {
                    KiRegenRate = 3;

                    if (KiEssence3)
                    {
                        KiRegenRate = 5;

                        if (KiEssence4)
                        {
                            KiRegenRate = 7;

                            if (KiEssence5)
                            {
                                KiRegenRate = 10;
                            }
                        }
                    }
                }
            }
            if (!KiEssence1 && !KiEssence2 && !KiEssence3 && !KiEssence4 && !KiEssence5)
            {
                KiRegenRate = 1;
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
            KiMax = GetKiMaxFromFragments();
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
                    if(RageCurrent >= 3)
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
                        UI.TransMenu.MenuSelection = 1;
                        RageCurrent = 0;
                        EndTransformations();
                        return false;
                    }
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && SSJ1Achieved && !SSJ2Achieved && player.whoAmI == Main.myPlayer && !(playerTrait == "Legendary") && NPC.downedMechBossAny && player.HasBuff(mod.BuffType("SSJ1Buff")) && MasteryLevel1 >= 1)
                {
                    Main.NewText("The rage of failing once more dwells deep within you.", Color.Red);
                    player.statLife = player.statLifeMax2 / 2;
                    player.HealEffect(player.statLifeMax2 / 2);
                    SSJ2Achieved = true;
                    IsTransforming = true;
                    SSJ2Transformation();
                    UI.TransMenu.MenuSelection = 2;
                    EndTransformations();
                    RageCurrent = 0;
                    return false;
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && SSJ1Achieved && !LSSJAchieved && player.whoAmI == Main.myPlayer && playerTrait == "Legendary" && NPC.downedMechBossAny && player.HasBuff(mod.BuffType("SSJ1Buff")) && MasteryLevel1 >= 1)
                {
                    Main.NewText("Your rage is overflowing, you feel something rise up from deep inside.", Color.Green);
                    player.statLife = player.statLifeMax2 / 2;
                    player.HealEffect(player.statLifeMax2 / 2);
                    LSSJAchieved = true;
                    IsTransforming = true;
                    LSSJTransformation();
                    UI.TransMenu.MenuSelection = 4;
                    EndTransformations();
                    RageCurrent = 0;
                    return false;
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if ((culprit.type == NPCID.Golem || culprit.type == NPCID.GolemFistLeft || culprit.type == NPCID.GolemFistRight || culprit.type == NPCID.GolemHead || culprit.type == NPCID.GolemHeadFree) && SSJ1Achieved && SSJ2Achieved && !SSJ3Achieved && !(playerTrait == "Legendary") && player.whoAmI == Main.myPlayer && player.HasBuff(mod.BuffType("SSJ2Buff")) && MasteryLevel2 >= 1)
                {
                    Main.NewText("The ancient power of the Lihzahrds seeps into you, causing your power to become unstable.", Color.Orange);
                    player.statLife = player.statLifeMax2 / 2;
                    player.HealEffect(player.statLifeMax2 / 2);
                    SSJ3Achieved = true;
                    IsTransforming = true;
                    SSJ3Transformation();
                    UI.TransMenu.MenuSelection = 3;
                    EndTransformations();
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
            if(BlockState == 1)
            {
                damage = 0;
                return true;
            }
            if(BlockState == 2)
            {
                damage /= 2;
                return true;
            }
            if(BlockState == 3)
            {
                damage /= 3;
                return true;
            }
            if (ChlorophyteHeadPieceActive && !player.HasBuff(mod.BuffType("ChlorophyteRegen")))
            {
                player.AddBuff(mod.BuffType("ChlorophyteRegen"), 180);
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
            if (drawPlayer.HasBuff(mod.BuffType("SSJ2Buff")))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningBlue");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
            if (drawPlayer.HasBuff(mod.BuffType("LSSJBuff")) || drawPlayer.HasBuff(mod.BuffType("LSSJ2Buff")))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningGreen");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
            if (drawPlayer.HasBuff(mod.BuffType("SSJ3Buff")))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningYellow");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
            if (drawPlayer.HasBuff(mod.BuffType("KaiokenBuffX100")) || drawPlayer.HasBuff(mod.BuffType("SSJ4Buff")))
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
            LightningEffects.visible = true;
            layers.Add(LightningEffects);
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (victim != player && victim.whoAmI != NPCID.TargetDummy)
            {
                float expierenceToAdd = 10.0f;
                float experienceMult = 1.0f;

                if (IsTransformed)
                {
                    experienceMult = 2.0f;
                }

                m_progressionSystem.AddKiExperience(expierenceToAdd * experienceMult);
            }

            base.OnHitAnything(x, y, victim);
        }

        public void EndTransformations()
        {
            player.ClearBuff(mod.BuffType("SSJ1Buff"));
            player.ClearBuff(mod.BuffType("SSJ1KaiokenBuff"));
            player.ClearBuff(mod.BuffType("SSJ2Buff"));
            player.ClearBuff(mod.BuffType("ASSJBuff"));
            player.ClearBuff(mod.BuffType("USSJBuff"));
            player.ClearBuff(mod.BuffType("SSJ3Buff"));
            player.ClearBuff(mod.BuffType("LSSJBuff"));
            player.ClearBuff(mod.BuffType("LSSJ2Buff"));
            player.ClearBuff(mod.BuffType("SSJGBuff"));
            player.AddBuff(mod.BuffType("TransExhaustionBuff"), 600);
            if (!Main.dedServ)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));

            if (transformationSound != null)
            {
                transformationSound.Stop();
                transformationSound = null;
            }
            IsTransforming = false;

            IsTransformed = false;
        }

    }

    public class SSJHairDraw : ModPlayer
    {
        public Texture2D Hair;
        public static SSJHairDraw ModPlayer(Player player)
        {
            return player.GetModPlayer<SSJHairDraw>();
        }
        public override void PreUpdate()
        {
            if (player.GetModPlayer<MyPlayer>().IsTransformed)
            {
                if (!player.armor[10].vanity && player.armor[10].headSlot == -1)
                {
                    if (player.HasBuff(mod.BuffType("SSJ1Buff")))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ1Hair");
                    }
                    else if (player.HasBuff(mod.BuffType("ASSJBuff")))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/ASSJHair");
                    }
                    else if (player.HasBuff(mod.BuffType("USSJBuff")))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/USSJHair");
                    }
                    else if (player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ1KaiokenHair");
                    }
                    else if (player.HasBuff(mod.BuffType("SSJ2Buff")))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ2Hair");
                    }
                    else if (player.HasBuff(mod.BuffType("SSJ3Buff")))
                    {
                        Hair = mod.GetTexture("Hairs/SSJ/SSJ3Hair");
                    }
                    else if (player.HasBuff(mod.BuffType("LSSJBuff")))
                    {
                        Hair = mod.GetTexture("Hairs/LSSJ/LSSJHair");
                    }
                    else if (player.HasBuff(mod.BuffType("LSSJ2Buff")))
                    {
                        Hair = mod.GetTexture("Hairs/LSSJ/LSSJ2Hair");
                    }
                }
            }
            else
            {
                Hair = null;
            }
            if(player.dead)
            {
                Hair = null;
            }
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            
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
        public override void clientClone(ModPlayer clientClone)
        {
            SSJHairDraw clone = clientClone as SSJHairDraw;

            clone.Hair = Hair;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            SSJHairDraw clone = clientPlayer as SSJHairDraw;
            var packet = mod.GetPacket();
            if (clone.Hair != Hair)
            {
                packet.Send();
            }
        }
    }

}