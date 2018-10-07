using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DBZMOD.Items
{
	public class RadiantFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Fragment");
			ToolTip.SetDefault("'The endurance of the cosmos crackles around this fragment'")
<<<<<<< HEAD
		    ItemID.Sets.ItemNoGravity[item.type] = true;
=======
		   	ItemID.Sets.ItemNoGravity[item.type] = true;
>>>>>>> ec9999d76a0341d42a914adea30ebb4a075a8be4
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
