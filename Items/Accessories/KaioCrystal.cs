﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class KaioCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A spiritual fragment of one of the legendary Kais.'" +
                "\nAll Kaioken forms drain half as much health" +
                "\n30% less max ki");
            DisplayName.SetDefault("Kaio Crystal");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 90000;
            item.rare = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KaiokenDrainMulti -= 0.5f;
                player.GetModPlayer<MyPlayer>(mod).KiMaxMult *= 0.7f;
                player.GetModPlayer<MyPlayer>(mod).kaioCrystal = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AngerKiCrystal", 15);
            recipe.AddIngredient(ItemID.CrystalShard, 12);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}