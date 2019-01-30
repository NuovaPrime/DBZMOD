using System;
using System.Collections.Generic;
using DBZMOD.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Test
{
	public class CircleTester : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circle Spawner");
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
            item.shootSpeed = 0f;
			item.useAnimation = 15;
			item.useStyle = 4;
			item.shoot = mod.ProjectileType("Circle");
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

        public override void AddRecipes()
        {
            if (DebugHelper.IsDebugModeOn())
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
