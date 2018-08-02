﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PalladiumBlindfoldCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% Increased Ki Damage"
                + "\n6% Increased Ki Crit Chance");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 16;
            item.value = 8000;
            item.rare = 4;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.PalladiumBreastplate && legs.type == ItemID.PalladiumLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "ArmorSetBonus.Palladium";
            MyPlayer.ModPlayer(player).KiDamage += 0.09f;
            MyPlayer.ModPlayer(player).KiCrit += 6;
            player.palladiumRegen = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PalladiumBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}