using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class FlashyPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Flashy");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.12f);
            item.shootSpeed *= 1.05f;

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 1;
                ((KiItem)item.modItem).KiDrain *= 1.01f;
            }
        }
    }
}