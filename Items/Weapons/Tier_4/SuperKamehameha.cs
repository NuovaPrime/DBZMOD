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
	public class SuperKamehameha: KiItem
	{
		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shoot = mod.ProjectileType("SuperKamehamehaBall");
			item.shootSpeed = 0f;
			item.damage = 64;
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
		    recipe.AddIngredient(null, "Kamehameha", 1);
		    recipe.AddIngredient(null, "AngerKiCrystal", 40);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("SuperKamehamehaBall")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
