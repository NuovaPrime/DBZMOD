using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_2
{
	public class EnergyBlastBarrage : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("EnergyBlastBarrageProjectile");
			item.shootSpeed = 29f;
			item.damage = 34;
			item.knockBack = 5f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 25;
			item.useTime = 12;
			item.width = 20;
			item.noUseGraphic = true;
			item.height = 20;
			item.autoReuse = true;
			if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Kiblast1").WithPitchVariance(.3f);
            }
			item.value = 4000;
			item.rare = 2;
            kiDrain = 50;
			weaponType = "Barrage";
	    }
		public override void SetStaticDefaults()
		{
		DisplayName.SetDefault("Energy Blast Barrage");
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("EnergyBlastBarrageProjectile");
            int numberProjectiles = 1 + Main.rand.Next(2);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "CalmKiCrystal", 30);
            recipe.AddIngredient(null, "KiBlast");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
