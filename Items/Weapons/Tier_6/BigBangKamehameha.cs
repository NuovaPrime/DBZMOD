using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_6
{
	public class BigBangKamehameha : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("BigBangKamehamehaBall");
			item.shootSpeed = 0f;
			item.damage = 150;
			item.knockBack = 2f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
            item.channel = true;
			item.useAnimation = 170;
			item.useTime = 170;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 120000;
			item.rare = 8;
            KiDrain = 150;
			WeaponType = "Beam";
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("Maximum Charges = 10");
		DisplayName.SetDefault("Big Bang Kamehameha");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentStardust, 18);
            recipe.AddIngredient(null, "Kamehamehax10");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("BigBangKamehamehaBall")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
