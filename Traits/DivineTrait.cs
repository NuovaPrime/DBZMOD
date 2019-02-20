namespace DBZMOD.Traits
{
    public sealed class DivineTrait : Trait
    {
        public DivineTrait() : base("divine", "Divine", 1)
        {
        } 

        public override bool CanSee(MyPlayer player)
        {
            return base.CanSee(player);
        }
    }
}
