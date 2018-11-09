using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class DistractedPrefix : ModPrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Distracted");  
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
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = -27;
            ((KiItem)item.modItem).KiDrain *= 0.73f;
            item.damage = (int)(item.damage * 0.86f);
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult -= 0.18f;
        }
    }
}