using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DBZMOD
{
    public class DBZMODNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        /*public override void SetDefaults(NPC npc)
        {
            Mod thoriummod = ModLoader.GetMod("ThoriumMod");
            Mod calamitymod = ModLoader.GetMod("CalamityMod");

            if (MyPlayer.RealismMode)
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
                else if (npc.type == thoriummod.NPCType("TheGrandThunderBirdv2"))
                {
                    npc.lifeMax = npc.lifeMax * 2;
                }
                else if (npc.type == thoriummod.NPCType("QueenJelly"))
                {
                    npc.lifeMax = npc.lifeMax * 3;
                }
                else if (npc.type == thoriummod.NPCType("Viscount"))
                {
                    npc.lifeMax = npc.lifeMax * 4;
                }
                else if (npc.type == thoriummod.NPCType("TheBuriedWarrior"))
                {
                    npc.lifeMax = npc.lifeMax * 5;
                }
                else if (npc.type == thoriummod.NPCType("GraniteEnergyStorm"))
                {
                    npc.lifeMax = npc.lifeMax * 5;
                }
                else if (npc.type == thoriummod.NPCType("ThePrimeScouter"))
                {
                    npc.lifeMax = npc.lifeMax * 6;
                }
                else if (npc.type == thoriummod.NPCType("BoreanStrider"))
                {
                    npc.lifeMax = npc.lifeMax * 7;
                }
                else if (npc.type == thoriummod.NPCType("BoreanStriderPopped"))
                {
                    npc.lifeMax = npc.lifeMax * 7;
                }
                else if (npc.type == thoriummod.NPCType("FallenDeathBeholder"))
                {
                    npc.lifeMax = npc.lifeMax * 7;
                }
                else if (npc.type == thoriummod.NPCType("FallenDeathBeholder2"))
                {
                    npc.lifeMax = npc.lifeMax * 7;
                }
                else if (npc.type == thoriummod.NPCType("Lich"))
                {
                    npc.lifeMax = npc.lifeMax * 8;
                }
                else if (npc.type == thoriummod.NPCType("LichHeadless"))
                {
                    npc.lifeMax = npc.lifeMax * 8;
                }
                else if (npc.type == thoriummod.NPCType("Abyssion"))
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == thoriummod.NPCType("AbyssionCracked"))
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == thoriummod.NPCType("AbyssionReleased"))
                {
                    npc.lifeMax = npc.lifeMax * 9;
                }
                else if (npc.type == thoriummod.NPCType("Aquaius"))
                {
                    npc.lifeMax = npc.lifeMax * 13;
                }
                else if (npc.type == thoriummod.NPCType("Aquaius2"))
                {
                    npc.lifeMax = npc.lifeMax * 13;
                }
                else if (npc.type == thoriummod.NPCType("Omnicide"))
                {
                    npc.lifeMax = npc.lifeMax * 13;
                }
                else if (npc.type == thoriummod.NPCType("SlagFury"))
                {
                    npc.lifeMax = npc.lifeMax * 13;
                }
                else if (npc.type == thoriummod.NPCType("RealityBreaker"))
                {
                    npc.lifeMax = npc.lifeMax * 13;
                }
            }
            base.SetDefaults(npc);
        }*/

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == NPCID.Nurse)
            {
                if (firstButton)
                {

                    Player player = Main.LocalPlayer;
                    int kihealvalue = MyPlayer.ModPlayer(player).OverallKiMax() - MyPlayer.ModPlayer(player).GetKi();
                    MyPlayer.ModPlayer(player).AddKi(MyPlayer.ModPlayer(player).OverallKiMax());
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), kihealvalue, false, false);
                    if(!Main.dedServ)
                    {
                        Main.PlaySound(SoundID.MaxMana);
                    }
                }
            }
        }
        public override void NPCLoot(NPC npc)
        {
            // now loops over all connected players and plays the message for anyone who doesn't have it.
            bool isDisplayingJungleMessage = false;
            bool isDisplayingHellMessage = false;
            bool isDisplayingEvilMessage = false;
            bool isDisplayingMushroomMessage = false;
            foreach (Player player in Main.player) {
                MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
                if (npc.type == NPCID.EaterofWorldsHead && npc.boss)
                {
                    if (!modPlayer.JungleMessage)
                    {
                        modPlayer.JungleMessage = true;
                        isDisplayingJungleMessage = true;
                    }
                }
                if (npc.type == NPCID.BrainofCthulhu)
                {
                    if (!modPlayer.JungleMessage)
                    {
                        modPlayer.JungleMessage = true;
                        isDisplayingJungleMessage = true;
                    }
                }
                if (npc.type == NPCID.SkeletronHead)
                {
                    if (!modPlayer.HellMessage)
                    {
                        modPlayer.HellMessage = true;
                        isDisplayingHellMessage = true;
                    }
                }
                if (npc.type == NPCID.WallofFlesh)
                {
                    if (!modPlayer.EvilMessage)
                    {
                        modPlayer.EvilMessage = true;
                        isDisplayingEvilMessage = true;
                    }
                }
                if (npc.type == NPCID.Plantera)
                {
                    if (!modPlayer.MushroomMessage)
                    {
                        modPlayer.MushroomMessage = true;
                        isDisplayingMushroomMessage = true;
                    }
                }
            }

            if (isDisplayingJungleMessage)
            {
                if (Main.netMode != 2)
                {
                    Main.NewText("You feel a calm radiance of ki from the jungle.", 13, 201, 10);
                }
                else
                {
                    NetworkText text2 = NetworkText.FromKey("You feel a calm radiance of ki from the jungle.");
                    NetMessage.BroadcastChatMessage(text2, new Color(13, 201, 10));
                }
            }

            if (isDisplayingHellMessage)
            {
                if (Main.netMode != 2)
                {
                    Main.NewText("The underworld's flames shimmer with energy.", 201, 10, 10);
                }
                else
                {
                    NetworkText text2 = NetworkText.FromKey("The underworld's flames shimmer with energy.");
                    NetMessage.BroadcastChatMessage(text2, new Color(201, 10, 10));
                }
            }

            if (isDisplayingEvilMessage)
            {
                if (Main.netMode != 2)
                {
                    Main.NewText("The world's evil erupts with destructive energy.", 152, 36, 173);
                }
                else
                {
                    NetworkText text2 = NetworkText.FromKey("The world's evil erupts with destructive energy.");
                    NetMessage.BroadcastChatMessage(text2, new Color(152, 36, 173));
                }
            }

            if (isDisplayingMushroomMessage)
            {
                if (Main.netMode != 2)
                {
                    Main.NewText("The glowing mushrooms of the caverns explode with radiance.", 232, 242, 50);
                }
                else
                {
                    NetworkText text2 = NetworkText.FromKey("The glowing mushrooms of the caverns explode with radiance.");
                    NetMessage.BroadcastChatMessage(text2, new Color(232, 242, 50));
                }
            }
            if (npc.damage > 1 && npc.lifeMax > 10 && !npc.friendly)
            {
                if (!Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneBeach && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCrimson && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDungeon && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneHoly && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneMeteor && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOldOneArmy && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSandstorm && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerNebula && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerSolar && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerStardust && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerVortex && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUndergroundDesert && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneRockLayerHeight && !Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDirtLayerHeight)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StableKiCrystal"), Main.rand.Next(1, 2));

                    }
                }

                if (NPC.downedBoss2)
                {
                    if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneJungle)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CalmKiCrystal"), Main.rand.Next(1, 3));
                        }
                    }
                }
                if (NPC.downedBoss3)
                {
                    if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PridefulKiCrystal"), Main.rand.Next(1, 3));
                        }
                    }
                }
                if (Main.hardMode)
                {
                    if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCrimson)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(1, 3));
                        }
                    }
                }
                if (Main.hardMode)
                {
                    if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneCorrupt)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(1, 3));
                        }
                    }
                }
                if (Main.hardMode && NPC.downedPlantBoss)
                {
                    if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PureKiCrystal"), Main.rand.Next(1, 5));
                        }
                    }
                }
            }
            if (npc.boss)
            {
                if (Main.rand.Next(4) == 0)
                {
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SenzuBean"), Main.rand.Next(1, 3));
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment1"), 1);
                        }
                    }

                }

                if (npc.type == NPCID.SkeletronHead)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment2"), 1);
                        }
                    }
                }
                if (npc.type == NPCID.SkeletronPrime)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment3"), 1);
                        }
                    }
                }
                if (npc.type == NPCID.Plantera)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment4"), 1);
                        }
                    }
                }
                if (npc.type == NPCID.CultistBoss)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiFragment5"), 1);
                        }
                    }
                }
                if (npc.type == NPCID.WallofFlesh)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SpiritualEmblem"), 1);
                        }
                    }
                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragmentFirst"), 1);

                    }

                }
            }
            if (!Main.expertMode)
            {
                if (npc.type == NPCID.SkeletronHead)
                {
                    if (Main.rand.Next(4) == 0)
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
                    if (Main.rand.Next(4) == 0)
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
                    if (Main.rand.Next(4) == 0)
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
                    if (Main.rand.Next(4) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KaioFragment4"), 1);
                        }
                    }
                }
            }
            if (NPC.downedQueenBee)
            {
                if (npc.type == NPCID.Harpy)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AstralEssentia"), Main.rand.Next(1, 3));
                        }
                    }
                }
            }
            if (NPC.downedBoss3)
            {
                if (npc.type == NPCID.AngryBones || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigHelmet || npc.type == NPCID.AngryBonesBigMuscle || npc.type == NPCID.DarkCaster)
                {
                    if (Main.rand.Next(5) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SkeletalEssence"), Main.rand.Next(1, 3));
                        }
                    }
                }
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                if (npc.type == NPCID.CrimsonAxe || npc.type == NPCID.PossessedArmor || npc.type == NPCID.EnchantedSword || npc.type == NPCID.CursedHammer)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SoulofEntity"), Main.rand.Next(1, 6));
                        }
                    }
                }
            }
            if (NPC.downedGolemBoss)
            {
                if (npc.type == NPCID.RedDevil || npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DemonicSoul"), Main.rand.Next(1, 3));
                        }
                    }
                }
            }

            if (!NPC.downedBoss1)
            {
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StableKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedBoss2)
            {
                if (npc.type == NPCID.BrainofCthulhu || (npc.type == NPCID.EaterofWorldsHead && npc.boss))
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StableKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedBoss3)
            {
                if (npc.type == NPCID.SkeletronHead)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CalmKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedQueenBee)
            {
                if (npc.type == NPCID.QueenBee)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CalmKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!Main.hardMode)
            {
                if (npc.type == NPCID.WallofFlesh)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PridefulKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedMechBossAny)
            {
                if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer || npc.type == NPCID.TheDestroyer)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedPlantBoss)
            {
                if (npc.type == NPCID.Plantera)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AngerKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedGolemBoss)
            {
                if (npc.type == NPCID.Golem)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PureKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedFishron)
            {
                if (npc.type == NPCID.DukeFishron)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PureKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (!NPC.downedAncientCultist)
            {
                if (npc.type == NPCID.CultistBoss)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PureKiCrystal"), Main.rand.Next(6, 18));
                }
            }
            if (NPC.downedBoss2)
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int) npc.position.X, (int) npc.position.Y, npc.width, npc.height,
                        mod.ItemType("EarthenShard"), Main.rand.Next(1, 3));
                }
            }
            if (Main.hardMode)
            {
                if (npc.type == NPCID.LunarTowerSolar || npc.type == NPCID.LunarTowerNebula || npc.type == NPCID.LunarTowerVortex || npc.type == NPCID.LunarTowerStardust)
                {
                    int stacks = Main.rand.Next(6, 12) / 2;
                    if (Main.expertMode)
                    {
                        stacks = (int)(stacks * 1.5f);
                    }
                    for (int i = 0; i < stacks; i++)
                    {
                        Item.NewItem((int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, mod.ItemType("RadiantFragment"), Main.rand.Next(1, 4));
                    }
                }
            }
            if (Main.hardMode)
            {
                if (npc.type == NPCID.LunarTowerSolar || npc.type == NPCID.LunarTowerNebula || npc.type == NPCID.LunarTowerVortex || npc.type == NPCID.LunarTowerStardust)
                {
                    if (Main.rand.Next(10) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SSJGUnlockItem"));
                    }
                }
            }
            if (NPC.downedBoss3)
            {
                if (npc.type == NPCID.GraniteGolem)
                {
                    if (Main.rand.Next(5) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BaldurEssentia"));
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
            if (type == NPCID.TravellingMerchant)
            {
                if (Main.rand.Next(5) == 0)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Accessories.ArmCannon>());
                    nextSlot++;
                }
                if (Main.rand.Next(3) == 0)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Consumables.MRE>());
                    nextSlot++;
                }
            }
            if (type == NPCID.SkeletonMerchant)
            {
                if (Main.rand.Next(3) == 0)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Consumables.DisgustingGoop>());
                    nextSlot++;
                }
            }
            if (type == NPCID.ArmsDealer && Main.bloodMoon && Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Weapons.Tier_4.BlightedFang>());
                nextSlot++;
            }
            if (type == NPCID.Dryad && NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SuspiciousLookingEye);
                nextSlot++;
            }
            if (type == NPCID.Dryad && NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.ClothierVoodooDoll);
                nextSlot++;
            }
            if (type == NPCID.Dryad && NPC.downedGolemBoss)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LihzahrdPowerCell);
                nextSlot++;
            }

            if (type == NPCID.Steampunker)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Accessories.LuminousSectum>());
                nextSlot++;
            }

        }
    }

}

