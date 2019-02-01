using DBZMOD.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Head)]
    public class RadiantVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("18% Increased Ki Damage\n24% Increased Ki crit\n+750 Max Ki\nIncreased flight speed");
            DisplayName.SetDefault("Radiant Visor");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 52000;
            item.rare = 10;
            item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.18f;
            MyPlayer.ModPlayer(player).kiCrit += 10;
            MyPlayer.ModPlayer(player).kiMax2 += 750;
            MyPlayer.ModPlayer(player).flightSpeedAdd += 0.3f;

        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
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