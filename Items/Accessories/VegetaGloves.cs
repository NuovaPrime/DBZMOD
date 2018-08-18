using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Items.Accessories;

namespace DBZMOD.Items.Accessories
{
    public class VegetaGloves : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increased ki cast speed" +
                "6% Increased ki damage");
            DisplayName.SetDefault("Vegeta's Gloves");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 8000;
            item.rare = 3;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.06f;
                player.GetModPlayer<MyPlayer>(mod).KiSpeedAddition += 2;
                player.GetModPlayer<MyPlayer>(mod).vegetaGloves = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CalmKiCrystal", 15);
            recipe.AddIngredient(ItemID.Silk , 20);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}