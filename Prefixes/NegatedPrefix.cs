using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class NegatedPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Negated");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 0.68f);

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODGlobalItem>().kiChangeBonus = -20;
                ((KiItem)item.modItem).kiDrain *= 0.80f;
            }
        }
    }
}