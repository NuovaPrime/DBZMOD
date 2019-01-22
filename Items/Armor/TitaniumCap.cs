using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("14% Increased Ki Damage"
                + "\n9% Increased Ki Crit Chance" +
                "\nMaximum Ki increased by 250.");
            DisplayName.SetDefault("Titanium Cap");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 11000;
            item.rare = 4;
            item.defense = 8;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.TitaniumBreastplate && legs.type == ItemID.TitaniumLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Briefly become invulnerable after striking an enemy";
            player.onHitDodge = true;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).kiDamage += 0.15f;
            MyPlayer.ModPlayer(player).kiCrit += 9;
            MyPlayer.ModPlayer(player).kiMax2 += 250;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 13);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}