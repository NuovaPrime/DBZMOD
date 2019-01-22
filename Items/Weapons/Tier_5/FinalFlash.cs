using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_5
{
	public class FinalFlash : BaseBeamItem
    {
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("FinalFlashCharge");
			item.shootSpeed = 0f;
			item.damage = 144;
			item.knockBack = 3f;
			item.useStyle = 5;
            item.useAnimation = 240;
			item.useTime = 240;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 55000;
			item.rare = 6;
            item.channel = true;
            kiDrain = 220;
			weaponType = "Beam";
	    }
	    public override void SetStaticDefaults()
		{
		    Tooltip.SetDefault("Maximum Charges = 9\nRight Click Hold to Charge\nLeft Click to Fire");
            DisplayName.SetDefault("Final Flash");
		}
        
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
		    recipe.AddIngredient(null, "GalickGun", 1);
		    recipe.AddIngredient(null, "PureKiCrystal", 30);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
    }
}
