using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_5
{
	public class Kamehamehax10 : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("Kamehamehax10Ball");
			item.shootSpeed = 0f;
			item.damage = 156;
			item.knockBack = 2f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
            item.channel = true;
			item.useAnimation = 200;
			item.useTime = 200;
			item.width = 36;
			item.noUseGraphic = true;
			item.height = 36;
			item.autoReuse = false;
			item.value = 75000;
			item.rare = 7;
            KiDrain = 120;
			WeaponType = "Beam";
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("Maximum Charges = 8");
		DisplayName.SetDefault("Kamehameha x10");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "PureKiCrystal", 35);
            recipe.AddIngredient(null, "DemonicSoul", 15);
            recipe.AddIngredient(null, "SuperKamehameha");
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("Kamehamehax10Ball")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
