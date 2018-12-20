﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_4
{
	public class SuperKamehameha: BaseBeamItem
    {
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("SuperKamehamehaCharge");
			item.shootSpeed = 0f;
			item.damage = 118;
			item.knockBack = 7f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
			item.useAnimation = 110;
			item.useTime = 110;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 31500;
			item.rare = 4;
            item.channel = true;
            KiDrain = 150;
			WeaponType = "Beam";
	    }

	    public override void SetStaticDefaults()
		{
		    Tooltip.SetDefault("Maximum Charges = 8");
		    DisplayName.SetDefault("Super Kamehameha");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
		    recipe.AddIngredient(null, "Kamehameha", 1);
		    recipe.AddIngredient(null, "AngerKiCrystal", 30);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
    }
}
