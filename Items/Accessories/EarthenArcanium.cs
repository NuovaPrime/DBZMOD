﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class EarthenArcanium : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A core of the pure energy of the earth." +
                "\n10% Increased ki damage" +
                "\nIncreased ki regen" +
                "\nReduced flight ki usage" +
                "\n+1 Max Charges" +
                "\nIncreased flight speed" +
                "\nThe longer you charge the more ki you charge, limits at +500%.");
            DisplayName.SetDefault("Earthen Arcanium");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 360000;
            item.rare = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.10f;
                player.GetModPlayer<MyPlayer>(mod).FlightSpeedAdd += 0.1f;
                player.GetModPlayer<MyPlayer>(mod).KiRegen += 1;
                player.GetModPlayer<MyPlayer>(mod).FlightUsageAdd += 1;
                player.GetModPlayer<MyPlayer>(mod).ChargeLimitAdd += 1;
                player.GetModPlayer<MyPlayer>(mod).earthenArcanium = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EarthenScarab");
            recipe.AddIngredient(null, "EarthenSigil");
            recipe.AddIngredient(null, "PridefulKiCrystal", 25);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}