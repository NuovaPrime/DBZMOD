using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class DemonLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% Increased Ki Damage"
                + "\n16% Increased Ki Crit Chance" +
                               "\n+500 Max Ki" +
                               "\nIncreased Ki Regen" +
                               "\n18% Increased movement speed");
            DisplayName.SetDefault("Demon Leggings");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 32000;
            item.rare = 9;
            item.defense = 14;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.20f;
            MyPlayer.ModPlayer(player).KiCrit += 16;
            MyPlayer.ModPlayer(player).KiMax += 500;
            MyPlayer.ModPlayer(player).KiRegen += 2;
            player.moveSpeed += 0.18f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpectreBar, 15);
            recipe.AddIngredient(null, "SatanicCloth", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}