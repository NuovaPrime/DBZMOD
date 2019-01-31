﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class SapphireNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% Increased ki damage and +100 max ki.");
            DisplayName.SetDefault("Sapphire Necklace");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 36;
            item.value = 30000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.09f;
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 100;
                player.GetModPlayer<MyPlayer>(mod).sapphireNecklace = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EmptyNecklace");
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}