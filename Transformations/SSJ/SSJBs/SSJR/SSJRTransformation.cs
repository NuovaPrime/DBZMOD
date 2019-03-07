using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJBs.SSJR
{
    public sealed class SSJRTransformation : TransformationDefinition
    {
        public SSJRTransformation(params TransformationDefinition[] parents) : base("Super Saiyan Rosé", TransformationDefinitionManager.roseTransformationTextColor,
            4.75f, 4.75f, 22, 2.75f, 310f / 60, 155f / 60, 0f, 
            new TransformationAppearanceDefinition(AuraAnimations.ssjrAura, new ReadOnlyColor(0.5f, 0.1f, 0.4f), 
                new HairAppearance("Hairs/SSJR/SSJRHair", null, null, HairAppearance.BASE_HAIRSTYLE_KEY), Color.LightPink),
            typeof(SSJRBuff),
            buffIconGetter: () => GFX.ssjrButtonImage, hasMenuIcon: true,
            canBeMastered: true,
            unlockRequirements: p => p.IsDivine(), parents: parents)
        {
        }
    }

    public sealed class SSJRBuff : TransformationBuff
    {
        public SSJRBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.SSJRDefinition)
        {
        }
    }
}
