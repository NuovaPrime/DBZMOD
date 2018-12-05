using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class UnstablePrefix : ModPrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Unstable");  
	    }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 0.90f);

            if (item.modItem != null && item.modItem is KiItem)
            {
                ((KiItem)item.modItem).KiDrain *= 1.10f;
            }
        }
    }
}