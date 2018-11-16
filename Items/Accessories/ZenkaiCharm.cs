﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class ZenkaiCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A charm that harnesses the true power of a saiyan." +
                "\n8% increased ki damage" +
                "\nTaking fatal damage will instead return you to 50 hp" +
                "\nand grant x2 damage for a short time." +
                "\n2 Minute cooldown");
            DisplayName.SetDefault("Zenkai Charm");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 135000;
            item.rare = 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.08f;
                player.GetModPlayer<MyPlayer>(mod).zenkaiCharm = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AngerKiCrystal", 20);
            recipe.AddIngredient(null, "SoulofEntity", 16);
            recipe.AddIngredient(ItemID.GoldBar, 12);
            recipe.AddIngredient(ItemID.Obsidian, 8);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}