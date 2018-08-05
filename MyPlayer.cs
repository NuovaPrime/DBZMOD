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

namespace DBZMOD
{
    public class MyPlayer : ModPlayer
    {
        #region Variables
        public float KiDamage;
        public float KiKbAddition;
        public float KiSpeedAddition;
        public int KiCrit;
        public int KiRegenTimer;
        public bool CobaltBonus;
        public int KiMax;
        public int KiCurrent;
        public int KiRegenRate = 1;
        public bool ZoneCustomBiome = false;
        public int drawX;
        public int drawY;
        public bool SSJ1Achieved;
        public bool scouterT2;
        public bool scouterT3;
        public bool scouterT4;
        public bool scouterT5;
        public bool scouterT6;
        public bool IsTransformingSSJ1;
        public bool IsTransformingSSJ2;
        public bool IsTransformingSSJ3;
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
        public bool spiritualEmblem;
        public int SSJAuraBeamTimer;
        public bool hasKaioken;
        public float KaiokenTimer  = 0.0f;
        public bool hasSSJ1;
        public bool kiLantern;
        public bool speedToggled = true;
        public bool IsTransformed;
        public float KiDrainMulti;
        public int ChargeSoundTimer;
        public int TransformCooldown;
        public bool diamondNecklace;
        public bool emeraldNecklace;
        public bool sapphireNecklace;
        public bool topazNecklace;
		public bool amberNecklace;
		public bool amethystNecklace;
        public bool rubyNecklace;
		public bool dragongemNecklace;
        public static ModHotKey KaiokenKey;
        public static ModHotKey EnergyCharge;
        public static ModHotKey Transform;
        public static ModHotKey PowerDown;
        public static ModHotKey SpeedToggle;
        public static ModHotKey QuickKi;
        public static ModHotKey TransMenu;
        public bool ASSJAchieved;
        public bool USSJAchieved;
        public bool SSJ2Achieved;
        public bool SSJ3Achieved;
        public bool IsCharging;
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
        //public static bool RealismMode = false;
        #endregion

        public static MyPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<MyPlayer>();
        }
        public bool KaiokenCheck()
        {
            if(player.HasBuff(mod.BuffType("KaiokenBuff")) || player.HasBuff(mod.BuffType("KaiokenBuffX3")) || player.HasBuff(mod.BuffType("KaiokenBuffX10")) || player.HasBuff(mod.BuffType("KaiokenBuffX20")) || player.HasBuff(mod.BuffType("KaiokenBuffX100")))
            {
                return hasKaioken = true;
            }
            else
            {
                return hasKaioken = false;
            }
        }
        public override void PreUpdate()
        {
            if(kiLantern)
            {
                player.AddBuff(mod.BuffType("KiLanternBuff"), 18000);
            }
            if (!kiLantern)
            {
                player.ClearBuff(mod.BuffType("KiLanternBuff"));
            }
            if(IsTransformingSSJ1)
            {
                SSJAuraBeamTimer++;
            }
            if (SSJAuraBeamTimer > 10 && IsTransformingSSJ1)
            {
                SSJTransformationBeams();
                SSJAuraBeamTimer = 0;
            }
            if(KiCurrent < 0)
            {
                KiCurrent = 0;
            }
            if(CobaltBonus)
            {
                KiKbAddition += 15;
            }
            if(SSJ1Achieved)
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
            if (player.HasBuff(mod.BuffType("SSJ2Buff")))
            {
                LightningFrameTimer++;
            }
            if (player.HasBuff(mod.BuffType("SSJ3Buff")))
            {
                LightningFrameTimer++;
            }
            if (LightningFrameTimer >= 15)
            {
                LightningFrameTimer = 0;
            }
            if(KaiokenCheck())
            {
                KaiokenTimer += 1.5f;
            }

            if (MasteryLevel1 >= 0.5f && !ASSJAchieved)
            {
                ASSJAchieved = true;
                Main.NewText("Your SSJ1 Mastery has been upgraded.", 232, 242, 50);
            }
            else if (MasteryLevel1 >= 0.75f && !USSJAchieved)
            {
                USSJAchieved = true;
                Main.NewText("Your SSJ1 Mastery has been upgraded.", 232, 242, 50);
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
            
        }

        public bool SSJ1Check()
        {
            if (player.HasBuff(mod.BuffType("SSJ1Buff")))
            {
                return hasSSJ1 = true;
            }
            else
            {
                return hasSSJ1 = false;
            }
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
            //RealismMode = tag.Get<bool>("RealismMode");
        }




        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Transform.JustPressed)
            {
                if (!player.HasBuff(mod.BuffType("SSJ1Buff")) && SSJ1Achieved && UI.TransMenu.MenuSelection == 1 && !IsTransformingSSJ1 && !player.channel && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("KaiokenBuffX3")) && !player.HasBuff(mod.BuffType("KaiokenBuffX10")) && !player.HasBuff(mod.BuffType("KaiokenBuffX20")) && !player.HasBuff(mod.BuffType("KaiokenBuffX100")) && !player.HasBuff(mod.BuffType("ASSJBuff")) && !player.HasBuff(mod.BuffType("USSJBuff")) && !player.HasBuff(mod.BuffType("SSJ2Buff")) && !player.HasBuff(mod.BuffType("SSJ3Buff"))))
                {
                    player.AddBuff(mod.BuffType("SSJ1Buff"), 1800);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ1AuraProjStart"), 0, 0, player.whoAmI);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                }
                else if (player.HasBuff(mod.BuffType("SSJ1Buff")) && ASSJAchieved && !IsTransformingSSJ1 && !player.channel && (UI.TransMenu.MenuSelection == 1))
                {
                    player.AddBuff(mod.BuffType("ASSJBuff"), 1800);
                    player.ClearBuff(mod.BuffType("SSJ1Buff"));
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(1.0f));
                }
                else if (player.HasBuff(mod.BuffType("ASSJBuff")) && USSJAchieved && !IsTransformingSSJ1 && !player.channel && (UI.TransMenu.MenuSelection == 1))
                {
                    player.AddBuff(mod.BuffType("USSJBuff"), 1800);
                    player.ClearBuff(mod.BuffType("ASSJBuff"));
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                }
                else if (!player.HasBuff(mod.BuffType("SSJ2Buff")) && !player.HasBuff(mod.BuffType("SSJ1Buff")) && !player.HasBuff(mod.BuffType("SSJ3Buff")) && SSJ2Achieved && UI.TransMenu.MenuSelection == 2 && !IsTransformingSSJ1 && !IsTransformingSSJ2 && !player.channel && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("KaiokenBuffX3")) && !player.HasBuff(mod.BuffType("KaiokenBuffX10")) && !player.HasBuff(mod.BuffType("KaiokenBuffX20")) && !player.HasBuff(mod.BuffType("KaiokenBuffX100")) && !player.HasBuff(mod.BuffType("ASSJBuff")) && !player.HasBuff(mod.BuffType("USSJBuff"))))
                {
                    player.AddBuff(mod.BuffType("SSJ2Buff"), 1800);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ2AuraProj"), 0, 0, player.whoAmI);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                }
                else if (!player.HasBuff(mod.BuffType("SSJ3Buff")) && !player.HasBuff(mod.BuffType("SSJ1Buff")) && !player.HasBuff(mod.BuffType("SSJ2Buff")) && SSJ3Achieved && UI.TransMenu.MenuSelection == 3 && !IsTransformingSSJ1 && !IsTransformingSSJ2 && !IsTransformingSSJ3 && !player.channel && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("KaiokenBuffX3")) && !player.HasBuff(mod.BuffType("KaiokenBuffX10")) && !player.HasBuff(mod.BuffType("KaiokenBuffX20")) && !player.HasBuff(mod.BuffType("KaiokenBuffX100")) && !player.HasBuff(mod.BuffType("ASSJBuff")) && !player.HasBuff(mod.BuffType("USSJBuff"))))
                {
                    player.AddBuff(mod.BuffType("SSJ3Buff"), 900);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ3AuraProj"), 0, 0, player.whoAmI);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SSJAscension").WithVolume(.7f));
                }
            }

            if (SpeedToggle.JustPressed)
            {
                speedToggled = !speedToggled;
            }
            if(QuickKi.JustPressed)
            {
            }

            if (TransMenu.JustPressed)
            {
                UI.TransMenu.menuvisible = !UI.TransMenu.menuvisible;
            }
                
            if (KaiokenKey.JustPressed && (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("KaiokenBuffX3")) && !player.HasBuff(mod.BuffType("KaiokenBuffX10")) && !player.HasBuff(mod.BuffType("KaiokenBuffX20")) && !player.HasBuff(mod.BuffType("KaiokenBuffX100"))) && !player.HasBuff(mod.BuffType("TiredDebuff")) && !player.HasBuff(mod.BuffType("SSJ1Buff")) && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && !player.HasBuff(mod.BuffType("SSJ2Buff")) && !player.HasBuff(mod.BuffType("SSJ3Buff")) && KaioAchieved && !player.channel)
            {
                player.AddBuff(mod.BuffType("KaiokenBuff"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProj"), 0, 0, player.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraStart").WithVolume(.5f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuff"))) && KaioFragment1)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.AddBuff(mod.BuffType("KaiokenBuffX3"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx3"), 0, 0, player.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.6f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuffX3"))) && KaioFragment2)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuffX3"));
                player.AddBuff(mod.BuffType("KaiokenBuffX10"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx10"), 0, 0, player.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.6f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuffX10"))) && KaioFragment3)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuffX10"));
                player.AddBuff(mod.BuffType("KaiokenBuffX20"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx20"), 0, 0, player.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.7f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuffX20"))) && KaioFragment4)
            {
                player.ClearBuff(mod.BuffType("KaiokenBuffX20"));
                player.AddBuff(mod.BuffType("KaiokenBuffX100"), 18000);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProjx100"), 0, 0, player.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.8f));
            }
            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("SSJ1Buff"))) && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && !player.HasBuff(mod.BuffType("TiredDebuff")))
            {
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.ClearBuff(mod.BuffType("SSJ1Buff"));
                player.AddBuff(mod.BuffType("SSJ1KaiokenBuff"), 1800);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProj"), 0, 0, player.whoAmI);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ1AuraProj"), 0, 0, player.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.8f));
            }

            if (EnergyCharge.Current && (KiCurrent < KiMax) && !player.channel)
            {
                KiCurrent += KiRegenRate;
                player.velocity = new Vector2(0,player.velocity.Y);
                ChargeSoundTimer++;
                if (ChargeSoundTimer > 22)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyCharge").WithVolume(.5f));
                    ChargeSoundTimer = 0;
                }
            }
            if (KiCurrent > KiMax)
            {
                KiCurrent = KiMax;
            }
            if (EnergyCharge.JustPressed)
            {
                if(!IsTransformed)
                {
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaseAuraProj"), 0, 0, player.whoAmI);
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyChargeStart").WithVolume(.7f));
                IsCharging = true;
            }
            if (EnergyCharge.JustReleased)
            {
                IsCharging = false;
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
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
                IsTransformed = false;
            }
            if (PowerDown.JustPressed && player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                player.ClearBuff(mod.BuffType("SSJ1KaiokenBuff"));
                player.AddBuff(mod.BuffType("TiredDebuff"), (int)(KaiokenTimer*2));
                KaiokenTimer = 0.0f;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
                IsTransformed = false;
            }
            if (PowerDown.JustPressed && (player.HasBuff(mod.BuffType("SSJ1Buff")) || player.HasBuff(mod.BuffType("SSJ2Buff")) || player.HasBuff(mod.BuffType("ASSJBuff")) || player.HasBuff(mod.BuffType("USSJBuff")) || player.HasBuff(mod.BuffType("SSJ3Buff"))))
            {
                player.ClearBuff(mod.BuffType("SSJ1Buff"));
                player.ClearBuff(mod.BuffType("SSJ2Buff"));
                player.ClearBuff(mod.BuffType("ASSJBuff"));
                player.ClearBuff(mod.BuffType("USSJBuff"));
                player.ClearBuff(mod.BuffType("SSJ3Buff"));
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
                IsTransformed = false;
            }

        }        
        public MyPlayer() : base()
		{
		}
        public override void ResetEffects()
        {
            KiDamage = 1f;
            KiKbAddition = 0f;
            if(Fragment1)
            {
                KiMax = 2000;

                if (Fragment2)
                {
                    KiMax = 4000;

                    if (Fragment3)
                    {
                        KiMax = 6000;

                        if (Fragment4)
                        {
                            KiMax = 8000;

                            if (Fragment5)
                            {
                                KiMax = 10000;
                            }
                        }
                    }
                }
            }
           else
            {
                KiMax = 1000;
            }
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

                            if(KiEssence5)
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
            CobaltBonus = false;
            diamondNecklace = false;
            emeraldNecklace = false;
            rubyNecklace = false;
			dragongemNecklace = false;
            sapphireNecklace = false;
            topazNecklace = false;
			amberNecklace = false;
			amethystNecklace = false;
            //IsCharging = false;
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && !SSJ1Achieved && player.whoAmI == Main.myPlayer && NPC.downedBoss3)
                {
                    if ((Main.rand.Next(9) == 0))
                    {
                        Main.NewText("The humiliation of failing drives you mad.", Color.Yellow);
                        player.statLife = 1;
                        player.HealEffect(1);
                        SSJ1Achieved = true;
                        IsTransformingSSJ1 = true;
                        SSJTransformation();
                        UI.TransMenu.MenuSelection = 1;
                        return false;
                    }
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && SSJ1Achieved && !SSJ2Achieved && player.whoAmI == Main.myPlayer && NPC.downedPlantBoss && player.HasBuff(mod.BuffType("SSJ1Buff")))
                {
                    if ((Main.rand.Next(4) == 0))
                    {
                        Main.NewText("The rage of failing once more dwells deep within you.", Color.Red);
                        player.statLife = 1;
                        player.HealEffect(1);
                        SSJ2Achieved = true;
                        IsTransformingSSJ2 = true;
                        SSJ2Transformation();
                        UI.TransMenu.MenuSelection = 2;
                        return false;
                    }
                }
            }
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.type == NPCID.CultistBoss && SSJ1Achieved && SSJ2Achieved && !SSJ3Achieved && player.whoAmI == Main.myPlayer && player.HasBuff(mod.BuffType("SSJ2Buff")))
                {
                    if ((Main.rand.Next(2) == 0))
                    {
                        Main.NewText("The ancient power of the cultist seeps into you, causing your power to go haywire.", Color.Blue);
                        player.statLife = 1;
                        player.HealEffect(1);
                        SSJ3Achieved = true;
                        IsTransformingSSJ3 = true;
                        SSJ3Transformation();
                        UI.TransMenu.MenuSelection = 3;
                        return false;
                    }
                }
            }

            return true;
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if(IsTransformingSSJ1)
            {
                return false;
            }
            if (IsTransformingSSJ2)
            {
                return false;
            }
            if (IsTransformingSSJ3)
            {
                return false;
            }
            if (ChlorophyteHeadPieceActive && !player.HasBuff(mod.BuffType("ChlorophyteRegen")))
            {
                player.AddBuff(mod.BuffType("ChlorophyteRegen"), 180);
                return true;
            }
            return true;
        }

        public void SSJTransformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJRockProjStart"), 0, 0, player.whoAmI);
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/GroundRumble").WithVolume(2f));
        }
        public void SSJ2Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJAuraBall"), 0, 0, player.whoAmI);
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Awakening").WithVolume(2f));
        }
        public void SSJ3Transformation()
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 70, 0, 0, mod.ProjectileType("SSJ3LightPillar"), 0, 0, player.whoAmI);
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Awakening").WithVolume(2f));
        }

        public void SSJTransformationBeams()
        {
            Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 12;
            Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("SSJ1AuraProjBeamHead"), 0, 0, player.whoAmI);
        }
        public override void SetupStartInventory(IList<Item> items)
        {
            /*Item item1 = new Item();
            item1.SetDefaults(mod.ItemType("KiFist1"));   
            item1.stack = 1;
            items.Add(item1);*/

            Item item9 = new Item();
            item9.SetDefaults(mod.ItemType("KiAggravationStone"));
            item9.stack = 1;
            items.Add(item9);

            Item item8 = new Item();
            item8.SetDefaults(mod.ItemType("EmptyNecklace"));
            item8.stack = 1;
            items.Add(item8);
        }
        //public override void UpdateBiomes()
        public int LightningFrameTimer;
        //ZoneCustomBiome = (DBZMODWorld.customBiome > 0);
        public static readonly PlayerLayer LightningEffects = new PlayerLayer("DBZMOD", "LightningEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("DBZMOD");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            int frame = modPlayer.LightningFrameTimer / 5;
            /*if(frame == 0)
            {
                Main.NewText("Frame 1");
            }
            if (frame == 1)
            {
                Main.NewText("Frame 2");
            }
            if (frame == 2)
            {
                Main.NewText("Frame 3");
            }*/
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
            if (drawPlayer.HasBuff(mod.BuffType("SSJ3Buff")))
            {
                Texture2D texture = mod.GetTexture("Dusts/LightningYellow");
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
            if (player.HasBuff(mod.BuffType("SSJ1Buff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/SSJ1Hair");
                player.eyeColor = Color.Turquoise;
            }
            else if (player.HasBuff(mod.BuffType("ASSJBuff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/ASSJHair");
                player.eyeColor = Color.Turquoise;
            }
            else if (player.HasBuff(mod.BuffType("USSJBuff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/USSJHair");
                player.eyeColor = Color.Turquoise;
            }
            else if (player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/SSJ1KaiokenHair");
                player.eyeColor = Color.Red;
            }
            else if (player.HasBuff(mod.BuffType("SSJ2Buff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/SSJ2Hair");
                player.eyeColor = Color.Turquoise;
            }
            else if (player.HasBuff(mod.BuffType("SSJ3Buff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/SSJ3Hair");
                player.eyeColor = Color.Turquoise;
            }
            else
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
                   //if (!MyPlayer.ModPlayer(player).IsTransformed)
                   // return;

                   Color alpha = draw.drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)(draw.position.X + draw.drawPlayer.width * 0.5) / 16, (int)((draw.position.Y + draw.drawPlayer.height * 0.25) / 16.0), Color.White), draw.shadow);
                       DrawData data = new DrawData(Hair, new Vector2((float)((int)(draw.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(draw.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.headPosition + draw.headOrigin, player.bodyFrame, alpha, player.headRotation, draw.headOrigin, 1f, draw.spriteEffects, 0);
                       data.shader = draw.hairShader;
                       Main.playerDrawData.Add(data);
                   });
            }
            if (Hair != null)
            {
                PlayerLayer.Head.visible = false;
            }
        }
    }
}
