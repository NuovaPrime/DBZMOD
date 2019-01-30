namespace DBZMOD.Items.DragonBalls
{
    public class FiveStarDB : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("5 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 5 stars inscribed on it." +
                "\nRight-click while holding all 7 to make your wish.");
        }
    }
}