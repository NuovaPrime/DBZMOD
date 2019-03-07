using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJ1
{
    public sealed class USSJTransformation : TransformationDefinition
    {
        public USSJTransformation(params TransformationDefinition[] parents) : base(FormBuffHelper.GetUSSJNamePreference(), TransformationDefinitionManager.defaultTransformationTextColor,
            1.9f, 0.9f, 6, 1.6f, 1.5f, 0.75f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ussjAura, new ReadOnlyColor(SSJ1Transformation.LIGHTING_RED, SSJ1Transformation.LIGHTING_GREEN, SSJ1Transformation.LIGHTING_BLUE), 
                new HairAppearance("Hairs/ASSJ/ASSJHair", null, 0, HairAppearance.SSJ1_HAIRSTYLE_KEY), Color.Turquoise),
            typeof(USSJBuff),
            canBeMastered: true, parents: parents)
        {
        }
    }

    public sealed class USSJBuff : TransformationBuff
    {
        public USSJBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition)
        {
        }
    }
}
