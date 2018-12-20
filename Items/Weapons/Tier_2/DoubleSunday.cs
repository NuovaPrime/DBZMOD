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
	public class DoubleSunday : BaseBeamItem
    {
		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shoot = mod.ProjectileType("DoubleSundayCharge");
			item.shootSpeed = 70f;
			item.damage = 62;
			item.knockBack = 2f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
			item.useAnimation = 100;
			item.useTime = 100;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 3500;
			item.rare = 2;
            KiDrain = 55;
			WeaponType = "Beam";
	    }

	    public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Double Sunday");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "CalmKiCrystal", 25);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
