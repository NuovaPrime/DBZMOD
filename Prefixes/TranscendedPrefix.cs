using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class TranscendedPrefix : BasePrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Transcended");
        }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 1.16f);
        }

        public override void ApplyKiItemModifier(Item item)
        {
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = -13;
            ((KiItem)item.modItem).KiDrain *= 0.87f;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult -= 0.10f;
            knockbackMult += 0.14f;
        }
    }
}