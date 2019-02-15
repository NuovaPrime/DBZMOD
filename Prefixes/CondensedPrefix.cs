using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class CondensedPrefix : ModPrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Condensed");
        }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.10f);
            item.shootSpeed *= 0.85f;

            if (item.modItem != null && item.modItem is KiItem)
            {
                //none yet?
            }
        }
    }
}