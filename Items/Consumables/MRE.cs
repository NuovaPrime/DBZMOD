using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
	public class MRE : PatreonItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.consumable = true;
			item.maxStack = 99;
			item.UseSound = SoundID.Item1;
			item.useStyle = 2;
			item.useTurn = true;
			item.useAnimation = 17;
			item.useTime = 17;
			item.value = 3000;
			item.rare = 3;
			item.potion = false;
            item.buffType = mod.BuffType("MREBuff");
            item.buffTime = 10800;
		    PatreonName = "CanadianMRE";
		}
    
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MRE");
            Tooltip.SetDefault("A full course meal, grants a variety of bonuses.");
        }
    }
}
