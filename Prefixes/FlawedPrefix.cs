using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class FlawedPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Flawed");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 0.86f);
            item.shootSpeed *= 0.94f;

            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 10;
                ((KiItem)item.modItem).KiDrain *= 1.10f;
            }
        }
    }
}