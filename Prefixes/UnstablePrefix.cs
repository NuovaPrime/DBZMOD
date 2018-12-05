using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class UnstablePrefix : BasePrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Unstable");  
	    }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 0.90f);
        }

        public override void ApplyKiItemModifier(Item item)
        {
            ((KiItem)item.modItem).KiDrain *= 1.10f;
        }
    }
}