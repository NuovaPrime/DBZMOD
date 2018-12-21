using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class ThreeStarDB : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 3 stars inscribed on it.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.createTile = mod.TileType("ThreeStarDBTile");
        }
    }
}