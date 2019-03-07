using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.LSSJ
{
    public sealed class LSSJ1Transformation : TransformationDefinition
    {
        public LSSJ1Transformation(params TransformationDefinition[] parents) : base("Legendary Super Saiyan", TransformationDefinitionManager.defaultTransformationTextColor,
            2.30f, 2.30f, 6, 2.10f, 2.15f, 1.65f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.lssjAura, new ReadOnlyColor(0f, 0.2f, 0f), 
                new HairAppearance("Hairs/LSSJ/LSSJHair", null, null, HairAppearance.SSJ2_HAIRSTYLE_KEY), Color.Turquoise),
            typeof(LSSJ1Buff),
            buffIconGetter: () => GFX.lssjButtonImage, hasMenuIcon: true,
            canBeMastered: true, unlockRequirements: p => p.IsLegendary(), 
            parents: parents)
        {
        }

        public override string UnlocalizedName => "LSSJBuff";
    }

    public sealed class LSSJ1Buff : TransformationBuff
    {
        public LSSJ1Buff() : base(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition)
        {
        }
    }
}
