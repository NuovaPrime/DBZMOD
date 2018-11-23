﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class CrystalliteAlleviate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The essence of pure energy lives within the crystal." +
                "\nDrastically Increased speed while charging" +
                "\n+2500 Max ki");
            DisplayName.SetDefault("Aspera Crystallite");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 34;
            item.value = 720000;
            item.rare = 9;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiMax += 2500;
                player.GetModPlayer<MyPlayer>(mod).chargeMoveSpeed += 1.5f;
                player.GetModPlayer<MyPlayer>(mod).crystalliteAlleviate = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "RadiantFragment", 5);
            recipe.AddIngredient(ItemID.FragmentVortex, 5);
            recipe.AddIngredient(ItemID.FragmentSolar, 5);
            recipe.AddIngredient(ItemID.FragmentStardust, 5);
            recipe.AddIngredient(ItemID.FragmentNebula, 5);
            recipe.AddIngredient(null, "CrystalliteFlow");
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}