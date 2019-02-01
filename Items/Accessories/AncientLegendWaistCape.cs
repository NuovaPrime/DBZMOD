﻿using DBZMOD.Players;
 using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class AncientLegendWaistCape : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A ancient garment made of a ki enhancing material.\n14% reduced ki usage\n6% increased ki damage\n-250 max ki");
            DisplayName.SetDefault("Ancient Legend Waistcape");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 150000;
            item.rare = 5;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.06f;
                player.GetModPlayer<MyPlayer>(mod).kiDrainMulti *= 0.86f;
                player.GetModPlayer<MyPlayer>(mod).kiMax2 -= 250;
                player.GetModPlayer<MyPlayer>(mod).legendWaistcape = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PureKiCrystal", 25);
            recipe.AddIngredient(null, "SatanicCloth", 8);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}