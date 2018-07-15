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
	public class KiBlast : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("KiBlastProjectile");
			item.shootSpeed = 25f;
			item.damage = 15;
			item.knockBack = 5f;
			item.useStyle = 1;
			item.useAnimation = 25;
			item.useTime = 16;
			item.width = 20;
			item.noUseGraphic = true;
			item.height = 20;
			if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Kiblast1").WithPitchVariance(.3f);
            }
			item.autoReuse = false;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = 1;
            KiDrain = 20;
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("-Tier 1-");
		DisplayName.SetDefault("Ki Blast");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 25);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
