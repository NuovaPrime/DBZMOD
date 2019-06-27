using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_3
{
	public class Kamehameha : BaseBeamItem
    {
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("KamehamehaCharge");
			item.shootSpeed = 0f;
			item.damage = 88;
			item.knockBack = 2f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item12;
            item.channel = true;
			item.useAnimation = 100;
			item.useTime = 100;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 10500;
			item.rare = 3;
            kiDrain = 80;
			weaponType = "Beam";
	    }
	    public override void SetStaticDefaults()
		{
		    Tooltip.SetDefault("Maximum Charges = 6\nHold Right Click to Charge\nHold Left Click to Fire");
            DisplayName.SetDefault("Kamehameha");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "PridefulKiCrystal", 30);
			recipe.AddIngredient(null, "Masenko");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
    }
}
