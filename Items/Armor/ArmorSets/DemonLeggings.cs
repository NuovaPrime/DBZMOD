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
            Tooltip.SetDefault("13% Increased Ki Damage\n9% Increased Ki Crit Chance\n+300 Max Ki\nIncreased Ki Regen\n12% Increased movement speed");
            DisplayName.SetDefault("Demon Leggings");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 18000;
            item.rare = 9;
            item.defense = 12;
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
            recipe.AddIngredient(null, "SatanicCloth", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}