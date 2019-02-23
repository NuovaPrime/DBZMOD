using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJG
{
    public sealed class SSJGTransformation : TransformationDefinition
    {
        public SSJGTransformation(params TransformationDefinition[] parents) : base(BuffKeyNames.ssjg, "Super Saiyan God", TransformationDefinitionManager.godTransformationTextColor,
            3.5f, 3.5f, 16, 1.5f, 3.5f, 1.75f, -2f,
            new TransformationAppearanceDefinition(AuraAnimations.ssjgAura, new ReadOnlyColor(0.2f, 0f, 0f), null, new ReadOnlyColor(255, 57, 74), 1, Color.Red),
            typeof(SSJGBuff),
            buffIconGetter: () => GFX.ssjgButtonImage, transformationFailureText: "The godlike power of the lunar star could awaken something beyond mortal comprehension.", extraTooltipText: "\nSlightly increased health regen.", canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.ssjg,
            unlockRequirements: p => !p.IsLegendary(), selectionRequirements: (p, t) => t.PlayerHasTransformationAndNotLegendary(p),
            parents: parents)
        {
        }

        public override float GetHealthDrainRate(MyPlayer player) => ModifiedHealthDrainRate * (player.masteryLevels[UnlocalizedName] >= 1 ? 2f : 1f);
    }
}
