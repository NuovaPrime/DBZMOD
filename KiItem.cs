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
