using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJBs.SSJB
{
    public sealed class SSJBTransformation : TransformationDefinition
    {
        public SSJBTransformation(params TransformationDefinition[] parents) : base("Super Saiyan Blue", TransformationDefinitionManager.blueTransformationTextColor,
            4.50f, 4.50f, 20, 2.75f, 5f, 2.5f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ssjbAura, new ReadOnlyColor(0f, 0f, 2.55f), 
                new HairAppearance("Hairs/SSJB/SSJBHair", null, null, HairAppearance.SSJ1_HAIRSTYLE_KEY), Color.Blue), 
            typeof(SSJBBuff),
            buffIconGetter: () => GFX.ssjbButtonImage, hasMenuIcon: true,
            canBeMastered: true,
            unlockRequirements: p => !p.IsDivine() && !p.IsLegendary(), parents: parents)
        {
        }
    }

    public sealed class SSJBBuff : TransformationBuff
    {
        public SSJBBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.SSJBDefinition)
        {
        }
    }
}
