using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_1
{
	public class KiBlast : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("KiBlastProjectile");
			item.shootSpeed = 15f;
			item.damage = 19;
			item.knockBack = 5f;
			item.useStyle = 5;
			item.useAnimation = 22;
			item.useTime = 22;
			item.width = 20;
			item.noUseGraphic = true;
			item.height = 20;
			if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Kiblast1").WithPitchVariance(.3f);
            }
			item.autoReuse = false;
			item.value = 500;
			item.rare = 1;
            kiDrain = 15;
			weaponType = "Blast";
	    }
	    public override void SetStaticDefaults()
		{
		DisplayName.SetDefault("Ki Blast");
		}

		public override bool Shoot(Player ply, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;

		    return true;

		    /*position += Vector2.Normalize(new Vector2(speedX, speedY)) * 40f * item.scale;
			int a = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, item.damage, item.knockBack, item.owner, 0f, 0f);
			Main.projectile[a].scale = 2;
			Main.projectile[a].rotation = Main.projectile[a].velocity.ToRotation();



			int dust = Dust.NewDust(position, 0, 0, mod.DustType("StarMuzzleFlash"));
			Main.dust[dust].scale = 0.8f;
			Main.dust[dust].position = position - Main.dust[dust].scale * new Vector2(4, 4);

			return false;*/
		}
		
		public override void UseStyle(Player ply)
		{
			ply.itemLocation.X = ply.position.X + (float)ply.width * 0.5f;// - (float)Main.itemTexture[item.type].Width * 0.5f;// - (float)(player.direction * 2);
			ply.itemLocation.Y = ply.MountedCenter.Y + ply.gravDir * (float)Main.itemTexture[item.type].Height * 0.5f;

			float relativeX = (float)Main.mouseX + Main.screenPosition.X - ply.Center.X;
			float relativeY = (float)Main.mouseY + Main.screenPosition.Y - ply.Center.Y;

			if (ply.gravDir == -1f)
			    relativeY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - ply.Center.Y;

			if (relativeX - relativeY > 0)
			{
				if (relativeX + relativeY > 0)
				{
					ply.itemRotation = 0;
				}
				else
				{

					ply.itemRotation = ply.direction * -MathHelper.Pi / 2;
					ply.itemLocation.X += ply.direction * 2;
					ply.itemLocation.Y -= 10;
				}
			}
			else
			{
				if (relativeX + relativeY > 0)
				{
					ply.itemRotation = ply.direction * MathHelper.Pi / 2;
					ply.itemLocation.X += ply.direction * 2;
					Main.rand.Next(0, 100);
				}
				else
				{
					ply.itemRotation = 0;
				}
			}
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 25);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
