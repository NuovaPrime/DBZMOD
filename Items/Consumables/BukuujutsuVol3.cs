﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class BukuujutsuVole : ModItem
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
            item.rare = 6;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bukuujutsu Guide Vol 3 - Lux Ruinam");
            Tooltip.SetDefault("It has an ancient technique inscribed in it, holding it makes your feet feel softer.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).flightDampeningUnlocked = true;
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You now take no fall damage for 10 seconds after flying.");
                return true;
            }
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).flightDampeningUnlocked)
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
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
