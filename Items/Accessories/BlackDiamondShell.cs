﻿using DBZMOD;
 using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Back)]
    public class BlackDiamondShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A jeweled turtle shell that gets the attention of many creatures, for some reason it's unbelievably tough.\n12% increased ki damage, 14% increased ki knockback.\n+200 Max Ki.\nGetting hit restores a small amount of Ki.");
            DisplayName.SetDefault("Black Diamond Shell");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.value = 120000;
            item.rare = 7;
            item.accessory = true;
            item.defense = 14;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.12f;
                player.GetModPlayer<MyPlayer>(mod).kiKbAddition += 0.14f;
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 200;
                player.GetModPlayer<MyPlayer>(mod).blackDiamondShell = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "LargeTurtleShell", 1);
            recipe.AddIngredient(ItemID.Diamond, 15);
            recipe.AddIngredient(ItemID.Obsidian, 5);
            recipe.AddIngredient(null, "EarthenShard", 5);
            recipe.AddIngredient(ItemID.BeetleHusk, 15);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}