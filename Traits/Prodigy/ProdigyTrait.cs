namespace DBZMOD.Traits.Prodigy
{
    public sealed class ProdigyTrait : Trait
    {
        public ProdigyTrait() : base("prodigy", "Prodigy", "You are truly gifted.\nFaster mastery gains.", 20, nameof(ProdigyTraitBuff))
        {
        }
    }
}
