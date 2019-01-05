using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class FourStarDB : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("4 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 4 stars inscribed on it.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.createTile = mod.TileType("FourStarDBTile");
        }

        public override int GetWhichDragonBall()
        {
            return 4;
        }
    }
}