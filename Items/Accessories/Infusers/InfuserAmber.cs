﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories.Infusers
{
    public class InfuserAmber : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies with ki attacks inflicts ichor.");
            DisplayName.SetDefault("Amber Ki Infuser");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 3200;
            item.rare = 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).infuserAmber = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AngerKiCrystal", 25);
            recipe.AddIngredient(null, "ScrapMetal", 12);
            recipe.AddIngredient(ItemID.Ichor, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            // alternate anti-crimson recipe
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "AngerKiCrystal", 25);
            recipe1.AddIngredient(null, "ScrapMetal", 12);
            recipe1.AddIngredient(ItemID.CursedFlame, 5);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
}