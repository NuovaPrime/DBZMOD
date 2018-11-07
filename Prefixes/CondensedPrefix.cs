using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
	public class CondensedPrefix : ModPrefix
	{
		public override float RollChance(Item item)
		{
            return 5f;
		} 
		public override bool CanRoll(Item item)
		{
			if(item.modItem is KiItem)
            {
			    return true;
            }
            return false;
		}
		public override PrefixCategory Category { get { return PrefixCategory.Custom; } }

		public override void Apply(Item item)
		{
            //((KiItem)item.modItem)
            item.damage = (int)(item.damage * 1.10f);
            item.shootSpeed *= 1.15f;
		}
	}
}