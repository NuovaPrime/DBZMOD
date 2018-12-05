using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class PoweredPrefix : BasePrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Powered");  
	    }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 1.09f);
        }

        public override void ApplyKiItemModifier(Item item)
        {
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 20;
            ((KiItem)item.modItem).KiDrain *= 1.16f;
        }
    }
}