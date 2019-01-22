using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class MystifyingPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mystifying");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.07f);

            if (item.modItem != null && item.modItem is KiItem)
            {
                //no mods yet?
            }
        }
    }
}