using System;
using DBZMOD.Traits;

namespace DBZMOD
{
    public partial class MyPlayer
    {
        public bool? syncTraitChecked;
        public Trait syncPlayerTrait;

        [Obsolete]
        public float GetProdigyMasteryMultiplier() => IsProdigy() ? 2f : 1f;

        #region Shortcut Traits

        public bool IsLegendary() => PlayerTrait == DBZMOD.Instance.TraitManager.Legendary;

        public bool IsProdigy() => PlayerTrait == DBZMOD.Instance.TraitManager.Prodigy;

        public bool IsPrimal() => PlayerTrait == DBZMOD.Instance.TraitManager.Primal;

        public bool IsDivine() => PlayerTrait == DBZMOD.Instance.TraitManager.Divine;

        #endregion

        public Trait PlayerTrait { get; set; }
    }
}
