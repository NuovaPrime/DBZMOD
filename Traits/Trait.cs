namespace DBZMOD.Traits
{
    public abstract class Trait : IHasUnlocalizedName
    {
        protected Trait(string unlocalizedName, string traitText, float percentage)
        {
            UnlocalizedName = unlocalizedName;
            TraitText = traitText;
            Percentage = percentage;
        }

        public virtual bool CanSee(MyPlayer player)
        {
            return true;
        }

        public string UnlocalizedName { get; }
        public string TraitText { get; }

        public float Percentage { get; }
    }
}
