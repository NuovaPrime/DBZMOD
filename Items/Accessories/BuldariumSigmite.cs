﻿using DBZMOD;
 using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class BuldariumSigmite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A fragment of the god of defense's soul.'\nCharging grants a protective barrier that grants massively increased defense\nCharging also grants drastically increased life regen\nIncreased ki charge rate");
            DisplayName.SetDefault("Buldarium Sigmite");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 90000;
            item.rare = 8;
            item.defense = 10;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiChargeRate += 2;
                player.GetModPlayer<MyPlayer>(mod).buldariumSigmite = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BaldurEssentia");
            recipe.AddIngredient(null, "KiChip");
            recipe.AddIngredient(ItemID.ShinyStone);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}