using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_6
{
    public class BlackBlitz : KiItem
    {
        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("BlackBlitz");
            item.shootSpeed = 5f;
            item.damage = 130;
            item.knockBack = 3f;
            item.useStyle = 5;
            item.useAnimation = 8;
            item.useTime = 4;
            item.width = 50;
            item.height = 50;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.value = 60000;
            item.rare = 7;
            KiDrain = 45;
            WeaponType = "Barrage";
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Power Ball");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RadiantFragment", 45);
            recipe.AddIngredient(null, "EarthenShard", 30); //replace with hardmode versions later
            recipe.AddIngredient(null, "SuperEnergyBarrage");
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
