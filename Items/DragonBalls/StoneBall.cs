namespace DBZMOD.Items.DragonBalls
{
    public class StoneBall : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inert Dragon Ball");
            Tooltip.SetDefault("It may hold power if returned to its homeworld.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.consumable = false;
        }
    }
}
