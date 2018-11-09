using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class BoostedPrefix : ModPrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Boosted");  
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
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = 5;
            ((KiItem)item.modItem).KiDrain *= 1.05f;
            item.damage = (int)(item.damage * 1.06f);
            item.shootSpeed *= 1.10f;
        }
    }
}