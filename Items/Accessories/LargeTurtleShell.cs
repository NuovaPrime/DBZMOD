﻿using DBZMOD.Players;
 using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Back)]
    public class LargeTurtleShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A turtle shell with an odd resemblence to the turtle hermit.'\n7% increased ki damage, 8% increased ki knockback.");
            DisplayName.SetDefault("Large Turtle Shell");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.value = 70000;
            item.rare = 4;
            item.accessory = true;
            item.defense = 9;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.07f;
                player.GetModPlayer<MyPlayer>(mod).kiKbAddition += 0.08f;
                player.GetModPlayer<MyPlayer>(mod).turtleShell = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 10);
            recipe.AddIngredient(ItemID.TurtleShell, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}