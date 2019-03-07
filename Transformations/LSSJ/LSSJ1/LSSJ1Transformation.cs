using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.LSSJ.LSSJ1
{
    public sealed class LSSJ1Transformation : TransformationDefinition
    {
        public LSSJ1Transformation(params TransformationDefinition[] parents) : base(BuffKeyNames.lssj, "Legendary Super Saiyan", TransformationDefinitionManager.defaultTransformationTextColor,
            2.30f, 2.30f, 6, 2.10f, 2.15f, 1.65f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.lssjAura, new ReadOnlyColor(0f, 0.2f, 0f), new HairAppearance("Hairs/LSSJ/LSSJHair", new ReadOnlyColor(0f, 0f, 0f), 0), HairStyleAppearance.LSSJHairStyle, Color.Turquoise),
            typeof(LSSJ1Buff),
            buffIconGetter: () => GFX.lssjButtonImage, canBeMastered: true, unlockRequirements: p => p.IsLegendary(), 
            parents: parents)
        {
        }
    }
}
