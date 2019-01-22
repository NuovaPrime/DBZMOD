﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class TimeRing : PatreonItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The sacred ring of the kais'" +
                "\nDrastically increased health regen" +
                "\nDrastically increased ki regen.");
            DisplayName.SetDefault("Time Ring");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = 60000;
            item.rare = 6;
            item.accessory = true;
            patreonName = "Lethaius";
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.lifeRegen += 4;
                player.GetModPlayer<MyPlayer>(mod).kiRegen += 3;
                player.GetModPlayer<MyPlayer>(mod).timeRing = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.AdamantiteBar, 8);
            recipe2.AddIngredient(ItemID.SoulofLight, 5);
            recipe2.AddIngredient(ItemID.SoulofNight, 5);
            recipe2.AddTile(null, "KaiTable");
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}