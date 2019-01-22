using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class WornGloves : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% Increased ki cast speed" +
                "\n6% Increased ki damage");
            DisplayName.SetDefault("Worn Gloves");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 8000;
            item.rare = 3;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiDamage += 0.06f;
                player.GetModPlayer<MyPlayer>(mod).kiSpeedAddition += 0.10f;
                player.GetModPlayer<MyPlayer>(mod).wornGloves = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CalmKiCrystal", 15);
            recipe.AddIngredient(ItemID.Silk , 20);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}