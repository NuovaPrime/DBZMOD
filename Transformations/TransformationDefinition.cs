namespace DBZMOD.Transformations
{
    public abstract class TransformationDefinition : IHasUnlocalizedName
    {
        protected TransformationDefinition(string unlocalizedName, bool canBeMastered = true, float masteryScaling = 1f)
        {
            UnlocalizedName = unlocalizedName;
            CanBeMastered = canBeMastered;
            MasteryScaling = masteryScaling;
        }

        public string UnlocalizedName { get; }

        public bool CanBeMastered { get; }

        public float MasteryScaling { get; }
    }
}
