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
        public int MenuSelection = 0;
        public bool SSJ1Achieved;
        public bool scouterT2;
        public bool scouterT3;
        public bool scouterT4;
        public bool scouterT5;
        public bool scouterT6;
        public bool IsTransforming;
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
        public bool spiritualEmblem;
        public int SSJAuraBeamTimer;
        public bool hasKaioken;
        public bool hasSSJ1;
        public bool kiLantern;
        public bool speedToggled = true;
        public bool IsTransformed;
        public float KiDrainMulti;
        public int ChargeSoundTimer;
        public int TransformCooldown;
        public bool diamondRing;
        public bool emeraldRing;
        public bool sapphireRing;
        public bool topazRing;
        public bool rubyRing;
        public static ModHotKey KaiokenKey;
        public static ModHotKey EnergyCharge;
        public static ModHotKey Transform;
        public static ModHotKey PowerDown;
        public static ModHotKey SpeedToggle;
        public static ModHotKey QuickKi;
        public static ModHotKey TransMenu;
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
            if(IsTransforming)
            {
                SSJAuraBeamTimer++;
            }
            if (SSJAuraBeamTimer > 10 && IsTransforming)
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
            tag.Add("KiCurrent", KiCurrent);
            tag.Add("KiRegenRate", KiRegenRate);
            tag.Add("KiEssence1", KiEssence1);
            tag.Add("KiEssence2", KiEssence2);
            tag.Add("KiEssence3", KiEssence3);
            tag.Add("KiEssence4", KiEssence4);

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
            KiCurrent = tag.Get<int>("KiCurrent");
            KiRegenRate = tag.Get<int>("KiRegenRate");
            KiEssence1 = tag.Get<bool>("KiEssence1");
            KiEssence2 = tag.Get<bool>("KiEssence2");
            KiEssence3 = tag.Get<bool>("KiEssence3");
            KiEssence4 = tag.Get<bool>("KiEssence4");
        }




        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Transform.JustPressed)
            {
                if (!player.HasBuff(mod.BuffType("SSJ1Buff")) && SSJ1Achieved && !IsTransforming && !player.channel && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
                {
                    player.AddBuff(mod.BuffType("SSJ1Buff"), 1800);
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ1AuraProjStart"), 0, 0, player.whoAmI);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/AuraStart").WithVolume(.7f));
                }
            }
            if (SpeedToggle.JustPressed)
            {
                speedToggled = !speedToggled;
            }
            if(QuickKi.JustPressed)
            {
            }

            if (TransMenu.JustPressed && !UI.TransMenu.menuvisible)
            {
                UI.TransMenu.menuvisible = true;
            }
            if (TransMenu.JustPressed && UI.TransMenu.menuvisible)
            {
                UI.TransMenu.menuvisible = false;
            }
                
            if (KaiokenKey.JustPressed && (!player.HasBuff(mod.BuffType("KaiokenBuff")) && !player.HasBuff(mod.BuffType("KaiokenBuffX3")) && !player.HasBuff(mod.BuffType("KaiokenBuffX10")) && !player.HasBuff(mod.BuffType("KaiokenBuffX20")) && !player.HasBuff(mod.BuffType("KaiokenBuffX100"))) && !player.HasBuff(mod.BuffType("TiredDebuff")) && !player.HasBuff(mod.BuffType("SSJ1Buff")) && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")) && KaioAchieved && !player.channel)
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

            else if (KaiokenKey.JustPressed && (player.HasBuff(mod.BuffType("SSJ1Buff"))) && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.ClearBuff(mod.BuffType("SSJ1Buff"));
                player.AddBuff(mod.BuffType("SSJ1KaiokenBuff"), 1800);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KaiokenAuraProj"), 0, 0, player.whoAmI);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("SSJ1AuraProj"), 0, 0, player.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KaioAuraAscend").WithVolume(.8f));
            }
            else if (Transform.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuff"))) && !player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
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
                if(!hasKaioken && !hasSSJ1)
                {
                    Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("BaseAuraProj"), 0, 0, player.whoAmI);
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyChargeStart").WithVolume(.7f));
            }
            if (PowerDown.JustPressed && (player.HasBuff(mod.BuffType("KaiokenBuff")) || player.HasBuff(mod.BuffType("KaiokenBuffX3")) || player.HasBuff(mod.BuffType("KaiokenBuffX10")) || player.HasBuff(mod.BuffType("KaiokenBuffX20")) || player.HasBuff(mod.BuffType("KaiokenBuffX100")) || player.HasBuff(mod.BuffType("SSJ1KaiokenBuff"))))
            {
                player.ClearBuff(mod.BuffType("KaiokenBuff"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX3"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX10"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX20"));
                player.ClearBuff(mod.BuffType("KaiokenBuffX100"));
                player.ClearBuff(mod.BuffType("SSJ1KaiokenBuff"));
                player.AddBuff(mod.BuffType("TiredDebuff"), 3600);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
                IsTransformed = false;
            }
            if (PowerDown.JustPressed && (player.HasBuff(mod.BuffType("SSJ1Buff"))))
            {
                player.ClearBuff(mod.BuffType("SSJ1Buff"));
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
           else if (!Fragment1 && !Fragment2 && !Fragment3 && !Fragment4 && !Fragment5)
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
                        }
                    }
                }
            }
            if (!KiEssence1 && !KiEssence2 && !KiEssence3 && !KiEssence4)
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
            diamondRing = false;
            emeraldRing = false;
            rubyRing = false;
            sapphireRing = false;
            topazRing = false;
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damageSource.SourceNPCIndex > -1)
            {
                NPC culprit = Main.npc[damageSource.SourceNPCIndex];
                if (culprit.boss && !SSJ1Achieved && player.whoAmI == Main.myPlayer)
                {
                    if ((Main.rand.Next(9) == 0))
                    {
                        Main.NewText("The humiliation of failing drives you mad.");
                        player.statLife = 1;
                        player.HealEffect(1);
                        SSJ1Achieved = true;
                        IsTransforming = true;
                        SSJTransformation();
                        MenuSelection = 1;
                        return false;
                    }
                }
            }

            return true;
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if(IsTransforming)
            {
                return false;
            }
            if(ChlorophyteHeadPieceActive && !player.HasBuff(mod.BuffType("ChlorophyteRegen")))
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

        public void SSJTransformationBeams()
        {
            Vector2 velocity = Vector2.UnitY.RotateRandom(MathHelper.TwoPi) * 12;
            Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("SSJ1AuraProjBeamHead"), 0, 0, player.whoAmI);
        }
        public override void SetupStartInventory(IList<Item> items)
        {
            Item item1 = new Item();
            item1.SetDefaults(mod.ItemType("KiFist1"));   
            item1.stack = 1;
            items.Add(item1);

            Item item9 = new Item();
            item9.SetDefaults(mod.ItemType("KiAggravationStone"));
            item9.stack = 1;
            items.Add(item9);

            Item item8 = new Item();
            item8.SetDefaults(mod.ItemType("EmptyRing"));
            item8.stack = 1;
            items.Add(item8);
        }
        //public override void UpdateBiomes()

        //ZoneCustomBiome = (DBZMODWorld.customBiome > 0);  
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
			if (player.HasBuff(mod.BuffType("SSJ1KaiokenBuff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/SSJ1KaiokenHair");
                player.eyeColor = Color.Turquoise;
            }
            else if (player.HasBuff(mod.BuffType("SSJ2Buff")))
            {
                Hair = mod.GetTexture("Hairs/SSJ/SSJ2Hair");
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
