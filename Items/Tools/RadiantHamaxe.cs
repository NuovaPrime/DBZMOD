using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Items.Tools
{
    public class RadiantHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Hamaxe");
        }

        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useStyle = 1;
            item.width = 48;
            item.height = 46;
            item.rare = 10;
            item.useTime = 9;
            item.useAnimation = 27;
            item.damage = 60;
            item.melee = true;
            item.value = 50000;
            item.axe = 30;
            item.hammer = 100;
            item.knockBack = 7;
            item.tileBoost = 4;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 12);
            recipe.AddIngredient(null, "RadiantFragment", 14);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
