using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Items.Weapons.Tier_6
{
    public class BlindingBlade : KiItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Flings a ki-coated disk that bounces off walls.");
            DisplayName.SetDefault("Blinding Blade");
        }

        public override void SetDefaults()
        {
            item.damage = 196;
            item.useStyle = 5;
            item.useAnimation = 32;
            item.useTime = 32;
            item.shootSpeed = 172f;
            item.knockBack = 4.2f;
            item.width = 56;
            item.height = 56;
            item.scale = 1f;
            item.rare = 10;
            item.UseSound = SoundID.Item1;
            item.value = 40000;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            KiDrain = 105;
            item.shoot = mod.ProjectileType("BlindingBladeProj");
            item.shootSpeed = 20f;
            WeaponType = "Disk";
            if (!Main.dedServ)
            {
                item.UseSound = SoundID.Item71;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RadiantFragment", 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
