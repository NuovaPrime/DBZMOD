using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HallowedFedora : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("13% Increased Ki Damage"
                + "\n11% Increased Ki Crit Chance" +
                "\nMaximum Ki increased by 300.");
            DisplayName.SetDefault("Hallowed Fedora");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 16;
            item.value = 12000;
            item.rare = 4;
            item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.HallowedPlateMail && legs.type == ItemID.HallowedGreaves;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "9% Increased Ki Damage" +
                "\n+200 Max Ki";
            MyPlayer.ModPlayer(player).KiDamage += 0.09f;
            MyPlayer.ModPlayer(player).KiMax2 += 200;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.13f;
            MyPlayer.ModPlayer(player).KiCrit += 11;
            MyPlayer.ModPlayer(player).KiMax2 += 300;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}