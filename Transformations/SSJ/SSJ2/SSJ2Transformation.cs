using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Transformations.SSJ.SSJ1;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJ2
{
    public sealed class SSJ2Transformation : TransformationDefinition
    {
        public SSJ2Transformation(params TransformationDefinition[] parents) : base("Super Saiyan 2", TransformationDefinitionManager.defaultTransformationTextColor, 
            2.25f, 2.25f, 8, 1.625f, 2, 1, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ssj2Aura, new ReadOnlyColor(SSJ1Transformation.LIGHTING_RED, SSJ1Transformation.LIGHTING_GREEN, SSJ1Transformation.LIGHTING_BLUE), 
                new HairAppearance("Hairs/SSJ2/SSJ2Hair", null, null, HairAppearance.SSJ2_HAIRSTYLE_KEY), Color.Turquoise),
            typeof(SSJ2Buff),
            buffIconGetter: () => GFX.ssj2ButtonImage, hasMenuIcon: true,
            failureText: "One may awaken their true power through extreme pressure while ascended.", canBeMastered: true,
            unlockRequirements: p => !p.IsLegendary(), parents: parents)
        {
        }
    }

    public class SSJ2Buff : TransformationBuff
    {
        public SSJ2Buff() : base(DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition)
        {
        }
    }
}
