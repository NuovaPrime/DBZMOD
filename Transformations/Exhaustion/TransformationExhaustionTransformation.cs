using DBZMOD.Buffs;

namespace DBZMOD.Transformations.Exhaustion
{
    public sealed class TransformationExhaustionTransformation : TransformationDefinition
    {
        public TransformationExhaustionTransformation() : base(BuffKeyNames.transformationExhaustion, null, TransformationDefinitionManager.defaultTransformationTextColor,
            1f, 1f, 0, 1f, 0f, 0f, 0f,
            new TransformationAppearanceDefinition(null, null, null, null, null, 0, null),
            typeof(TransformationExhaustionBuff),
            exhaustsPlayer: false)
        {
        }
    }
}
