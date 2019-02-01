﻿using DBZMOD.Players;
 using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Head, EquipType.Face)]
    public class ScouterT3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A Piece of equipment used for scanning powerlevels.'\nGives Increased Ki Damage and Hunter effect.\n--Tier 3--");
            DisplayName.SetDefault("Blue Scouter");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 70000;
            item.rare = 4;
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
            player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.08f;
            player.GetModPlayer<MyPlayer>(mod).scouterT3 = true;
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
            recipe.AddIngredient(null, "PridefulKiCrystal", 20);
            recipe.AddIngredient(null, "ScouterT2");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}