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
                "\nDrastically increased life regen.");
            DisplayName.SetDefault("Time Ring");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = 60000;
            item.rare = 6;
            item.accessory = true;
            PatreonName = "Lethaius";
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.lifeRegen += 6;
                player.GetModPlayer<MyPlayer>(mod).KiRegen += 5;
                player.GetModPlayer<MyPlayer>(mod).timeRing = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe.AddIngredient(ItemID.AdamantiteBar, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}