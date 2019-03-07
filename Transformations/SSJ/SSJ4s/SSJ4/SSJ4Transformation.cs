using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJ4s.SSJ4
{
    public sealed class SSJ4Transformation : TransformationDefinition
    {
        public SSJ4Transformation(params TransformationDefinition[] parents) : base("Super Saiyan 4", TransformationDefinitionManager.defaultTransformationTextColor,
            3.30f, 3.30f, 16, 2.30f, 250f / 60, 125f / 60, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ssj4Aura, new ReadOnlyColor(0.4f, 0.4f, 0f), 
                new HairAppearance("Hairs/SSJ4/SSJ4Hair", null, null, HairAppearance.SSJ4_HAIRSTYLE_KEY), Color.Red),
            typeof(SSJ4Buff),
            buffIconGetter: () => GFX.ssj4ButtonImage, hasMenuIcon: true,
            canBeMastered: true,
            unlockRequirements: p => p.IsPrimal(), parents: parents)
        {
        }
    }

    public sealed class SSJ4Buff : TransformationBuff
    {
        public SSJ4Buff() : base(DBZMOD.Instance.TransformationDefinitionManager.SSJ4Definition)
        {
        }
    }
}
