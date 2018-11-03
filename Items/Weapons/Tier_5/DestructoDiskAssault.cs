﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_5
{
	public class DestructoDiskAssault : KiItem
	{
        private Player player;

        public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("DestructoDiskAssaultProj");
			item.shootSpeed = 20f;
			item.damage = 90;
			item.knockBack = 5f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 96;
			item.useTime = 96;
			item.width = 20;
			item.noUseGraphic = true;
			item.height = 20;
			item.autoReuse = false;
			item.value = 0;
			item.rare = 7;
            KiDrain = 140;
			WeaponType = "Disk";
			if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/DiscFire").WithPitchVariance(.3f);
            }
	    }
	    public override void SetStaticDefaults()
		{
		DisplayName.SetDefault("Destructo Disk Assault");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "PureKiCrystal", 40);
			recipe.AddIngredient(null, "DestructoDisk", 1);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
