using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class SevenStarDB : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("7 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 7 stars inscribed on it.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            WhichDragonBall = 7;
            item.createTile = mod.TileType("SevenStarDBTile");
        }
    }
}