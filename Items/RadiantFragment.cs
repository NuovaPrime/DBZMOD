using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
	public class RadiantFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Fragment");
			ToolTip.SetDefault("'The endurance of the cosmos crackles around this fragment'")
		   	ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 9999;
			item.value = 6000;
			item.rare = 9;
		}
		public override Color? GetAlpha(Color lightColor)
   		{
            		return Color.White;
		}
	}
}
