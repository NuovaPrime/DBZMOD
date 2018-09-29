﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class RadiantTotem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It explodes with radiant energy." +
                "\n12% Increased Ki damage" +
                "\nIncreased flight speed" +
                "\n+500 Max Ki" +
                "\nDrastically increased ki regen");
            DisplayName.SetDefault("Radiant Totem");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 120000;
            item.rare = 9;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.12f;
                player.GetModPlayer<MyPlayer>(mod).KiMax += 500;
                player.GetModPlayer<MyPlayer>(mod).FlightSpeedAdd += 0.5f;
                player.GetModPlayer<MyPlayer>(mod).KiRegen += 2;
                player.GetModPlayer<MyPlayer>(mod).radiantTotem = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RadiantFragment", 10);
			recipe.AddIngredient(null, "DemonicSoul", 5);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}