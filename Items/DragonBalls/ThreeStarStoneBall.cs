namespace DBZMOD.Items.DragonBalls
{
    public class ThreeStarStoneBall : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inert Three Star Dragon Ball");
            Tooltip.SetDefault("It may hold power if returned to its homeworld.");            
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.consumable = false;
        }

        public override int GetWhichDragonBall()
        {
            return 3;
        }
    }
}
