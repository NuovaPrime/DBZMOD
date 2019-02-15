using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_5
{
	public class Kamehamehax10 : BaseBeamItem
    {
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("Kamehameha10Charge");
			item.shootSpeed = 0f;
			item.damage = 156;
            item.knockBack = 10f;
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
            kiDrain = 120;
			weaponType = "Beam";
	    }
	    public override void SetStaticDefaults()
		{
		    Tooltip.SetDefault("Maximum Charges = 8\nHold Right Click to Charge\nHold Left Click to Fire");
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
    }
}
