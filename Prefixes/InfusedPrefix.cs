using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class InfusedPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Infused");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.12f);

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODGlobalItem>().kiChangeBonus = 20;
                ((KiItem)item.modItem).kiDrain *= 1.20f;
            }
        }
    }
}