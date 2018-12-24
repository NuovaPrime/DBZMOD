﻿using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameInput;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using DBZMOD;
using System;
using Util;
using Network;

namespace DBZMOD
{
    public abstract class KiProjectile : ModProjectile
    {
        public static float DefaultBeamKnockback = 1.5f;
        public int ChargeLevel;
        public int ChargeTimer;
        public float ChargeTimerMax;
        public int ChargeLimit = 4;
        public int FinalChargeLimit = 4;
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
            FinalChargeLimit = ChargeLimit + MyPlayer.ModPlayer(player).ChargeLimitAdd;
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
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
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

                // if the player is channeling, increment the timer and apply some slowdown
                if (player.channel && projectile.active)
                {
                    ChargeTimer++;
                    ApplyChannelingSlowdown(player);
                }

                //ChargeTimerMax -= MyPlayer.ModPlayer(player).chargeTimerMaxAdd;

                if (ChargeTimer > ChargeTimerMax && ChargeLevel < FinalChargeLimit)
                {
                    ChargeLevel += 1;
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), ChargeLevel, false, false);
                    ChargeTimer = 0;
                }
                if (Main.time > 0 && Math.Ceiling(Main.time % 2) == 0 && !MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    MyPlayer.ModPlayer(player).AddKi(-KiDrainRate);
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

        public static void ApplyChannelingSlowdown(Player player)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            if (modPlayer.IsFlying)
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                float yVelocity = 0f;
                if (modPlayer.IsDownHeld || modPlayer.IsUpHeld)
                {
                    yVelocity = player.velocity.Y / (1.2f - chargeMoveSpeedBonus);
                } else
                {
                    yVelocity = Math.Min(-0.4f, player.velocity.Y / (1.2f - chargeMoveSpeedBonus));
                }
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), yVelocity);
            }
            else
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                // don't neuter falling - keep the positive Y velocity if it's greater - if the player is jumping, this reduces their height. if falling, falling is always greater.                        
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), Math.Max(player.velocity.Y, player.velocity.Y / (1.2f - chargeMoveSpeedBonus)));
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
            if (MyPlayer.ModPlayer(player).infuserSapphire)
            {
                npc.AddBuff(BuffID.Frostburn, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserRuby)
            {
                npc.AddBuff(BuffID.OnFire, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserEmerald)
            {
                npc.AddBuff(BuffID.Poisoned, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserDiamond)
            {
                npc.AddBuff(BuffID.Confused, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserAmethyst)
            {
                npc.AddBuff(BuffID.ShadowFlame, 300);
            }
            if (MyPlayer.ModPlayer(player).infuserTopaz)
            {
                npc.AddBuff(BuffID.OnFire, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserAmber)
            {
                npc.AddBuff(BuffID.Ichor, 300);
            }
            if (MyPlayer.ModPlayer(player).infuserRainbow)
            {
                npc.AddBuff(BuffID.Ichor, 300);
                npc.AddBuff(BuffID.OnFire, 180);
                npc.AddBuff(BuffID.ShadowFlame, 300);
                npc.AddBuff(BuffID.Confused, 180);
                npc.AddBuff(BuffID.Poisoned, 180);
                npc.AddBuff(BuffID.Bleeding, 180);
                npc.AddBuff(BuffID.Frostburn, 180);

            }
        }
    }    

    public abstract class KiItem : ModItem
    {
        internal Player player;
        private NPC npc;
        public bool IsFistWeapon;
        public bool CanUseHeavyHit;
        public float KiDrain;
        public string WeaponType;
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
            if (MyPlayer.ModPlayer(player).HasKi(RealKiDrain(player)))
            {
                MyPlayer.ModPlayer(player).AddKi(-RealKiDrain(player));
                return true;
            }
            return false;
        }

        public override bool UseItem(Player player)
        {
            if(Transformations.IsLSSJ(player) && !Transformations.IsSSJ1(player))
            {
                int i = Main.rand.Next(1, 4);
                MyPlayer.ModPlayer(player).OverloadCurrent += i;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine Indicate = new TooltipLine(mod, "", "");
            string[] Text = Indicate.text.Split(' ');
            Indicate.text = "Consumes " + RealKiDrain(Main.LocalPlayer) + " Ki ";
            Indicate.overrideColor = new Color(34, 232, 222);
            tooltips.Add(Indicate);
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] splitText = tt.text.Split(' ');
                string damageValue = splitText.First();
                string damageWord = splitText.Last();
                tt.text = damageValue + " ki " + damageWord;
            }
            TooltipLine Indicate2 = new TooltipLine(mod, "", "");
            string[] Text2 = Indicate.text.Split(' ');
            Indicate2.text = WeaponType + " Technique ";
            Indicate2.overrideColor = new Color(232, 202, 34);
            tooltips.Add(Indicate2);
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
            MyPlayer.ModPlayer(player).AddKi(KiHeal);
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
