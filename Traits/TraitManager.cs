using DBZMOD.Buffs;
using DBZMOD.Managers;

namespace DBZMOD.Traits
{
    public class TraitManager : Manager<Trait>
    {
        internal override void DefaultInitialize()
        {
            Divine = Add(new DivineTrait());
            Legendary = Add(new LegendaryTrait());
            Primal = Add(new PrimalTrait());
            Prodigy = Add(new ProdigyTrait());
        }

        public Trait Divine { get; private set; }

        public Trait Legendary { get; private set; }

        public Trait Primal { get; private set; }

        public Trait Prodigy { get; private set; }
    }
}
