using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Head)]
    public class RadiantVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("12% Increased Ki Damage"
                + "\n10% Increased Ki crit" +
                               "\n+500 Max Ki" +
                               "\nIncreased flight speed");
            DisplayName.SetDefault("Radiant Visor");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 52000;
            item.rare = 10;
            item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.12f;
            MyPlayer.ModPlayer(player).KiCrit += 10;
            MyPlayer.ModPlayer(player).KiMax += 500;
            MyPlayer.ModPlayer(player).FlightSpeedAdd += 1f;

        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(null, "RadiantFragment", 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}