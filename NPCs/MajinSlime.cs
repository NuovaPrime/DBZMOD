using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.NPCs
{
	public class MajinSlime : ModNPC
	{
        private int _majinRegentimer;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Majin Slime");
			Main.npcFrameCount[npc.type] = 2;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 33;
			npc.damage = 62;
			npc.defense = 22;
			npc.lifeMax = 240;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 800f;
			npc.knockBackResist = 0.7f;
			npc.aiStyle = 1;
			aiType = NPCID.BlackSlime;
			animationType = NPCID.BlackSlime;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
		    if (NPC.downedAncientCultist)
		    {
		        return SpawnCondition.OverworldNightMonster.Chance * 0.05f;
		    }
		    else
		    {
                return SpawnCondition.OverworldNightMonster.Chance * 0f;
            }
		}

        public override void AI()
        {
            _majinRegentimer++;
            if(_majinRegentimer > 14 && npc.life <= npc.lifeMax)
            {
                npc.life += 1;
                _majinRegentimer = 0;
                npc.netUpdate = true;
            }
            base.AI();
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(20) == 0)
            {
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MajinNucleus"));
                }
            }
        }
    }
}
