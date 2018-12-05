using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class MystifyingPrefix : BasePrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Mystifying");  
	    }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 1.07f);
        }

        public override void ApplyKiItemModifier(Item item)
        {
            //no mods yet?
        }
    }
}