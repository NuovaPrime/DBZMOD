 using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_4
{
	public class BigBangAttack : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("BigBangAttackProjectile");
			item.damage = 62;
			item.shootSpeed = 25f;
			item.knockBack = 6f;
			item.useStyle = 3;
			item.useAnimation = 150;
			item.useTime = 150;
			item.width = 40;
			item.noUseGraphic = true;
			item.height = 40;
			item.autoReuse = false;
			item.value = 21000;
			item.rare = 4;
            KiDrain = 100;
			WeaponType = "Massive Blast";
            if (!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KiBlastFast").WithPitchVariance(.1f);
            }
        }
	    public override void SetStaticDefaults()
		{
		DisplayName.SetDefault("Big Bang Attack");
		}


        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "PridefulKiCrystal", 15);
		    recipe.AddIngredient(null, "AngerKiCrystal", 25);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
