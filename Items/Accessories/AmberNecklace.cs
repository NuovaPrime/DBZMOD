using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Items.Accessories;

namespace DBZMOD.Items.Accessories
{
    public class AmberNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% Increased life regen");
            DisplayName.SetDefault("Amber Necklace");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = 30000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.lifeRegen += 1;
                player.GetModPlayer<MyPlayer>(mod).amberNecklace = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EmptyNecklace");
            recipe.AddIngredient(ItemID.Amber, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}