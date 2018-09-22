﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class BukuujutsuGuide : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 4;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bukuujutsu Guide");
            Tooltip.SetDefault("It has an ancient technique inscribed in it, reading it may unlock something akin to flight.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).flightUnlocked = true;
            Main.NewText("You have unlocked flight.");
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).flightUnlocked)
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
            recipe.AddIngredient(null, "StableKiCrystal", 100);
            recipe.AddIngredient(ItemID.ManaCrystal, 3);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
