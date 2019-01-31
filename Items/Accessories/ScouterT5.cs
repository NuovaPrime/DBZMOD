﻿using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Head, EquipType.Face)]
    public class ScouterT5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'A Piece of equipment used for scanning powerlevels.'\nGives Increased Ki Damage and Hunter effects.\n--Tier 5--");
            DisplayName.SetDefault("Purple Scouter");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 180000;
            item.rare = 6;
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
            player.detectCreature = true;
            player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.15f;
            player.GetModPlayer<MyPlayer>(mod).scouterT5 = true;            
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PureKiCrystal", 20);
            recipe.AddIngredient(null, "ScouterT4");
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}