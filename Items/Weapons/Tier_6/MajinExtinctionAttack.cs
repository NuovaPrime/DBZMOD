using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Items.Weapons.Tier_6 
{
	public class MajinExtinctionAttack : KiItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Majin Extinction Attack");
			Tooltip.SetDefault("'Human Extinction...'");
		}

	    public override void SetDefaults()
	    {
	        item.damage = 70;
	        item.width = 28;
	        item.height = 30;
	        item.useTime = 4;
			item.useAnimation = 12;
	        item.useStyle = 5;
	        item.noMelee = true;
			item.noUseGraphic = true;
	        item.knockBack = 5;
	        item.rare = 8;
			item.reuseDelay = 42;
	        item.shoot = mod.ProjectileType("MajinBall");
	        item.shootSpeed = 16f;
			if (!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Scatterbeam").WithPitchVariance(.3f);

            }
            item.value = 0;
            item.rare = 8;
            KiDrain = 500;
	    }
	    
	    public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
	    	float spread = 30f * 0.0174f;
	    	double startAngle = Math.Atan2(speedX, speedY)- spread/2;
	    	double deltaAngle = spread/8f;
	    	double offsetAngle;
	    	int i;
	    	for (i = 0; i < 4; i++ )
	    	{
	   			offsetAngle = (startAngle + deltaAngle * ( i + i * i ) / 2f ) + 32f * i;
	        	Projectile.NewProjectile(position.X, position.Y, (float)( Math.Sin(offsetAngle) * 4f ), (float)( Math.Cos(offsetAngle) * 4f ), type, damage, knockBack, Main.myPlayer);
	        	Projectile.NewProjectile(position.X, position.Y, (float)( -Math.Sin(offsetAngle) * 3f ), (float)( -Math.Cos(offsetAngle) * 3f ), type, damage, knockBack, Main.myPlayer);
				Projectile.NewProjectile(position.X, position.Y, (float)( Math.Sin(offsetAngle) * 2f ), (float)( Math.Cos(offsetAngle) * 2f ), type, damage, knockBack, Main.myPlayer);
	        	Projectile.NewProjectile(position.X, position.Y, (float)( -Math.Sin(offsetAngle) * 6f ), (float)( -Math.Cos(offsetAngle) * 6f ), type, damage, knockBack, Main.myPlayer);
				Projectile.NewProjectile(position.X, position.Y, (float)( Math.Sin(offsetAngle) * 7f ), (float)( Math.Cos(offsetAngle) * 7f ), type, damage, knockBack, Main.myPlayer);
	        	Projectile.NewProjectile(position.X, position.Y, (float)( -Math.Sin(offsetAngle) * 1f ), (float)( -Math.Cos(offsetAngle) * 1f ), type, damage, knockBack, Main.myPlayer);
	    	}
	    	return false;
		}
	
	    /*public override void AddRecipes()
	    {
	        ModRecipe recipe = new ModRecipe(mod);
	        recipe.SetResult(this);
	        recipe.AddRecipe();
	    }*/
	}
}