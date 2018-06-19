using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD
{
    class DBZMODNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        private Player player;
        public override void SetDefaults(NPC npc)
        {
            if (npc.boss && DBZWorld.RealismMode)
            {
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    npc.lifeMax = npc.lifeMax * 2;
                }
                else if (npc.type == NPCID.EaterofWorldsHead)
                {
                    npc.lifeMax = npc.lifeMax * 3;
                }
                else if (npc.type == NPCID.EaterofWorldsBody)
                {
                    npc.lifeMax = npc.lifeMax * 3;
                }
                else if (npc.type == NPCID.EaterofWorldsTail)
                {
                    npc.lifeMax = npc.lifeMax * 3;
                }
                else if (npc.type == NPCID.BrainofCthulhu)
                {
                    npc.lifeMax = npc.lifeMax * 3;
                }
                else if (npc.type == NPCID.QueenBee)
                {
                    npc.lifeMax = npc.lifeMax * 4;
                }
                else if (npc.type == NPCID.SkeletronHead)
                {
                    npc.lifeMax = npc.lifeMax * 5;
                }
                else if (npc.type == NPCID.SkeletronHand)
                {
                    npc.lifeMax = npc.lifeMax * 5;
                }
                else if (npc.type == NPCID.WallofFlesh)
                {
                    npc.lifeMax = npc.lifeMax * 7;
                }
                else if (npc.type == NPCID.WallofFleshEye)
                {
                    npc.lifeMax = npc.lifeMax * 7;
                }
                else if (npc.type == NPCID.Spazmatism)
                {
                    npc.lifeMax = npc.lifeMax * 8;
                }
                else if (npc.type == NPCID.Retinazer)
                {
                    npc.lifeMax = npc.lifeMax * 8;
                }
                else if (npc.type == NPCID.SkeletronPrime)
                {
                    npc.lifeMax = npc.lifeMax * 8;
                }
                else if (npc.type == NPCID.TheDestroyer)
                {
                    npc.lifeMax = npc.lifeMax * 8;
                }
                else if (npc.type == NPCID.Plantera)
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == NPCID.Golem)
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == NPCID.GolemHead)
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == NPCID.GolemFistLeft)
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == NPCID.GolemFistRight)
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == NPCID.DukeFishron)
                {
                    npc.lifeMax = npc.lifeMax * 10;
                }
                else if (npc.type == NPCID.CultistBoss)
                {
                    npc.lifeMax = npc.lifeMax * 11;
                }
                else if (npc.type == NPCID.MoonLordCore)
                {
                    npc.lifeMax = npc.lifeMax * 12;
                }
                else if (npc.type == NPCID.MoonLordHand)
                {
                    npc.lifeMax = npc.lifeMax * 12;
                }
                else if (npc.type == NPCID.MoonLordHead)
                {
                    npc.lifeMax = npc.lifeMax * 12;
                }
            }
            base.SetDefaults(npc);
        }
        public override void NPCLoot(NPC npc)
        {    

            if (!Main.hardMode)
            {
                if (!Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneBeach && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCrimson && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDesert && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDungeon && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneHoly && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneMeteor && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOldOneArmy && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSandstorm && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSnow && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerNebula && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerSolar && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerStardust && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerVortex && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUndergroundDesert && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight)        //this is where you choose what biome you whant the item to drop. ZoneCorrupt is in Corruption
                {
                    if (Main.rand.Next(2) == 0)      
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StableKiCrystal"), Main.rand.Next(1, 5));

                    }
                }
            }
            if (!Main.hardMode && NPC.downedBoss1)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CalmKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (!Main.hardMode && NPC.downedBoss3)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HonorKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (Main.hardMode)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneHoly)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PridefulKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (Main.hardMode && NPC.downedMechBossAny)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCrimson)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (Main.hardMode && NPC.downedMechBossAny)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            //THIS IS AN EXAMPLE HOW TO ADD A SECOND DORP
            //if (Main.rand.Next(2) == 0) //this is the item rarity, so 2 is 1 in 3 chance that the npc will drop the item.
            //{
            //{
            //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GunName"), 1); //this is where you set what item to drop, mod.ItemType("GunName") is an example of how to add your custom item. and 1 is the amount
            //}
            //}
            //}
            //}
            //else    //else if it's not hardmode this will happen
            // {
            //if (Main.player[Main.myPlayer].ZoneCorrupt)  //so again if the player is in corruption
            //{
            //if (Main.rand.Next(2) == 0)    //   the item has a 1 in 3 chance to drop
            //{
            //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.VileMushroom, Main.rand.Next(5, 8));
            //}
            //}

            //}
            //THIS IS AN EXAMPLE OF HOW TO MAKE ITEMS DROP FROM VANILA NPCs
            if (!Main.expertMode)
            {
                if (npc.boss)   //this is where you choose the npc you want
                {
                    if (Main.rand.Next(3) == 0) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SenzuBean"), Main.rand.Next(1, 3)); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
                if (npc.type == NPCID.EyeofCthulhu)   //this is where you choose the npc you want
                {
                    if ((Main.rand.Next(3) == 0)) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment1"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }

                }

                if (npc.type == NPCID.SkeletronHead)   //this is where you choose the npc you want
                {
                    if ((Main.rand.Next(3) == 0)) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment2"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
                if (npc.type == NPCID.Spazmatism)   //this is where you choose the npc you want
                {
                    if ((Main.rand.Next(3) == 0)) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment3"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
                if (npc.type == NPCID.Plantera)   //this is where you choose the npc you want
                {
                    if ((Main.rand.Next(3) == 0)) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment4"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
                if (npc.type == NPCID.CultistBoss)   //this is where you choose the npc you want
                {
                    if ((Main.rand.Next(3) == 0)) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment5"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.EyeofCthulhu)   //this is where you choose the npc you want
                {
                    if (Main.rand.Next(3) == 0) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragmentFirst"), 1);

                    }

                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.SkeletronHead)   //this is where you choose the npc you want
                {
                    if (Main.rand.Next(3) == 0) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment1"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.SkeletronPrime)   //this is where you choose the npc you want
                {
                    if (Main.rand.Next(3) == 0) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment2"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.Golem)   //this is where you choose the npc you want
                {
                    if (Main.rand.Next(3) == 0) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment3"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.MoonLordCore)   //this is where you choose the npc you want
                {
                    if (Main.rand.Next(3) == 0) //this is the item rarity, so 4 is 1 in 5 chance that the npc will drop the item.
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment4"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
                        }
                    }
                }
            }
        }


            public override void SetupShop(int type, Chest shop, ref int nextSlot)
            {
                if (type == NPCID.Merchant)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType<Items.ScrapMetal>());
                    nextSlot++;
                }
            }
        //THIS IS AN EXAMPLE OF HOW TO MAKE ITEMS DROP FROM NPCs IN CUSTOM BIOME
        //if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].GetModPlayer<MyPlayer>(mod).ZoneCustomBiome) //change MyPlayer with your myplayer.cs name and ZoneCustomBiome with your zone name
        //{
        //if (Main.rand.Next(2) == 0) //this is the item rarity, so 2 is 1 in 3 chance that the npc will drop the item.
        //{
        //{
        //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofMight, 1); //this is where you set what item to drop
        //  }
        // }
        //}

        //THIS IS AN EXAMPLE OF HOW TO MAKE ITEMS DROP FROM CUSTOM NPCs

        //if (npc.type == mod.NPCType("GelmalineSlime"))//this is how you add your custom npc
        //{
        //if (Main.rand.Next(3) == 0) //this is the item rarity, so 2 is 1 in 3 chance that the npc will drop the item.
        //{
        //{
        // Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GelmalineOre"), Main.rand.Next(3, 12)); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
        //}
        // }
        //}
        //if (npc.type == mod.NPCType("OrangeSlime"))//this is how you add your custom npc
        //{
        //if (Main.rand.Next(20) == 0) //this is the item rarity, so 2 is 1 in 3 chance that the npc will drop the item.
        //{
        //{
        //  Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlazingCore"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
        //}
        // }
        // }
        //if (npc.type == mod.NPCType("CyanSlime"))//this is how you add your custom npc
        //{
        // if (Main.rand.Next(20) == 0)  //this is the item rarity, so 2 is 1 in 3 chance that the npc will drop the item.
        //{
        //{
        //     Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IcyAmalgam"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
        //}
        //}
        //}
        //if (npc.type == mod.NPCType("TealSlime"))//this is how you add your custom npc
        //{
        // if (Main.rand.Next(20) == 0) //this is the item rarity, so 2 is 1 in 3 chance that the npc will drop the item.
        //{
        //  {
        //    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PoisonPearl"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
        //}
        // }
        // }
        //if (npc.type == mod.NPCType("WhiteSlime"))//this is how you add your custom npc
        //{
        // if (Main.rand.Next(20) == 0) //this is the item rarity, so 2 is 1 in 3 chance that the npc will drop the item.
        //{
        // {
        //    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ArmorShard"), 1); //this is where you set what item to drop, mod.ItemType("CustomSword") is an example of how to add your custom item. and 1 is the amount
        //}
        //}
    }

}

