using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

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
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 20;
                ((KiItem)item.modItem).KiDrain *= 1.20f;
            }
        }
    }
}