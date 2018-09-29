using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Normal
{
	public class FarmerShotgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A weapon forged by the gods.");
            DisplayName.SetDefault("Farmer's Shotgun");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.ranged = true;
			item.width = 62;
			item.height = 20;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 8f;
			item.value = 30000;
			item.rare = 3;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 18f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ScrapMetal", 12);
            recipe.AddIngredient(ItemID.BorealWood, 15);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
		    int numberProjectiles = 4 + Main.rand.Next(3);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
