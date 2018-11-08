﻿using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace DBZMOD
{
    public class DBZMODItem : GlobalItem
    {
        public int kiChangeBonus;
        public int speedChangeBonus;
        public int maxChargeBonus;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public DBZMODItem()
        {
            kiChangeBonus = 0;
            speedChangeBonus = 0;
            maxChargeBonus = 0;
        }
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            DBZMODItem DBZClone = (DBZMODItem)base.Clone(item, itemClone);
            DBZClone.kiChangeBonus = kiChangeBonus;
            DBZClone.speedChangeBonus = speedChangeBonus;
            DBZClone.maxChargeBonus = maxChargeBonus;
            return DBZClone;
        }
        
        //Broken right now, smh
        /*public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if ((item.modItem is KiItem && item.damage > 0) && item.maxStack == 1 && rand.NextBool(10))
            {
                return mod.PrefixType("CondensedPrefix");
            }
            if ((item.modItem is KiItem && item.damage > 0) && item.maxStack == 1 && rand.NextBool(60))
            {
                return mod.PrefixType("MystifyingPrefix");
            }
            if ((item.modItem is KiItem && item.damage > 0) && item.maxStack == 1 && rand.NextBool(30))
            {
                return mod.PrefixType("UnstablePrefix");
            }
            if ((item.modItem is KiItem && item.damage > 0) && item.maxStack == 1 && rand.NextBool(10))
            {
                return mod.PrefixType("BalancedPrefix");
            }
            if ((item.modItem is KiItem && item.damage > 0) && item.maxStack == 1 && rand.NextBool(5))
            {
                return mod.PrefixType("MasteredPrefix");
            }
            return -1;
        }*/
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!item.social && item.prefix > 0)
			{
                int index = tooltips.FindLastIndex(t => (t.mod == "Terraria" || t.mod == mod.Name) && (t.isModifier || t.Name.StartsWith("Tooltip")));
				if (kiChangeBonus > 0)
				{
					TooltipLine line = new TooltipLine(mod, "PrefixKiChange", "+" + kiChangeBonus + "% More Ki Usage");
					line.isModifier = true;
                    line.isModifierBad = true;
					tooltips.Insert(index, line);
				}
                if (kiChangeBonus < 0)
				{
					TooltipLine line = new TooltipLine(mod, "PrefixKiChange", kiChangeBonus + "% Less Ki Usage");
					line.isModifier = true;
					tooltips.Insert(index, line);
				}
                if (speedChangeBonus > 0)
				{
					TooltipLine line = new TooltipLine(mod, "PrefixSpeedChange", "+" + speedChangeBonus + "% More cast speed");
					line.isModifier = true;
					tooltips.Insert(index, line);
				}
                if (speedChangeBonus < 0)
				{
					TooltipLine line = new TooltipLine(mod, "PrefixSpeedChange", speedChangeBonus + "% Less cast speed");
					line.isModifier = true;
                    line.isModifierBad = true;
					tooltips.Insert(index, line);
				}
                if (maxChargeBonus > 0)
				{
					TooltipLine line = new TooltipLine(mod, "PrefixKiChange", "+" + maxChargeBonus + " Maximum charges");
					line.isModifier = true;
					tooltips.Insert(index, line);
				}
            }
        } 
           
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (Main.rand.Next(4) == 0)
            {
                if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KaioFragmentFirst"));
                }
                if (context == "bossBag" && arg == ItemID.SkeletronBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KaioFragment1"));
                }
                if (context == "bossBag" && arg == ItemID.SkeletronPrimeBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KaioFragment2"));
                }
                if (context == "bossBag" && arg == ItemID.GolemBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KaioFragment3"));
                }
                if (context == "bossBag" && arg == ItemID.MoonLordBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KaioFragment4"));
                }
                if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KiFragment1"));
                }
                if (context == "bossBag" && arg == ItemID.SkeletronBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KiFragment2"));
                }
                if (context == "bossBag" && arg == ItemID.SkeletronPrimeBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KiFragment3"));
                }
                if (context == "bossBag" && arg == ItemID.GolemBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KiFragment4"));
                }
                if (context == "bossBag" && arg == ItemID.MoonLordBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("KiFragment5"));
                }
                if (context == "bossBag" && arg == ItemID.WallOfFleshBossBag)
                {
                    player.QuickSpawnItem(mod.ItemType("SpiritualEmblem"));
                }
            }
        }
    }
}