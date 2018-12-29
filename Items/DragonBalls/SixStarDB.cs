using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class SixStarDB : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("6 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 6 stars inscribed on it.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            WhichDragonBall = 6;
            item.createTile = mod.TileType("SixStarDBTile");
        }
    }
}