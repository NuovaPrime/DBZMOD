using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class RadiantGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("14% Increased Ki Damage"
                + "\n24% Increased Ki Crit Chance" +
                               "\n+750 Max Ki" +
                               "\nIncreased Ki Regen" +
                               "\n26% Increased movement speed");
            DisplayName.SetDefault("Radiant Greaves");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 34000;
            item.rare = 10;
            item.defense = 14;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.14f;
            MyPlayer.ModPlayer(player).KiCrit += 16;
            MyPlayer.ModPlayer(player).KiMax2 += 750;
            MyPlayer.ModPlayer(player).KiRegen += 3;
            player.moveSpeed += 0.26f;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 12);
            recipe.AddIngredient(null, "RadiantFragment", 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}