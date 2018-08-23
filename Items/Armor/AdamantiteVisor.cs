using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("12% Increased Ki Damage"
                + "\n10% Increased Ki Crit Chance" +
                "\nMaximum Ki increased by 250.");
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
            return body.type == ItemID.AdamantiteBreastplate && legs.type == ItemID.AdamantiteLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "7% Increased Ki Damage";
            MyPlayer.ModPlayer(player).KiDamage += 0.12f;
            MyPlayer.ModPlayer(player).KiCrit += 10;
            MyPlayer.ModPlayer(player).KiMax += 250;
            MyPlayer.ModPlayer(player).adamantiteBonus = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}