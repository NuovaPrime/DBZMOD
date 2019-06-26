using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class OutrageousPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Outrageous");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.13f);

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 14;
                ((KiItem)item.modItem).kiDrain *= 1.14f;
            }
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            knockbackMult += 0.12f;
        }
    }
}