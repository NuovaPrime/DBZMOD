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
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if ((item.modItem is KiItem && item.damage > 0) && item.maxStack == 1 && rand.NextBool(30))
            {
                return mod.PrefixType("CondensedPrefix");
            }
            return 0;
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