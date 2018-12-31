﻿using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Head)]
    public class ScouterT4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A Piece of equipment used for scanning powerlevels.'"
               + "\nGives Increased Ki Damage and Hunter effect."
               + "\n--Tier 4--");
            DisplayName.SetDefault("Red Scouter");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 120000;
            item.rare = 5;
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
            player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.12f;
            player.GetModPlayer<MyPlayer>(mod).scouterT4 = true;
            player.detectCreature = true;            
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AngerKiCrystal", 20);
            recipe.AddIngredient(null, "ScouterT3");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}