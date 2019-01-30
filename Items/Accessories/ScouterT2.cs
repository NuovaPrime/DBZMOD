﻿using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Head, EquipType.Face)]
    public class ScouterT2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A Piece of equipment used for scanning powerlevels.'\nIncreased Ki Damage and Hunter effect.\n-Tier 2-");
            DisplayName.SetDefault("Green Scouter");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 40000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateEquip(Player player)
        {
            GivePlayerBonuses(player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            GivePlayerBonuses(player);
        }

        public void GivePlayerBonuses(Player player)
        {
            player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.05f;
            player.GetModPlayer<MyPlayer>(mod).scouterT2 = true;
            player.detectCreature = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CalmKiCrystal", 20);
            recipe.AddIngredient(null, "ScrapMetal", 15);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}