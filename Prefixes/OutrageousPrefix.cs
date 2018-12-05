using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class OutrageousPrefix : BasePrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Outrageous");
        }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 1.13f);
        }

        public override void ApplyKiItemModifier(Item item)
        {
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 14;
            ((KiItem)item.modItem).KiDrain *= 1.14f;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            knockbackMult += 0.12f;
        }
    }
}