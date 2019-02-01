﻿using DBZMOD;
 using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CobaltCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% Increased Ki Damage\n6% Increased Ki Crit Chance");
            DisplayName.SetDefault("Cobalt Crown");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 20;
            item.value = 8000;
            item.rare = 4;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.CobaltBreastplate && legs.type == ItemID.CobaltLeggings;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "15% increased ki knockback";
            MyPlayer.ModPlayer(player).kiKbAddition += 15;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.09f;
            MyPlayer.ModPlayer(player).kiCrit += 6;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CobaltBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}