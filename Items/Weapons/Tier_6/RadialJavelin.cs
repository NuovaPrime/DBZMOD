using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Items.Weapons.Tier_6
{
	public class RadialJavelin : KiItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throws a volley of homing ki javelins.");
            DisplayName.SetDefault("Radial Javelin");
		}

		public override void SetDefaults()
		{
			item.damage = 97;
			item.useStyle = 1;
			item.useAnimation = 64;
            item.useTime = 64;
			item.shootSpeed = 17f;
			item.knockBack = 4.2f;
			item.width = 56;
			item.height = 56;
			item.scale = 1f;
            item.rare = 10;
			item.shoot = mod.ProjectileType("RadialJavelinProj");
			item.value = 150000;
			item.noMelee = true; 
			item.noUseGraphic = true; 
			item.autoReuse = true;
            KiDrain = 90;
            if (!Main.dedServ)
            {
                item.UseSound = SoundID.Item45;
            }
			WeaponType = "Unique";
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RadiantFragment", 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 2 + Main.rand.Next(2);
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 0.75f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType ("RadialJavelinProj"), damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
