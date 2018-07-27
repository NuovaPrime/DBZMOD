using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_3
{
    public class GalickGun : KiItem
    {
        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("GalickGunBall");
            item.shootSpeed = 0f;
            item.damage = 42;
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
            item.value = 0;
            item.rare = 3;
            KiDrain = 80;
            item.channel = true;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("-Tier 3-");
            DisplayName.SetDefault("Galick Gun");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 20f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 45);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
