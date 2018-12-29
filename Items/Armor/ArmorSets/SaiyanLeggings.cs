using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class SaiyanLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("7% Increased Ki Damage"
                + "\n5% Increased Ki Crit Chance" +
                               "\n16% Increased movement speed");
            DisplayName.SetDefault("Saiyan Battle Leggings");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 16000;
            item.rare = 4;
            item.defense = 9;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.07f;
            MyPlayer.ModPlayer(player).KiCrit += 5;
            player.moveSpeed += 0.16f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(null, "SkeletalEssence", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}