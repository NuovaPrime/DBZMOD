using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

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
                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 2;
                ((KiItem)item.modItem).KiDrain *= 1.02f;
            }
        }
    }
}