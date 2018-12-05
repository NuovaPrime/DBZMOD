using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class FlawedPrefix : BasePrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Flawed");  
	    }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 0.86f);
            item.shootSpeed *= 0.94f;
        }

        public override void ApplyKiItemModifier(Item item)
        {
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 10;
            ((KiItem)item.modItem).KiDrain *= 1.10f;
        }
    }
}