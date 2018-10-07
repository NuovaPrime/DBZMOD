using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
	public class RadiantKiCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Ki Crystal");
			Tooltip.SetDefault("-Tier 5 Material-");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 9999;
			item.value = 400;
			item.rare = 3;
		}
	}
}