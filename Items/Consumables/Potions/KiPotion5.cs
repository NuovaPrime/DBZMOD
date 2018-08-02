﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.Potions
{
    public class KiPotion5 : KiPotion
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 38;
            item.consumable = true;
            item.maxStack = 30;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 12;
            item.useTime = 12;
            item.value = 300;
            item.rare = 4;
            item.potion = false;
            IsKiPotion = true;
            KiHeal = 2500;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gigantic Ki Potion");
            Tooltip.SetDefault("Restores 2500 Ki.");
        }
		
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RadiantKiCrystal", 2);
            recipe.AddIngredient(null, "KiPotion4", 1);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}