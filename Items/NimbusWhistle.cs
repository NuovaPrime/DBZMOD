using Terraria.ModLoader;
using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DBZMOD.Items
{
	public class NimbusWhistle : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.value = 300;
			item.rare = 3;
			item.UseSound = SoundID.Item79;
			item.noMelee = true;
			item.mountType = mod.MountType("NimbusMount");
		}

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Nimbus Whistle");
      Tooltip.SetDefault("'Calls The Legendary Cloud, Nimbus.'");
    }

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 20);
            recipe.AddIngredient(null, "CalmKiCrystal", 5);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }		
	}
}
