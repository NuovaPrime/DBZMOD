using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class DistractedPrefix : BasePrefix
    {
        public override void SetDefaults()
	    {
		  DisplayName.SetDefault("Distracted");  
	    }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 0.86f);
        }

        public override void ApplyKiItemModifier(Item item)
        {
            item.GetGlobalItem<DBZMODItem>().kiChangeBonus = -27;
            ((KiItem)item.modItem).KiDrain *= 0.73f;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult -= 0.18f;
        }
    }
}