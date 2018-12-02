﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class AncientLegendNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A ancient necklace that seems to seal energy."
               + "\n12% reduced ki usage" +
               "\n9% increased ki damage" +
               "\n-500 max ki");
            DisplayName.SetDefault("Ancient Legend Necklace");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 140000;
            item.rare = 5;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.09f;
                player.GetModPlayer<MyPlayer>(mod).KiDrainMulti *= 0.88f;
                player.GetModPlayer<MyPlayer>(mod).KiMax2 -= 500;
                player.GetModPlayer<MyPlayer>(mod).legendNecklace = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PureKiCrystal", 20);
            recipe.AddIngredient(null, "DemonicSoul", 5);
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}