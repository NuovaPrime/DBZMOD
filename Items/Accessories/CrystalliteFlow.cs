using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class CrystalliteFlow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The essence of a calm flowing spirit lives within the crystal.'" +
                "\nGreatly Increased speed while charging" +
                "\n+1000 Max ki");
            DisplayName.SetDefault("Influunt Crystallite");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 34;
            item.value = 320000;
            item.rare = 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 1000;
                player.GetModPlayer<MyPlayer>(mod).chargeMoveSpeed = Math.Max(player.GetModPlayer<MyPlayer>(mod).chargeMoveSpeed, 1f);
                player.GetModPlayer<MyPlayer>(mod).crystalliteFlow = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AngerKiCrystal", 25);
			recipe.AddIngredient(null, "PureKiCrystal", 25);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddIngredient(null, "SoulofEntity", 10);
            recipe.AddIngredient(null, "CrystalliteControl");
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}