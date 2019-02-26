using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJBs.SSJR
{
    public sealed class SSJRTransformation : TransformationDefinition
    {
        public SSJRTransformation(params TransformationDefinition[] parents) : base(BuffKeyNames.ssjr, "Super Saiyan Rosé", TransformationDefinitionManager.roseTransformationTextColor,
            4.75f, 4.75f, 22, 2.75f, 310f / 60, 155f / 60, 0f, 
            new TransformationAppearanceDefinition(AuraAnimations.ssjrAura, new ReadOnlyColor(0.5f, 0.1f, 0.4f), "Hairs/God/SSJRHair", null, null, Color.LightPink),
            typeof(SSJRBuff),
            buffIconGetter: () => GFX.ssjrButtonImage, canBeMastered: true,
            unlockRequirements: p => p.IsDivine(), parents: parents)
        {
        }
    }
}
