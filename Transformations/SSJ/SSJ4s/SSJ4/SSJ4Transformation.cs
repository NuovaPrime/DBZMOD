using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJ4s.SSJ4
{
    public sealed class SSJ4Transformation : TransformationDefinition
    {
        public SSJ4Transformation(params TransformationDefinition[] parents) : base(BuffKeyNames.ssj4, "Super Saiyan 4", TransformationDefinitionManager.defaultTransformationTextColor,
            3.30f, 3.30f, 16, 2.30f, 250f / 60, 125f / 60, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ssj4Aura, new ReadOnlyColor(0.4f, 0.4f, 0f), "Hairs/SSJ/SSJ4Hair", null, null, Color.Red),
            typeof(SSJ4Buff),
            () => GFX.ssj4ButtonImage, canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.ssj4,
            unlockRequirements: p => p.IsPrimal(), parents: parents)
        {
        }
    }
}
