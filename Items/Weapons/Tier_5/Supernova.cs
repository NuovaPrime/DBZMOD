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
			item.shoot = mod.ProjectileType("SupernovaBall");
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
            item.value = 90000;
            item.rare = 7;
            KiDrain = 350;
            WeaponType = "Massive Blast";
        }
	    public override void SetStaticDefaults()
		{
		DisplayName.SetDefault("Supernova");
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBomb", 1);
            recipe.AddIngredient(null, "PureKiCrystal", 50);
            recipe.AddTile(null, "KaiTable");
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
