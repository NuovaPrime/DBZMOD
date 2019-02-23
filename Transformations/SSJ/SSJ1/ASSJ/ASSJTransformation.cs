using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJ1.ASSJ
{
    public sealed class ASSJTransformation : TransformationDefinition
    {
        public ASSJTransformation(params TransformationDefinition[] parents) : base(BuffKeyNames.assj, FormBuffHelper.GetASSJNamePreference(), TransformationDefinitionManager.defaultTransformationTextColor,
            1.75f, 1.75f, 5, 1.4f, 1.15f, 0.575f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.assjAura, new ReadOnlyColor(SSJ1Transformation.LIGHTING_RED, SSJ1Transformation.LIGHTING_GREEN, SSJ1Transformation.LIGHTING_BLUE), "Hairs/SSJ/ASSJHair", null, 0, Color.Turquoise),
            typeof(ASSJBuff),
            canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.ssj1, parents: parents)
        {
        }
    }
}
