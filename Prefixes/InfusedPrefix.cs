using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class InfusedPrefix : BasePrefix
    {
        public override void SetDefaults()
	    {
		    DisplayName.SetDefault("Infused");  
	    }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 1.12f);
        }

        public override void ApplyKiItemModifier(Item item)
        {
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 20;
            ((KiItem)item.modItem).KiDrain *= 1.20f;
        }
    }
}