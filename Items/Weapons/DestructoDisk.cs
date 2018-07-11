using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons
{
	public class DestructoDisk : KiItem
	{
        private Player player;

        public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("DestructoDiskProjectile");
			item.shootSpeed = 20f;
			item.damage = 50;
			item.knockBack = 5f;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 28;
			item.useTime = 28;
			item.width = 20;
			item.noUseGraphic = true;
			item.height = 20;
			item.autoReuse = false;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = 1;
            KiDrain = 70;
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("Its a frizbee I swear." 
		+ "\n-Tier 2.5-");
		DisplayName.SetDefault("Destructo Disk");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "HonorKiCrystal", 40);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
