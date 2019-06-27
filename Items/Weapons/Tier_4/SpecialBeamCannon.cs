using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_4
{
	public class SpecialBeamCannon : BaseBeamItem
    {
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("MakankosappoCharge");
            item.shootSpeed = 0f;
			item.damage = 65;
			item.knockBack = 3f;
			item.useStyle = 5;
            item.useAnimation = 240;
			item.useTime = 240;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 23000;
			item.rare = 6;
            item.channel = true;
            kiDrain = 220;
			weaponType = "Beam";
	    }
	    public override void SetStaticDefaults()
		{
		    Tooltip.SetDefault("Maximum Charges = 6\nRight Click Hold to Charge\nLeft Click to Fire");
            DisplayName.SetDefault("Makankosappo");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
		    recipe.AddIngredient(null, "Masenko", 1);
		    recipe.AddIngredient(null, "AngerKiCrystal", 30);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
    }
}
