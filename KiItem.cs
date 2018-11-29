﻿using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using DBZMOD;
using System;

namespace DBZMOD
{
    public abstract class KiProjectile : ModProjectile
    {
        public int ChargeLevel;
        public int ChargeTimer;
        public float ChargeTimerMax;
        public int ChargeLimit = 4;
        public int KiDrainTimer;
        public int SizeTimer;
        public int originalWidth;
        public int originalHeight;
        public bool ChargeBall;
        public bool KiWeapon = true;
        public bool BeamTrail;
        public int KiDrainRate;
        public Color color;
        public Player player;
        public MyPlayer myPlayer;
        public int yoffset;
        public int xoffset;
        public float ballscale = 1f;
        public int dusttype;


        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override bool PreAI()
        {
            if (player == null)
                player = Main.player[projectile.owner];

            if (myPlayer == null && player != null)
                myPlayer = MyPlayer.ModPlayer(player);

            return base.PreAI();
        }

        public override void PostAI()
        {
            projectile.netUpdate = true;
            player = Main.player[projectile.owner];
            ChargeLimit = ChargeLimit + MyPlayer.ModPlayer(player).ChargeLimitAdd;
            if (!ChargeBall)
            {
                projectile.scale = projectile.scale + ChargeLevel;
            }

            if (BeamTrail && projectile.scale > 0 && SizeTimer > 0)
            {
                SizeTimer = 120;
                SizeTimer--;
                projectile.scale = (projectile.scale * SizeTimer / 120f);
            }
            if (ChargeBall)
            {
                if (MyPlayer.ModPlayer(player).KiCurrent <= 0)
                {
                    projectile.active = false;
                }
                projectile.hide = true;

                if (projectile.timeLeft < 4)
                {
                    projectile.timeLeft = 10;
                }

                projectile.position.X = player.Center.X + (player.direction * 20 + xoffset) - 5;
                projectile.position.Y = player.Center.Y - 3 + yoffset;
                projectile.netUpdate2 = true;

                if (!player.channel && ChargeLevel < 1)
                {
                    projectile.Kill();
                }
                if (player.channel && projectile.active)
                {
                    ChargeTimer++;
                    KiDrainTimer++;
                    player.velocity = new Vector2(player.velocity.X / 3, player.velocity.Y);
                }

                //ChargeTimerMax -= MyPlayer.ModPlayer(player).chargeTimerMaxAdd;

                if (ChargeTimer > ChargeTimerMax && ChargeLevel < ChargeLimit)
                {
                    ChargeLevel += 1;
                    ChargeTimer = 0;
                }
                if (KiDrainTimer > 1 && MyPlayer.ModPlayer(player).KiCurrent >= 0)
                {
                    MyPlayer.ModPlayer(player).KiCurrent -= KiDrainRate;
                    KiDrainTimer = 0;
                }
                for (int d = 0; d < 4; d++)
                {
                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.position + (position * (10 + 2.0f * projectile.scale)), projectile.width, projectile.height, dusttype, 0f, 0f, 213, default(Color), ballscale);
                    tDust.velocity = Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                    tDust.noGravity = true;
                }
            }
        }

        public void SetScale(float scale)
        {
            projectile.scale = scale;
            projectile.width = (int)(originalWidth * scale);
            projectile.height = (int)(originalHeight * scale);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player owner = null;
            if (projectile.owner != -1)
                owner = Main.player[projectile.owner];
            else if (projectile.owner == 255)
                owner = Main.LocalPlayer;

            if (Main.expertMode)
            {
                if ((target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) || (target.type >= NPCID.TheDestroyer && target.type <= NPCID.TheDestroyerTail))
                {
                    damage /= 2;
                }
            }
        }
        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            int item = 0;
            if (KiWeapon)
            {
                if (npc.life <= 0)
                {
                    if (npc.boss && MyPlayer.ModPlayer(player).RageCurrent >= 1)
                    {
                        MyPlayer.ModPlayer(player).RageCurrent -= 1;
                    }
                    if (Main.rand.Next(MyPlayer.ModPlayer(player).KiOrbDropChance) == 0)
                    {
                        item = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiOrb"), 1);
                    }
                    if (Main.netMode == 1 && item >= 0)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            if (KiWeapon)
            {
                if (MyPlayer.ModPlayer(player).palladiumBonus)
                {
                    if (Main.rand.Next(10) == 0)
                    {
                        item = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiOrb"), 1);
                    }
                    if (Main.netMode == 1 && item >= 0)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            if (KiWeapon)
            {
                if (MyPlayer.ModPlayer(player).luminousSectum)
                {
                    if (Main.rand.Next(8) == 0)
                    {
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 20, mod.ProjectileType("KiSpark"),
                            damage / 2, 3, player.whoAmI);
                    }
                }
            }
        }
    }

    public abstract class AuraProjectile : ModProjectile
    {
        public bool IsSSJAura;
        public bool IsKaioAura;
        public bool IsGodAura;
        public bool AuraActive;
        public Vector2 AuraOffset;
        public float ScaleExtra;
        public int FrameAmount = 3;
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;

            if (projectile.timeLeft < 2)
            {
                projectile.timeLeft = 10;
            }
            if (player.channel)
            {
                player.velocity = new Vector2(0, player.velocity.Y);
            }

            if (IsSSJAura)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter > 5)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= FrameAmount)
                {
                    projectile.frame = 0;
                }
            }

            else if (IsKaioAura)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter > 5)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }        

            // added so that projectile alpha is always half
            projectile.alpha = 50;

            float chargingScale = 0.0f;
            float chargingAuraOffset = 0.0f;
            if (IsSSJAura || IsKaioAura)
            {
                if (MyPlayer.ModPlayer(player).IsCharging)
                {
                    chargingScale = 0.3f;
                    chargingAuraOffset = -18;
                }
                else
                {
                    chargingScale = 0.0f;
                    chargingAuraOffset = 0;
                }
            }

            projectile.scale = 1.0f + ScaleExtra + chargingScale;
            

            // update handler to reorient the charge up aura after the aura offsets are defined.
            bool isPlayerMostlyStationary = Math.Abs(player.velocity.X) <= 6F && Math.Abs(player.velocity.Y)  <= 6F;
            if (MyPlayer.ModPlayer(player).IsFlying && !isPlayerMostlyStationary)
            {
                double rotationOffset = player.fullRotation <= 0f ? (float)Math.PI : -(float)Math.PI;
                float newRotation = (float)((player.fullRotation + rotationOffset) % Math.PI);
                // hack to fix when the newRotation tries to be almost zero
                if (Math.Abs(newRotation) < 0.000001)
                {
                    projectile.rotation = (float)Math.PI;
                } else
                {
                    projectile.rotation = newRotation;
                }

                
                // using the angle of attack, construct the cartesian offsets of the aura based on the height of both things

                double widthRadius = player.width / 4;
                double heightRadius = player.height / 4;
                double auraWidthRadius = projectile.width / 4;
                double auraHeightRadius = projectile.height / 4;

                double widthOffset = auraWidthRadius - (widthRadius + (AuraOffset.Y + 18) + chargingAuraOffset);
                double heightOffset = auraHeightRadius - (heightRadius + (AuraOffset.Y + 18) + chargingAuraOffset);
                double cartesianOffsetX = widthOffset * Math.Cos(player.fullRotation);
                double cartesianOffsetY = heightOffset * Math.Sin(player.fullRotation);

                // Main.NewText(string.Format("Rotation Offset: {0} - heightOffset: {1} before transform: {2}", projectile.rotation, cartesianOffsetY, heightOffset));

                Vector2 cartesianOffset = player.Center + new Vector2((float)-cartesianOffsetY, (float)cartesianOffsetX);

                // offset the aura
                projectile.Center = cartesianOffset;

            }
            else
            {
                projectile.Center = player.Center + new Vector2(AuraOffset.X, (AuraOffset.Y + chargingAuraOffset));
                projectile.rotation = 0;
            }
        }
    }

    public abstract class KiItem : ModItem
    {
        public bool IsFistWeapon;
        public bool CanUseHeavyHit;
        #region Boss bool checks
        public bool EyeDowned;
        public bool BeeDowned;
        public bool WallDowned;
        public bool PlantDowned;
        public bool DukeDowned;
        public bool MoodlordDowned;
        public override void PostUpdate()
        {
            if (NPC.downedBoss1)
            {
                EyeDowned = true;
            }
            if (NPC.downedQueenBee)
            {
                BeeDowned = true;
            }
            if (Main.hardMode)
            {
                WallDowned = true;
            }
            if (NPC.downedPlantBoss)
            {
                PlantDowned = true;
            }
            if (NPC.downedFishron)
            {
                DukeDowned = true;
            }
            if (NPC.downedMoonlord)
            {
                MoodlordDowned = true;
            }
            if (item.channel)
            {
                item.autoReuse = false;
            }
        }
        #endregion
        #region FistAdditions 

        #endregion

        public override void SetDefaults()
        {
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.summon = false;
        }
        public float KiDrain;
        public string WeaponType;
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override int ChoosePrefix(Terraria.Utilities.UnifiedRandom rand)
        {
            var PrefixChooser = new WeightedRandom<int>();
            PrefixChooser.Add(mod.PrefixType("BalancedPrefix"), 3); // 3 times as likely
            PrefixChooser.Add(mod.PrefixType("CondensedPrefix"), 3);
            PrefixChooser.Add(mod.PrefixType("MystifyingPrefix"), 3);
            PrefixChooser.Add(mod.PrefixType("UnstablePrefix"), 3);
            PrefixChooser.Add(mod.PrefixType("FlawedPrefix"), 3);
            PrefixChooser.Add(mod.PrefixType("BoostedPrefix"), 3);
            PrefixChooser.Add(mod.PrefixType("NegatedPrefix"), 3);
            PrefixChooser.Add(mod.PrefixType("OutrageousPrefix"), 3);
            PrefixChooser.Add(mod.PrefixType("PoweredPrefix"), 2);
            PrefixChooser.Add(mod.PrefixType("FlashyPrefix"), 2);
            PrefixChooser.Add(mod.PrefixType("InfusedPrefix"), 2);
            PrefixChooser.Add(mod.PrefixType("DistractingPrefix"), 2);
            PrefixChooser.Add(mod.PrefixType("DestructivePrefix"), 2);
            PrefixChooser.Add(mod.PrefixType("MasteredPrefix"), 1);
            PrefixChooser.Add(mod.PrefixType("TranscendedPrefix"), 1);
            int choice = PrefixChooser;
            if ((item.damage > 0) && item.maxStack == 1)
            {
                return choice;
            }
            return -1;
        }
        public override bool ReforgePrice(ref int reforgePrice, ref bool canApplyDiscount)
        {
            return true;
        }
        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            knockback = knockback + MyPlayer.ModPlayer(player).KiKbAddition;
        }
        public override void GetWeaponDamage(Player player, ref int damage)
        {
            damage = (int)(damage * MyPlayer.ModPlayer(player).KiDamage);
        }
        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = crit + MyPlayer.ModPlayer(player).KiCrit;
        }
        public override float UseTimeMultiplier(Player player)
        {
            return MyPlayer.ModPlayer(player).KiSpeedAddition;
        }

        public int RealKiDrain(Player player)
        {
            return (int)(KiDrain * MyPlayer.ModPlayer(player).KiDrainMulti);
        }
        public override bool CanUseItem(Player player)
        {
            if (RealKiDrain(Main.LocalPlayer) <= MyPlayer.ModPlayer(player).KiCurrent)
            {
                MyPlayer.ModPlayer(player).KiCurrent -= RealKiDrain(Main.LocalPlayer);
                return true;
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if(RealKiDrain(Main.LocalPlayer) > 0)
            {
                TooltipLine Indicate = new TooltipLine(mod, "", "");
                string[] Text = Indicate.text.Split(' ');
                Indicate.text = "Consumes " + RealKiDrain(Main.LocalPlayer) + " Ki ";
                Indicate.overrideColor = new Color(34, 232, 222);
                tooltips.Add(Indicate);
            }
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] splitText = tt.text.Split(' ');
                string damageValue = splitText.First();
                string damageWord = splitText.Last();
                tt.text = damageValue + " ki " + damageWord;
            }
            if(WeaponType != null)
            {
                TooltipLine Indicate2 = new TooltipLine(mod, "", "");
                string[] Text2 = Indicate2.text.Split(' ');
                Indicate2.text = WeaponType + " Technique ";
                Indicate2.overrideColor = new Color(232, 202, 34);
                tooltips.Add(Indicate2);
            }
            if (item.damage > 0)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.mod == "Terraria" && line.Name == "Tooltip")
                    {
                        line.overrideColor = Color.Cyan;
                    }
                }
            }
        }
    }
    public abstract class KiPotion : ModItem
    {
        public int KiHeal;
        public int potioncooldown = 3600;
        public bool IsKiPotion;
        public override bool CloneNewInstances
        {
            get { return true; }
        }
        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).KiCurrent += KiHeal;
            player.AddBuff(mod.BuffType("KiPotionSickness"), potioncooldown);
            CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), KiHeal, false, false);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(mod.BuffType("KiPotionSickness")))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public abstract class PatreonItem : ModItem
    {
        public bool IsArmorPiece;
        public bool IsItem;
        public string PatreonName;
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine Indicate2 = new TooltipLine(mod, "", "");
            string[] Text2 = Indicate2.text.Split(' ');
            Indicate2.text = PatreonName + "'s Item";
            Indicate2.overrideColor = new Color(232, 169, 34);
            tooltips.Add(Indicate2);
        }
    }
}
