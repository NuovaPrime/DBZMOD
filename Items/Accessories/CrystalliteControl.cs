using System;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class CrystalliteControl : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The essence of pure ki control lives within the crystal.'" +
                "\nIncreased speed while charging" +
                "\n+500 Max ki");
            DisplayName.SetDefault("Imperium Crystallite");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 34;
            item.value = 120000;
            item.rare = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 500;
                player.GetModPlayer<MyPlayer>(mod).chargeMoveSpeed = Math.Max(player.GetModPlayer<MyPlayer>(mod).chargeMoveSpeed, 0.5f);
                player.GetModPlayer<MyPlayer>(mod).crystalliteControl = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CalmKiCrystal", 25);
			recipe.AddIngredient(null, "PridefulKiCrystal", 25);
            recipe.AddIngredient(null, "AstralEssentia", 10);
            recipe.AddIngredient(null, "SkeletalEssence", 10);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}