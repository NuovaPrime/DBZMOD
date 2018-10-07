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
	public class SpecialBeamCannon : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("SpecialBeamCannonBall");
			item.shootSpeed = 0f;
			item.damage = 60;
			item.knockBack = 3f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
			item.useAnimation = 240;
			item.useTime = 240;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 0;
			item.rare = 6;
            item.channel = true;
            KiDrain = 220;
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("-Tier 4-" +
		                   "\nMaximum Charges = 6");
		DisplayName.SetDefault("Special Beam Cannon");
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
		    recipe.AddIngredient(null, "Masenko", 1);
		    recipe.AddIngredient(null, "PureKiCrystal", 50);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("SpecialBeamCannonBall")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
