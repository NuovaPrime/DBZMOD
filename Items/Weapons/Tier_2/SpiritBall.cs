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
	public class SpiritBall : KiItem
	{
        private Player player;

        public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("SpiritBallProjectile");
			item.shootSpeed = 35f;
			item.damage = 32;
			item.knockBack = 8f;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 180;
			item.useTime = 180;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.channel = true;
			item.value = 0;
			item.rare = 2;
            KiDrain = 60;
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("Yamcha!." 
		+ "\n-Tier 2-");
		DisplayName.SetDefault("Spirit Ball");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "CalmKiCrystal", 30);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
