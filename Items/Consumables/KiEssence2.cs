﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class KiEssence2 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 3;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Student Ki Scroll");
            Tooltip.SetDefault("Increases your ki charge rate.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).KiRegenRate += 1;
            MyPlayer.ModPlayer(player).KiEssence2 = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).KiEssence2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 20);
            recipe.AddIngredient(null, "CalmKiCrystal", 30);
            recipe.AddIngredient(ItemID.Bunny);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
