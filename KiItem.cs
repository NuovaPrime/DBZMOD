using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using System;
using DBZMOD.Extensions;
using DBZMOD.Players;
using DBZMOD.Util;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

namespace DBZMOD
{
    public abstract class KiItem : ModItem
    {
        internal Player player;
        private NPC _npc;
        public bool isFistWeapon;
        public bool canUseHeavyHit;
        public float kiDrain;
        public string weaponType;
        #region Boss bool checks
        public bool eyeDowned;
        public bool beeDowned;
        public bool wallDowned;
        public bool plantDowned;
        public bool dukeDowned;
        public bool moodlordDowned;
        public override void PostUpdate()
        {
            if (NPC.downedBoss1)
            {
                eyeDowned = true;
            }
            if (NPC.downedQueenBee)
            {
                beeDowned = true;
            }
            if (Main.hardMode)
            {
                wallDowned = true;
            }
            if (NPC.downedPlantBoss)
            {
                plantDowned = true;
            }
            if (NPC.downedFishron)
            {
                dukeDowned = true;
            }
            if (NPC.downedMoonlord)
            {
                moodlordDowned = true;
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
            var prefixChooser = new WeightedRandom<int>();
            prefixChooser.Add(mod.PrefixType("BalancedPrefix"), 3); // 3 times as likely
            prefixChooser.Add(mod.PrefixType("CondensedPrefix"), 3);
            prefixChooser.Add(mod.PrefixType("MystifyingPrefix"), 3);
            prefixChooser.Add(mod.PrefixType("UnstablePrefix"), 3);
            prefixChooser.Add(mod.PrefixType("FlawedPrefix"), 3);
            prefixChooser.Add(mod.PrefixType("BoostedPrefix"), 3);
            prefixChooser.Add(mod.PrefixType("NegatedPrefix"), 3);
            prefixChooser.Add(mod.PrefixType("OutrageousPrefix"), 3);
            prefixChooser.Add(mod.PrefixType("PoweredPrefix"), 2);
            prefixChooser.Add(mod.PrefixType("FlashyPrefix"), 2);
            prefixChooser.Add(mod.PrefixType("InfusedPrefix"), 2);
            prefixChooser.Add(mod.PrefixType("DistractingPrefix"), 2);
            prefixChooser.Add(mod.PrefixType("DestructivePrefix"), 2);
            prefixChooser.Add(mod.PrefixType("MasteredPrefix"), 1);
            prefixChooser.Add(mod.PrefixType("TranscendedPrefix"), 1);
            int choice = prefixChooser;
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
            knockback = knockback + MyPlayer.ModPlayer(player).kiKbAddition;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {            
            damage = (int)Math.Ceiling(damage * MyPlayer.ModPlayer(player).KiDamage);
        }

        public override bool? CanHitNPC(Player player, NPC target)
        {
            if (this is BaseBeamItem)
            {
                return false;
            }
            return base.CanHitNPC(player, target);
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {            
            crit = crit + MyPlayer.ModPlayer(player).kiCrit;
        }

        public override float UseTimeMultiplier(Player player)
        {
            return MyPlayer.ModPlayer(player).kiSpeedAddition;
        }

        public int RealKiDrain(Player player)
        {
            return (int)(kiDrain * MyPlayer.ModPlayer(player).kiDrainMulti);
        }

        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).HasKi(RealKiDrain(player)))
            {
                MyPlayer.ModPlayer(player).AddKi(-RealKiDrain(player), true, false);
                return true;
            }
            return false;
        }

        public override bool UseItem(Player player)
        {
            if(player.IsLSSJ() && !player.IsSSJ1())
            {
                int i = Main.rand.Next(1, 4);
                MyPlayer.ModPlayer(player).overloadCurrent += i;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine indicate = new TooltipLine(mod, "", "");
            string[] text = indicate.text.Split(' ');
            indicate.text = "Consumes " + RealKiDrain(Main.LocalPlayer) + " Ki ";
            indicate.overrideColor = new Color(34, 232, 222);
            tooltips.Add(indicate);
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] splitText = tt.text.Split(' ');
                string damageValue = splitText.First();
                string damageWord = splitText.Last();
                tt.text = damageValue + " ki " + damageWord;
            }
            TooltipLine indicate2 = new TooltipLine(mod, "", "");
            string[] text2 = indicate.text.Split(' ');
            indicate2.text = weaponType + " Technique ";
            indicate2.overrideColor = new Color(232, 202, 34);
            tooltips.Add(indicate2);
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
    
    public abstract class PatreonItem : ModItem
    {
        public bool isArmorPiece;
        public bool isItem;
        public string patreonName;
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine indicate2 = new TooltipLine(mod, "", "");
            string[] text2 = indicate2.text.Split(' ');
            indicate2.text = patreonName + "'s Item";
            indicate2.overrideColor = new Color(232, 169, 34);
            tooltips.Add(indicate2);
        }
    }
}
