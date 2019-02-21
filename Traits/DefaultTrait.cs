namespace DBZMOD.Traits
{
    public sealed class DefaultTrait : Trait
    {
        public DefaultTrait() : base("default", "", "You're an average guy.", 0, null)
        {
            Default = true;
        }
    }
}
