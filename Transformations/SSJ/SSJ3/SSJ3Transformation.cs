using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Transformations.SSJ.SSJ1;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJ3
{
    public sealed class SSJ3Transformation : TransformationDefinition
    {
        public SSJ3Transformation(params TransformationDefinition[] parents) : base(BuffKeyNames.ssj3, "Super Saiyan 3", TransformationDefinitionManager.defaultTransformationTextColor,
            2.9f, 2.9f, 12, 1.95f, 2.65f, 1.325f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ssj3Aura, new ReadOnlyColor(SSJ1Transformation.LIGHTING_RED, SSJ1Transformation.LIGHTING_GREEN, SSJ1Transformation.LIGHTING_BLUE), new HairAppearance("Hairs/SSJ3/SSJ3Hair", new ReadOnlyColor(0f, 0f, 0f), 0), HairStyleAppearance.SSJ3HairStyle, Color.Turquoise),
            typeof(SSJ3Buff),
            buffIconGetter: () => GFX.ssj3ButtonImage, failureText: "", extraTooltipText: "(Life drains when below 30% Max Ki)", canBeMastered: true,
            unlockRequirements: p => !p.IsLegendary(), parents: parents
            )
        {
        }

        public override float GetHealthDrainRate(MyPlayer player)
        {
            if (player.GetKi() / player.OverallKiMax() <= 0.3f)
                return ModifiedHealthDrainRate + (player.PlayerTransformations[DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition].Mastery >= 1f ? 10 : 20);

            return ModifiedHealthDrainRate;
        }
    }
}
