using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Traits.Legendary
{
    public sealed class LegendaryTrait : Trait
    {
        public LegendaryTrait() : base("legendary", "Legendary", "You are the saiyan of legend.", 5, nameof(LegendaryTraitBuff))
        {
        }

        public override bool CanSee(MyPlayer player) => NPC.downedBoss1 && player.LSSJAchieved;
    }
}
