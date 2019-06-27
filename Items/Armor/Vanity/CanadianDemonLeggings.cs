using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class CanadianDemonLeggings : PatreonItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% Increased Ki Damage"
                + "\n16% Increased Ki Crit Chance" +
                               "\n+500 Max Ki" +
                               "\nIncreased Ki Regen" +
                               "\n18% Increased movement speed");
            DisplayName.SetDefault("Canadian Demon Leggings");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 32000;
            item.rare = 9;
            item.defense = 14;
            patreonName = "CanadianMRE";
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.13f;
            MyPlayer.ModPlayer(player).kiCrit += 9;
            MyPlayer.ModPlayer(player).kiMax2 += 300;
            MyPlayer.ModPlayer(player).kiRegen += 1;
            player.moveSpeed += 0.12f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DemonLeggings");
            recipe.AddIngredient(ItemID.RedandSilverDye);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}