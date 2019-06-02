using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.NPCs
{
	public class SaibaBlue : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kaiwareman");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 10;
			npc.height = 14;
			npc.damage = 24;
			npc.defense = 38;
			npc.lifeMax = 465;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 60f;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 3;
			aiType = NPCID.Zombie;
			animationType = NPCID.Zombie;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
		    if (NPC.downedBoss1)
		    {
		        return SpawnCondition.OverworldDaySlime.Chance * 0.08f;
		    }
		    else
		    {
                return SpawnCondition.OverworldDaySlime.Chance * 0f;
            }
		}
        public override void NPCLoot()
        {
            if (Main.rand.Next(10) == 0)
            {
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PridefulKiCrystal"));
                }
            }
        }
    }
}
