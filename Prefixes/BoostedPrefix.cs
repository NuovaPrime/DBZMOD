using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class BoostedPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Boosted");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.06f);
            item.shootSpeed *= 1.10f;

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 5;
                ((KiItem)item.modItem).kiDrain *= 1.05f;
            }
        }
    }
}