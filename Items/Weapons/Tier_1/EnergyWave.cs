﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_1
{
	public class EnergyWave : BaseBeamItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("EnergyWaveCharge");
			item.shootSpeed = 0f;
			item.damage = 32;
			item.knockBack = 2f;
			item.useStyle = 5;
            item.useAnimation = 2;
            item.useTime = 2;
            item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 500;
			item.rare = 1;
            KiDrain = 40;
            item.channel = true;
			WeaponType = "Beam";
	    }

	    public override void SetStaticDefaults()
		{
		    Tooltip.SetDefault("Maximum Charges = 4");
		    DisplayName.SetDefault("Energy Wave");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 25);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
    }
}
