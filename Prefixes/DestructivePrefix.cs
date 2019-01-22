using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class DestructivePrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Destructive");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.15f);

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 6;
                ((KiItem)item.modItem).kiDrain *= 1.06f;
            }
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult += 0.06f;
        }
    }
}