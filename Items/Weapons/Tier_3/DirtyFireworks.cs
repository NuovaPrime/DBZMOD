using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_3
{
	public class DirtyFireworks : KiItem
	{
        private Player _player;

        public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("DirtyFireworksProjectile");
			item.shootSpeed = 35f;
			item.damage = 1;
			item.knockBack = 1f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 600;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.channel = true;
			item.value = 5500;
			item.rare = 3;
            kiDrain = 110;
			weaponType = "Unique";
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("'Freezy Puff!.'");
		DisplayName.SetDefault("Dirty Fireworks");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "PridefulKiCrystal", 30);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
