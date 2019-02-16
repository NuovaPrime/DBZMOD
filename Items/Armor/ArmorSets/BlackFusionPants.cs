using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class BlackFusionPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% Increased Ki Damage\n16% Increased Ki Crit Chance\n+500 Max Ki\nIncreased Ki Regen\n18% Increased movement speed");
            DisplayName.SetDefault("Black Fusion Pants");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 32000;
            item.rare = 9;
            item.defense = 16;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).kiDamage += 0.20f;
            MyPlayer.ModPlayer(player).kiCrit += 16;
            MyPlayer.ModPlayer(player).kiMax2 += 500;
            MyPlayer.ModPlayer(player).kiRegen += 2;
            player.moveSpeed += 0.18f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DivineThreads", 12);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}