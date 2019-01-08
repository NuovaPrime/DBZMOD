﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_3
{
    public class GalickGun : BaseBeamItem
    {
        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("GalickGunCharge");
            item.shootSpeed = 0f;
            item.damage = 94;
            item.knockBack = 2f;
            item.useStyle = 5;
            item.UseSound = SoundID.Item12;
            item.channel = true;
            item.useAnimation = 110;
            item.useTime = 110;
            item.width = 40;
            item.noUseGraphic = true;
            item.height = 40;
            item.autoReuse = false;
            item.value = 10000;
            item.rare = 3;
            KiDrain = 80;
            item.channel = true;
            WeaponType = "Beam";
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Maximum Charges = 7\nRight Click Hold to Charge\nLeft Click to Fire");
            DisplayName.SetDefault("Galick Gun");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 30);
            recipe.AddIngredient(null, "SkeletalEssence", 15);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
