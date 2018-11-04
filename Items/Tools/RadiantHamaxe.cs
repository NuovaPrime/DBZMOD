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
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Tools/RadiantHamaxe");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
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
