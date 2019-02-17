using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Prefixes
{
    public class BalancedPrefix : ModPrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Balanced");  
	    }

        public override void Apply(Item item)
        {
            item.damage = (int)(item.damage * 1.05f);
        
            if (item.modItem != null && item.modItem is KiItem)
            {
                item.GetGlobalItem<DBZMODGlobalItem>().kiChangeBonus = 2;
                ((KiItem)item.modItem).kiDrain *= 1.02f;
            }
        }
    }
}