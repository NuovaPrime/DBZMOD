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
	public class EnergyWave : KiItem
	{
		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shoot = mod.ProjectileType("EnergyWaveBall");
			item.shootSpeed = 0f;
			item.damage = 22;
			item.knockBack = 2f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
			item.useAnimation = 90;
			item.useTime = 90;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 0;
			item.rare = 1;
            KiDrain = 40;
            item.channel = true;
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("-Tier 1-" +
            "\nMaximum Charges = 4");
		DisplayName.SetDefault("Energy Wave");
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 20f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 25);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("EnergyWaveBall")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
