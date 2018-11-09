using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class NegatedPrefix : ModPrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Negated");  
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
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = -20;
            ((KiItem)item.modItem).KiDrain *= 0.80f;
            item.damage = (int)(item.damage * 0.68f);
        }
    }
}