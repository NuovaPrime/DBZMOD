using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class HermitPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% Increased Ki Damage"
                + "\n4% Increased Ki Knockback" +
                               "\n+10% Increased movement speed");
            DisplayName.SetDefault("Turtle Hermit Pants");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 11000;
            item.rare = 3;
            item.defense = 6;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).kiDamage += 0.04f;
            MyPlayer.ModPlayer(player).kiKbAddition += 0.04f;
            player.moveSpeed += 0.10f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 16);
            recipe.AddIngredient(null, "EarthenShard", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}