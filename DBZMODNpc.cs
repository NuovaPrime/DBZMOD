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
            Mod thoriummod = ModLoader.GetMod("ThoriumMod");
            Mod calamitymod = ModLoader.GetMod("CalamityMod");

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
                else if(npc.type == thoriummod.NPCType(""))
                {
                    npc.lifeMax = npc.lifeMax * 8;
                }
            }
            base.SetDefaults(npc);
        }
        public override void NPCLoot(NPC npc)
        {    

            if (!Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneBeach && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCrimson && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDesert && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDungeon && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneHoly && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneMeteor && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOldOneArmy && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSandstorm && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSnow && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerNebula && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerSolar && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerStardust && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerVortex && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUndergroundDesert && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight)        //this is where you choose what biome you whant the item to drop. ZoneCorrupt is in Corruption
            {
                if (Main.rand.Next(2) == 0)      
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StableKiCrystal"), Main.rand.Next(1, 5));

                }
            }

            if (NPC.downedBoss2)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CalmKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (NPC.downedBoss3)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneHoly)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PridefulKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (Main.hardMode)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCrimson)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (Main.hardMode)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(1, 5));
                    }
                }
            }
            if (Main.hardMode && NPC.downedPlantBoss)
            {
                if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RadiantKiCrystal"), Main.rand.Next(1, 7));
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.boss)  
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SenzuBean"), Main.rand.Next(1, 3)); 
                        }
                    }
                }
                if (npc.type == NPCID.EyeofCthulhu)  
                {
                    if (Main.rand.Next(3) == 0) 
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment1"), 1); 
                        }
                    }

                }

                if (npc.type == NPCID.SkeletronHead)  
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment2"), 1); 
                        }
                    }
                }
                if (npc.type == NPCID.SkeletronPrime)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment3"), 1); 
                        }
                    }
                }
                if (npc.type == NPCID.Plantera)  
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment4"), 1); 
                        }
                    }
                }
                if (npc.type == NPCID.CultistBoss)  
                {
                    if (Main.rand.Next(3) == 0) 
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment5"), 1); 
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.EyeofCthulhu) 
                {
                    if (Main.rand.Next(3) == 0) 
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragmentFirst"), 1);

                    }

                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.SkeletronHead) 
                {
                    if (Main.rand.Next(3) == 0) 
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment1"), 1); 
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.SkeletronPrime)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment2"), 1);
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.Golem) 
                {
                    if (Main.rand.Next(3) == 0) 
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment3"), 1);
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.MoonLordCore) 
                {
                    if (Main.rand.Next(3) == 0) 
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment4"), 1);
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.WallofFlesh)
                {
                    if (Main.rand.Next(7) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ASSJItem"), 1);
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.TheDestroyer)
                {
                    if (Main.rand.Next(7) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("USSJItem"), 1);
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
    }

}

