using System;
using System.Collections.Generic;
using DBZMOD.Managers;
using DBZMOD.Traits.Divine;
using DBZMOD.Traits.Legendary;
using DBZMOD.Traits.Primal;
using DBZMOD.Traits.Prodigy;
using Terraria;
using Terraria.Utilities;

namespace DBZMOD.Traits
{
    public class TraitManager : Manager<Trait>
    {
        private bool _hasDefault;

        internal override void DefaultInitialize()
        {
            Default = Add(new DefaultTrait());
            Divine = Add(new DivineTrait());
            Legendary = Add(new LegendaryTrait());
            Primal = Add(new PrimalTrait());
            Prodigy = Add(new ProdigyTrait());
        }

        public override Trait Add(Trait item)
        {
            Trait trait = base.Add(item);

            if (trait == item && _hasDefault && trait.Default)
                throw new ArgumentException("The " + nameof(TraitManager) + " already has a default value.");

            _hasDefault = true;
            return trait;
        }

        public override bool Remove(Trait item)
        {
            if (byIndex.Contains(item) && item.Default && _hasDefault)
                _hasDefault = false;

            return base.Remove(item);
        }

        public Trait GetRandomTrait()
        {
            float totalWeight = 0;

            WeightedRandom<Trait> traitChooser = new WeightedRandom<Trait>();

            for (int i = 0; i < byIndex.Count; i++)
            {
                totalWeight += byIndex[i].Percentage;
                traitChooser.Add(byIndex[i], byIndex[i].Percentage);
            }

            int roll = Main.rand.Next(0, 100);

            if (roll > totalWeight)
                return traitChooser;

            return Default;
        }

        public Trait GetRandomTrait(Trait oldTrait)
        {
            WeightedRandom<Trait> traitChooser = new WeightedRandom<Trait>();

            for (int i = 0; i < byIndex.Count; i++)
            {
                if (byIndex[i] == oldTrait || byIndex[i] == Default) continue;
                traitChooser.Add(byIndex[i]);
            }

            return traitChooser;
        }

        public Trait Default { get; private set; }

        public Trait Divine { get; private set; }

        public Trait Legendary { get; private set; }

        public Trait Primal { get; private set; }

        public Trait Prodigy { get; private set; }
    }
}
