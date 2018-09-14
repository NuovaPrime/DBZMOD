﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class ScouterT6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A Piece of equipment used for scanning powerlevels."
               + "\nGives Increased Ki Damage and Hunter effects."
               + "\n-Tier 6-");
            DisplayName.SetDefault("Wide-Lens Scouter");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 240000;
            item.rare = 7;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {

                player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.20f;
                player.GetModPlayer<MyPlayer>(mod).scouterT6 = true;
            }
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RadiantKiCrystal", 20);
            recipe.AddIngredient(ItemID.FragmentNebula, 10);
            recipe.AddIngredient(null, "ScouterT5");
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}