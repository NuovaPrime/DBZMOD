﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_2
{
	public class DirtyFireworks : KiItem
	{
        private Player player;

        public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("DirtyFireworksProjectile");
			item.shootSpeed = 35f;
			item.damage = 1;
			item.knockBack = 1f;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 110;
			item.useTime = 180;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.channel = true;
			item.value = 0;
			item.rare = 2;
            KiDrain = 115;
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("FreezyPuff!."
		+ "\n-Tier 3-");
		DisplayName.SetDefault("DirtyFireworks");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "CalmKiCrystal", 30);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
