using DBZMOD;
using DBZMOD.Util;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class DragonRadar2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon Radar MK2");
            Tooltip.SetDefault("A medium tech piece of equipment used to locate dragon balls.\nHolding this will point you in the direction of the nearest dragon ball with reasonable accuracy,\nGetting too close to a dragon ball will overload the radar.\nWon't point to Dragon Balls you're holding in your inventory.");
        }

        public override void HoldItem(Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.isHoldingDragonRadarMk2 = true;
            base.HoldItem(player);
        }

        public override bool AltFunctionUse(Player player) => DebugHelper.DragonRadarDebug(player);

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 0;
            item.rare = -12;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DragonRadar1", 1);
            recipe.AddIngredient(null, "AngerKiCrystal", 20);
            recipe.AddIngredient(null, "RefinedMetal", 12);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}