using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class PoweredPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Powered");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.09f);

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 20;
                ((KiItem)item.modItem).KiDrain *= 1.16f;
            }
        }
    }
}