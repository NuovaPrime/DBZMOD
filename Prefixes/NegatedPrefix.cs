using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

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
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = -20;
                ((KiItem)item.modItem).KiDrain *= 0.80f;
            }
        }
    }
}