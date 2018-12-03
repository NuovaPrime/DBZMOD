﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_3
{
	public class Kamehameha : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("KamehamehaBall");
			item.shootSpeed = 0f;
			item.damage = 88;
			item.knockBack = 2f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
            item.channel = true;
			item.useAnimation = 100;
			item.useTime = 100;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 10500;
			item.rare = 3;
            KiDrain = 80;
			WeaponType = "Beam";
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("Maximum Charges = 6");
		DisplayName.SetDefault("Kamehameha");
		}

        /*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 20f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}*/

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "PridefulKiCrystal", 30);
			recipe.AddIngredient(null, "Masenko");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("KamehamehaBall")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
