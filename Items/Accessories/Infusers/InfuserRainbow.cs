﻿using DBZMOD.Players;
 using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories.Infusers
{
    public class InfuserRainbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies with ki attacks inflicts a multitude of debuffs.");
            DisplayName.SetDefault("Dragon Crystal Infuser");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 150000;
            item.rare = 8;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).infuserRainbow = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PureKiCrystal", 25);
            recipe.AddIngredient(null, "ScrapMetal", 12);
            recipe.AddIngredient(null, "InfuserAmber");
            recipe.AddIngredient(null, "InfuserAmethyst");
            recipe.AddIngredient(null, "InfuserTopaz");
            recipe.AddIngredient(null, "InfuserEmerald");
            recipe.AddIngredient(null, "InfuserDiamond");
            recipe.AddIngredient(null, "InfuserSapphire");
            recipe.AddIngredient(null, "InfuserRuby");
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}