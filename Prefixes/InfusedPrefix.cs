using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class InfusedPrefix : ModPrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Infused");  
	    }
        public override float RollChance(Item item)
        {
            return 3f;
        }
        public override bool CanRoll(Item item)
        {
            if (item.modItem is KiItem)
            {
                return true;
            }
            return false;
        }
        public override PrefixCategory Category { get { return PrefixCategory.AnyWeapon; } }

        public override void Apply(Item item)
        {
            if(item.modItem is KiItem)
            {
                KiItem kiitem = ((KiItem)item.modItem);

                item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 20;
                kiitem.KiDrain *= 1.20f;
            }

            item.damage = (int)(item.damage * 1.12f);
        }
    }
}