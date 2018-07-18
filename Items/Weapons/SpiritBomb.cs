using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons
{
	public class SpiritBomb : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("SpiritBombProjectile");
			item.shootSpeed = 6f;
			item.damage = 360;
			item.knockBack = 12f;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 550;
			item.useTime =  550;
			item.width = 40;
			item.channel = true;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SpiritBombFire").WithPitchVariance(.3f);
				
            }
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = 4;
            KiDrain = 500;
	    }
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 1 + Main.rand.Next(1);  //This defines how many projectiles to shot
            for (int index = 0; index < numberProjectiles; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile X position speed and randomnes
                float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile Y position speed and randomnes
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, (float)Main.rand.Next(5));
            }
            return false;
        }
		
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("-Tier 4-");
		DisplayName.SetDefault("Spirit Bomb");
		}
        public override bool UseItem(Player player)
        {
            base.UseItem(player);
            player.AddBuff(mod.BuffType("TiredDebuff"), 18000);
            return true;
        }
        
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 50);
		    recipe.AddIngredient(null, "AngerKiCrystal", 50);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
