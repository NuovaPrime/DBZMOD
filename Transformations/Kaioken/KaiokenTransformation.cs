using DBZMOD.Buffs;

namespace DBZMOD.Transformations.Kaioken
{
    public sealed class KaiokenTransformation : TransformationDefinition
    {
        public KaiokenTransformation() : base(BuffKeyNames.kaioken, null, TransformationDefinitionManager.defaultTransformationTextColor, 1f, 1f, 0f, 1f, 0f, 0f, 8)
        {
        }
    }
}
