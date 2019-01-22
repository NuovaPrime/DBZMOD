namespace DBZMOD.Items.DragonBalls
{
    public class TwoStarDB : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("2 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 2 stars inscribed on it.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.createTile = mod.TileType("TwoStarDBTile");
        }

        public override int GetWhichDragonBall()
        {
            return 2;
        }
    }
}