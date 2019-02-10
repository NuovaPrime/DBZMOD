using DBZMOD;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class SaiyanScoutPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("2% Increased Ki Damage\n2% Increased Ki Knockback\n+6% Increased movement speed");
            DisplayName.SetDefault("Saiyan Scout Pants");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 6000;
            item.rare = 2;
            item.defense = 2;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.02f;
            MyPlayer.ModPlayer(player).kiKbAddition += 0.02f;
            player.moveSpeed += 0.06f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 14);
            recipe.AddIngredient(ItemID.CopperBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.Silk, 14);
            recipe2.AddIngredient(ItemID.TinBar, 6);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}