using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_1
{
	public class KiBeam : KiItem
	{
		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shoot = mod.ProjectileType("KiBeamProjectile");
			item.shootSpeed = 70f;
			item.damage = 18;
			item.knockBack = 5f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
			item.useAnimation = 25;
			item.useTime = 18;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = true;
			item.value = 0;
			item.rare = 1;
            KiDrain = 30;
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("-Tier 1-");
		DisplayName.SetDefault("Ki Beam");
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 55f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 30);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
