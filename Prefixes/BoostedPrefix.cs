using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class BoostedPrefix : BasePrefix
    {
        public override void SetDefaults()
	    {
		    DisplayName.SetDefault("Boosted");  
	    }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 1.06f);
            item.shootSpeed *= 1.10f;
        }

        public override void ApplyKiItemModifier(Item item)
        {
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 5;
            ((KiItem)item.modItem).KiDrain *= 1.05f;
        }
    }
}