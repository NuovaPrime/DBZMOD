﻿using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff, EquipType.Face)]
    public class BattleKit : ModItem
    {        
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A generic kit of tools for the average soldier.'" +
                "\n6% Increased Ki damage" +
                "\n10% Reduced Ki usage" +
                "\nHunter effect + Increased charge speed" +
                "\n15% Increased ki cast speed");
            DisplayName.SetDefault("Battle Kit");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 20000;
            item.rare = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.06f;
                player.GetModPlayer<MyPlayer>(mod).kiDrainMulti -= 0.10f;
                player.GetModPlayer<MyPlayer>(mod).kiChargeRate += 1;
                player.detectCreature = true;
                player.GetModPlayer<MyPlayer>(mod).kiSpeedAddition += 0.15f;
                player.GetModPlayer<MyPlayer>(mod).battleKit = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ScouterT2");
			recipe.AddIngredient(null, "WornGloves");
            recipe.AddIngredient(null, "ArmCannon");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}