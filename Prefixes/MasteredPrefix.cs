using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class MasteredPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mastered");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.14f);
            item.shootSpeed *= 1.04f;

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = -7;
                ((KiItem)item.modItem).kiDrain *= 0.93f;
            }
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult -= 0.07f;
            knockbackMult += 0.08f;
        }
    }
}