﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_5
{
	public class Supernova : KiItem
	{
		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shoot = mod.ProjectileType("Supernova");
            item.shootSpeed = 4f;
            item.damage = 140;
            item.knockBack = 12f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 250;
            item.useTime = 250;
            item.width = 40;
            item.channel = true;
            item.noUseGraphic = true;
            item.height = 40;
            item.autoReuse = false;
            if (!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SpiritBombFire").WithPitchVariance(.3f);

            }
            item.value = 0;
            item.rare = 7;
            KiDrain = 300;
        }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("-Tier 5-");
		DisplayName.SetDefault("Supernova");
		}

        //public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        //{
        //	//Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 20f;
        //	//if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
        //	//{
        //	//	position += muzzleOffset;
        //	//}
        //	return true;
        //}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBomb", 1);
            recipe.AddIngredient(null, "PureKiCrystal", 50);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("SupernovaBall")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
