using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJBs.SSJB
{
    public sealed class SSJBTransformation : TransformationDefinition
    {
        public SSJBTransformation(params TransformationDefinition[] parents) : base(BuffKeyNames.ssjb, "Super Saiyan Blue", TransformationDefinitionManager.blueTransformationTextColor,
            4.50f, 4.50f, 20, 2.75f, 5f, 2.5f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ssjbAura, new ReadOnlyColor(0f, 0f, 0.5f), "Hairs/God/SSJBHair", null, 0, Color.Blue), 
            typeof(SSJBBuff),
            buffIconGetter: () => GFX.ssjbButtonImage, canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.ssjb,
            unlockRequirements: p => !p.IsDivine(), parents: parents)
        {
        }
    }
}
