using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace DBZMOD.NPCs
{
	[AutoloadHead]
	public class MasterRoshi : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "DBZMOD/NPCs/MasterRoshi";
			}
		}

		public override bool Autoload(ref string name)
		{
			name = "Master Roshi";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			DisplayName.SetDefault("Master Roshi");
			Main.npcFrameCount[npc.type] = 22;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 3;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 40;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
		}

		/*public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (player.active)
				{
					for (int j = 0; j < player.inventory.Length; j++)
					{
						if (player.inventory[j].type == mod.ItemType("ExampleItem") || player.inventory[j].type == mod.ItemType("ExampleBlock"))
						{
							return true;
						}
					}
				}
			}
			return false;
		}*/

		/*public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			int score = 0;
			for (int x = left; x <= right; x++)
			{
				for (int y = top; y <= bottom; y++)
				{
					int type = Main.tile[x, y].type;
					if (type == mod.TileType("ExampleBlock") || type == mod.TileType("ExampleChair") || type == mod.TileType("ExampleWorkbench") || type == mod.TileType("ExampleBed") || type == mod.TileType("ExampleDoorOpen") || type == mod.TileType("ExampleDoorClosed"))
					{
						score++;
					}
					if (Main.tile[x, y].wall == mod.WallType("ExampleWall"))
					{
						score++;
					}
				}
			}
			return score >= (right - left) * (bottom - top) / 2;
		}*/

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(4))
			{
                default:
                    return "";
			}
		}

		public override void FindFrame(int frameHeight)
		{
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat()
		{
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            if (modPlayer.IsPrimal() && Main.rand.Next(4) == 0) //If the player is a primal saiyan
			{
				return "Is that a tail? Could you be a saiyan? I haven't seen one in ages, its a nostalgic sight.";
			}
            if(!player.Male && Main.rand.Next(4) == 0) //If the player is a girl
            {
                return "Ah what a nice figure you have there, if you could allow me to have a peek at your body then perhaps I could assist you in your travels.";
            }
			switch (Main.rand.Next(3))
			{
				case 0:
					return "Oh, how interesting, I sense incredible power from you.";
				case 1:
					return "You seem to have latent untapped potential, perhaps I could whip you into shape.";
				default:
					return "If you could get me some new 'material' then maybe I could assist you in your training.";
			}
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.64");
            button2 = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            Main.npcChatCornerItem = 0;
            if (firstButton)
            {

                CheckQuests();

            }
            else
            {
                shop = true;
            }
        }

        void CheckQuests()
        {
            
            string NoQuest1 = "Sorry, that's all the requests I have for today.";
            string NoQuest2 = "Come back tommorow, I should have more jobs for you to do.";
            string NoQuest3 = "That's all I have for you to do, come back later.";
            foreach (Player player in Main.player)
            {
                if (player.active && player.talkNPC == npc.whoAmI)
                {
                    var questSystem = player.GetModPlayer<RoshiQuests>(mod);

                    if (questSystem.QuestsCompletedToday >= questSystem.QuestLimitPerDay)
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Main.npcChatText = NoQuest1; return;
                            case 1:
                                Main.npcChatText = NoQuest2; return;
                            default:
                                Main.npcChatText = NoQuest3; return;
                        }
                    }
                    else if (questSystem.CurrentQuest == -1)
                    {
                        int NewQuest = RoshiQuests.ChooseNewQuest();
                        Main.npcChatText = RoshiQuests.Quests[NewQuest].ToString();
                        if (RoshiQuests.Quests[NewQuest] is ItemQuest)
                        {
                            Main.npcChatCornerItem = (RoshiQuests.Quests[NewQuest] as ItemQuest).ItemType;
                            questSystem.CurrentQuest = NewQuest;
                        }
                        return;
                    }
                    else

                    if (questSystem.CheckQuest())
                    {
                        Main.npcChatText = questSystem.GetCurrentQuest().SayThanks();

                        Main.PlaySound(12, -1, -1, 1);
                        questSystem.SpawnReward(npc);
                        questSystem.CompleteQuest();
                        return;
                    }
                    else
                    {
                        Main.npcChatText = questSystem.GetCurrentQuest().ToString();
                        if (questSystem.GetCurrentQuest() is ItemQuest)
                        {
                            Main.npcChatCornerItem = (questSystem.GetCurrentQuest() as ItemQuest).ItemType;
                        }
                    }

                }
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(mod.ItemType("KiBlast"));
            shop.item[nextSlot].value = 10000;
			nextSlot++;
			if (NPC.downedBoss2)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("Kamehameha"));
                shop.item[nextSlot].value = 30000;
				nextSlot++;
			}
            if (NPC.downedQueenBee)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("HermitGi"));
                shop.item[nextSlot].value = 50000;
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("HermitPants"));
                shop.item[nextSlot].value = 50000;
                nextSlot++;
            }
        }

        /*public override void NPCLoot()
		{
			Item.NewItem(npc.getRect(), mod.ItemType<Items.Armor.ExampleCostume>());
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = mod.ProjectileType("SparklingBall");
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}*/
    }
}