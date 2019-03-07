using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_2
{
	public class SpiritBall : KiItem
	{

        public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("SpiritBallProjectile");
			item.shootSpeed = 35f;
			item.damage = 32;
			item.knockBack = 8f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 180;
			item.useTime = 180;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.channel = true;
			item.value = 2000;
			item.rare = 2;
            kiDrain = 60;
			weaponType = "Unique";
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("'Yamcha!.'");
		DisplayName.SetDefault("Spirit Ball");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "CalmKiCrystal", 30);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
