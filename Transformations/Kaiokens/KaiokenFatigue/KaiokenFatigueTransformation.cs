using DBZMOD.Buffs;

namespace DBZMOD.Transformations.Kaiokens.KaoikenFatigue
{
    public sealed class KaiokenFatigueTransformation : TransformationDefinition
    {
        public KaiokenFatigueTransformation() : base(null, TransformationDefinitionManager.defaultTransformationTextColor, 
            1f, 1f, 0, 1f, 1f, 0f, 0f,
            new TransformationAppearanceDefinition(null, null, new HairAppearance(null, null, null, null), null),
            typeof(TiredDebuff), hasMenuIcon: false)
        {
        }
    }
}
