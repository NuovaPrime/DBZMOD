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
            item.createTile = mod.TileType("SixStarDBTile");
        }

        public override int GetWhichDragonBall()
        {
            return 6;
        }
    }
}