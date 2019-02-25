using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using DBZMOD.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;
using DBZMOD.Models;
using DBZMOD.Enums;
using DBZMOD.Extensions;
using DBZMOD.Network;
using DBZMOD.Projectiles;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using DBZMOD.Traits;
using DBZMOD.Traits.Primal;

namespace DBZMOD
{
    // TODO Change this class name.
    public partial class MyPlayer : ModPlayer
    {
        #region Variables

        public float kiDamage;
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

        // helper fields to track whether the player is charging/using a charge attack, this is currently only for display purposes.
        public bool isPlayerUsingKiWeapon = false;
        public float currentKiAttackChargeLevel = 0f;
        public float currentKiAttackMaxChargeLevel = 0f;

        public int kiChargeRate = 1;
        public int overloadMax = 100;
        public int overloadCurrent;
        public int overloadTimer;
        public float chargeMoveSpeed;


        //Support Subclass vars
        
        //multiplier on healing, resets to 1f each frame.
        public float healMulti = 1f;

        //Transformation vars
        public bool isTransforming;
        public int ssjAuraBeamTimer;
        public int transformCooldown;

        public bool hasSSJ1;

        public int lssj2Timer;
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
        public Dictionary<TransformationDefinition, bool> masteryMessagesDisplayed = new Dictionary<TransformationDefinition, bool>();

        //Wish vars
        public const int POWER_WISH_MAXIMUM = 5;
        public int powerWishesLeft = 5;
        public int immortalityWishesLeft = 1;
        public int skillWishesLeft = 3;
        public int awakeningWishesLeft = 3;
        public int immortalityRevivesLeft = 0;

        //unsorted vars
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
        public bool demonBonus;
        public int orbGrabRange;
        public int orbHealAmount;
        public bool isFlying;

        public float flightKiConsumptionMultiplier;
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
        //public int kiDrainAddition;
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
		public bool metamoranSet;
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

        // player hitstun manager
        private HitStunManager hitStunManager = new HitStunManager();

        public Texture2D hair;

        #endregion

        #region Syncable Controls
        public bool isMouseRightHeld = false;
        public bool isMouseLeftHeld = false;
        public bool isLeftHeld = false;
        public bool isRightHeld = false;
        public bool isUpHeld = false;
        public bool isDownHeld = false;
        #endregion

        ProgressionSystem _mProgressionSystem = new ProgressionSystem();
        FistSystem _mFistSystem = new FistSystem();

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
        
        // TODO Change these
        public void HandleKiDrainMasteryContribution(float kiAmount, bool isWeaponDrain, bool isFormDrain)
        {
            if (isFormDrain)
            {
                TransformationDefinition transformation = GetCurrentTransformation();
                if (transformation == null) return;

                if (transformation.GetKiDrainRate(this) == 0 && transformation.GetKiDrainRateMastery(this) == 0)
                    return;

                //masteryLevels[transformation.MasteryBuffKeyName] = 0;
                PlayerTransformation playerTransformation = PlayerTransformations[transformation];

                playerTransformation.Mastery = GetMasteryIncreaseFromFormDrain(playerTransformation.Mastery);
                transformation.OnMasteryGained(this, playerTransformation.Mastery);
            }

            if (isWeaponDrain && kiAmount < 0)
            {
                TransformationDefinition transformation = GetCurrentTransformation();
                if (transformation == null) return;

                PlayerTransformation playerTransformation = PlayerTransformations[transformation];

                playerTransformation.Mastery = GetMasteryIncreaseFromWeaponDrain(playerTransformation.Mastery, kiAmount);
                transformation.OnMasteryGained(this, playerTransformation.Mastery);
            }
        }

        // currently doesn't use the damage received param. Included just in case.
        public void HandleDamageReceivedMastery(int damageReceived)
        {
            TransformationDefinition transformation = GetCurrentTransformation();
            if (transformation == null) return;

            PlayerTransformations[transformation].Mastery = GetMasteryIncreaseFromDamageTaken(PlayerTransformations[transformation].Mastery);
            transformation.OnMasteryGained(this, PlayerTransformations[transformation].Mastery);
        }

        private const float DAMAGE_TAKEN_MASTERY_CONTRIBUTION = 0.00232f;
        public float GetMasteryIncreaseFromDamageTaken(float currentMastery)
        {
          return Math.Min(1.0f, currentMastery + DAMAGE_TAKEN_MASTERY_CONTRIBUTION);
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

        public bool IsKiDepleted(float projectedKiDrain = 0f)
        {
            return _kiCurrent - projectedKiDrain <= 0;
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
            kiDamage *= PowerWishMulti();

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

        // TODO Untangle this
        public void HandleMasteryEvents(string masteryFormBuffKeyName)
        {
            TransformationDefinition transformation = GetCurrentTransformation();

            if (transformation == null || !masteryMessagesDisplayed.ContainsKey(transformation))
                return;

            PlayerTransformation playerTransformation = PlayerTransformations[transformation];

            bool isMessageDisplayed = masteryMessagesDisplayed[transformation];
            string masteryMessage = string.Empty;

            Color messageColor = new Color(232, 242, 50);

            if (masteryFormBuffKeyName.Equals(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition.UnlocalizedName))
            {
                if (playerTransformation.Mastery >= 0.5f && !ASSJAchieved)
                {
                    DBZMOD.Instance.TransformationDefinitionManager.ASSJDefinition.TryUnlock(this);
                    masteryMessage = $"Your SSJ1 Mastery has been upgraded.\nHold charge and transform while in SSJ1\nto ascend.";
                }
                else if (playerTransformation.Mastery >= 0.75f && !USSJAchieved)
                {
                    DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition.Unlock(this);
                    masteryMessage = $"Your SSJ1 Mastery has been upgraded.\nHold charge and transform while in ASSJ\nto ascend.";
                }
            }

            if (!string.IsNullOrEmpty(masteryMessage))
            {
                Main.NewText(masteryMessage, messageColor);
            }

            // handle general mastery messages here
            if (playerTransformation.Mastery >= 1.0f && !isMessageDisplayed)
            {
                masteryMessagesDisplayed[transformation] = true;
                Main.NewText($"You've achieved mastery in {DBZMOD.Instance.TransformationDefinitionManager[masteryFormBuffKeyName].Text} form.");
            }
        }

        public void ApplyHitStun(NPC target, int duration, float slowedTo, float recoversDuringFramePercent)
        {
            hitStunManager.DoHitStun(target, duration, slowedTo, recoversDuringFramePercent);
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

        // TODO Clean up
        private void HandleOverloadCounters()
        {
            TransformationDefinition transformation = GetCurrentTransformation();
            // clamp overload current values to 0/max
            overloadCurrent = (int)Math.Max(0, Math.Min(overloadMax, overloadCurrent));

            // does the player have the legendary trait
            if (IsLegendary())
            {
                // TODO Change this
                // is the player in a legendary transform step (that isn't SSJ1)?
                if (transformation == TransformationDefinitionManager.LSSJDefinition && transformation != TransformationDefinitionManager.SSJ1Definition && overloadCurrent <= overloadMax)
                {
                    overloadTimer++;
                    if (IsPlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition))
                    {
                        if (overloadTimer >= 45)
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

            if(IsPlayerTransformation(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition) || overloadCurrent > 0)
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
                kiDamage *= blackFusionIncrease;
                player.statDefense *= (int)blackFusionIncrease;
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

            if (!isAnyBossAlive)
            {
                blackFusionIncrease = 1f;
            }
        }

        public void HandleKaiokenDefenseDebuff()
        {
            if (DBZMOD.Instance.TransformationDefinitionManager.IsKaioken(GetCurrentTransformation()))
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
            if (player.heldProj != -1)
            {
                // player has a projectile, check to see if it's a charge ball or beam, that hijacks the octant for style.
                var proj = Main.projectile[player.heldProj];
                if (proj != null)
                {
                    if (proj.modProjectile != null && (proj.modProjectile is BaseBeamCharge))
                    {
                        var charge = proj.modProjectile as BaseBeamCharge;
                        if (charge.IsFired)
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

            if (syncPlayerTrait != PlayerTrait)
            {
                NetworkHelper.playerSync.SendChangedPlayerTrait(256, player.whoAmI, player.whoAmI, PlayerTrait);
                syncPlayerTrait = PlayerTrait;
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

        public void ChangeEyeColor(Color eyeColor)
        {
            // only fire this when attempting to change the eye color.
            if (originalEyeColor == null)
            {
                originalEyeColor = player.eyeColor;
            }
            player.eyeColor = eyeColor;
        }

        // TODO Remove all this in favor of auto checks.
        public void AwakeningFormUnlock()
        {
            if (!SSJ1Achived)
            {
                Main.NewText("The humiliation of failing drives you mad.", Color.Yellow);

                DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition.Unlock(this);

                isTransforming = true;
                SSJTransformation();
                UI.TransformationMenu.SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition;
                EndTransformations();
                rageCurrent = 0;
            }
            else if (SSJ1Achived && !SSJ2Achieved && !IsLegendary())
            {
                Main.NewText("The rage of failing once more dwells deep within you.", Color.Red);

                DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition.Unlock(this); 

                isTransforming = true;
                SSJ2Transformation();
                UI.TransformationMenu.SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition;
                EndTransformations();
                rageCurrent = 0;
            }
            else if (SSJ1Achived && IsLegendary() && !LSSJAchieved)
            {
                Main.NewText("Your rage is overflowing, you feel something rise up from deep inside.", Color.Green);

                DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition.Unlock(this);

                isTransforming = true;
                LSSJTransformation();
                UI.TransformationMenu.SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition;
                EndTransformations();
                rageCurrent = 0;
            }
            else if (SSJ2Achieved && !SSJ3Achieved)
            {
                Main.NewText("The ancient power of the Lihzahrds seeps into you, causing your power to become unstable.", Color.Orange);

                DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition.Unlock(this);

                isTransforming = true;
                SSJ3Transformation();
                UI.TransformationMenu.SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition;
                EndTransformations();
                rageCurrent = 0;
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

        // functions four-fold. Steps down one level in a given transformation tree: ussj -> assj -> ssj1. lssj2 -> Lssj -> ssj1. ssjg -> etc
        // also steps down from ssj1 + kk to just ssj1.
        public bool IsPoweringDownOneStep()
        {
            return powerDown.JustPressed && energyCharge.Current;
        }

        // TODO Change this
        public bool CanAscend()
        {
            TransformationDefinition transformation = GetCurrentTransformation();
            return transformation == TransformationDefinitionManager.SSJ1Definition || transformation == TransformationDefinitionManager.ASSJDefinition;
        }

        // TODO Change this
        public bool CanIncreaseKaiokenLevel()
        {
            // immediately handle aborts from super kaioken states
            if (IsPlayerTransformation(TransformationDefinitionManager.SuperKaiokenDefinition))
                return false;

            // TODO Chagne this
            if (!IsPlayerTransformation(TransformationDefinitionManager.KaiokenDefinition))
            {
                return kaiokenLevel == 0 && HasTransformation(TransformationDefinitionManager.KaiokenDefinition);
            }

            switch (kaiokenLevel)
            {
                case 0:
                    return HasTransformation(TransformationDefinitionManager.KaiokenDefinition);
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
            bool isKaioken = IsPlayerTransformation(TransformationDefinitionManager.KaiokenDefinition);

            if (kaiokenKey.JustPressed)
            {
                var canIncreaseKaiokenLevel = CanIncreaseKaiokenLevel();
                if (isKaioken)
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
                        TransformationDefinition transformation = isKaioken ? (TransformationDefinition) TransformationDefinitionManager.SuperKaiokenDefinition : TransformationDefinitionManager.KaiokenDefinition;

                        if (CanTransform(transformation))
                        {
                            kaiokenLevel++;
                            DoTransform(transformation);
                        }
                    }
                }
            } else if (IsPoweringDownOneStep())
            {
                if (isKaioken && kaiokenLevel > 1)
                {
                    kaiokenLevel--;
                }
            }
        }

        public void UpdateSynchronizedControls(TriggersSet triggerSet)
        {
            // this might look weird, but terraria seemed to treat these getters as changing the collection, resulting in some really strange errors/behaviors.
            // change these to normal ass setters at your own peril.
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
                Projectile.NewProjectile(originalPosition.X, originalPosition.Y, 0f, 0f, DBZMOD.Instance.ProjectileType("TransmissionLinesProj"), 0, 0, player.whoAmI);
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, DBZMOD.Instance.ProjectileType("TransmissionLinesProj"), 0, 0, player.whoAmI);
                
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
                    player.ApplyChannelingSlowdown();
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

        public bool HasTransformation(TransformationDefinition transformationDefinition) => PlayerTransformations.ContainsKey(transformationDefinition);

        public void CheckPlayerForTransformationStateDebuffApplication()
        {
            if (!DebugHelper.IsDebugModeOn())
            {
                wasKaioken = isKaioken;
                wasTransformed = isTransformed;

                TransformationDefinition transformation = GetCurrentTransformation();

                isKaioken = TransformationDefinitionManager.IsKaioken(transformation);
                isTransformed = IsPlayerTransformed();
                // this way, we can apply exhaustion debuffs correctly.
                if (wasKaioken && !isKaioken)
                {
                    bool wasSsjkk = wasTransformed;
                    player.AddKaiokenExhaustion(wasSsjkk ? 2 : 1);
                    kaiokenLevel = 0; // make triple sure the Kaio level gets reset.
                }

                if (wasTransformed && !isTransformed)
                {
                    //player.AddTransformationExhaustion();
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
    }
}