using DBZMOD;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class EliteSaiyanLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("22% Increased Ki Damage\n18% Increased Ki Crit Chance\n+750 Max Ki\nIncreased Ki Regen\n22% Increased movement speed");
            DisplayName.SetDefault("Elite Saiyan Leggings");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 44000;
            item.rare = 9;
            item.defense = 20;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.22f;
            MyPlayer.ModPlayer(player).kiCrit += 18;
            MyPlayer.ModPlayer(player).kiMax2 += 750;
            MyPlayer.ModPlayer(player).kiRegen += 2;
            player.moveSpeed += 0.22f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SaiyanLeggings", 1);
            recipe.AddIngredient(null, "KatchinScale", 12);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}