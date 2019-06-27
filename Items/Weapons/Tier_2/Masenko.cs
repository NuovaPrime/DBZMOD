using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_2
{
	public class Masenko : BaseBeamItem
    {
		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shoot = mod.ProjectileType("MasenkoCharge");
			item.shootSpeed = 0f;
			item.damage = 35;
			item.knockBack = 2f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
			item.useAnimation = 90;
			item.useTime = 90;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 3500;
			item.rare = 2;
            item.channel = true;
            kiDrain = 60;
			weaponType = "Beam";
	    }

	    public override void SetStaticDefaults()
		{
		    Tooltip.SetDefault("Maximum Charges = 5\nRight Click Hold to Charge\nLeft Click to Fire");
            DisplayName.SetDefault("Masenko");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CalmKiCrystal", 30);
            recipe.AddIngredient(null, "AstralEssentia", 15);
            recipe.AddIngredient(null, "EnergyWave");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
    }
}
