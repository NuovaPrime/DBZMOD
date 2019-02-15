using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace DBZMOD.Items.Materials
{
	public class RadiantFragment : DBItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Fragment");
			Tooltip.SetDefault("'The endurance of the cosmos crackles around this fragment'");
		    ItemID.Sets.ItemNoGravity[item.type] = true;
	        Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(1, 1));
            ItemID.Sets.ItemIconPulse[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.value = 10000;
			item.rare = 9;
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
		}
	}
}