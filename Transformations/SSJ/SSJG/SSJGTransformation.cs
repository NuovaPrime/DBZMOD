using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.SSJ.SSJG
{
    public sealed class SSJGTransformation : TransformationDefinition
    {
        public SSJGTransformation(params TransformationDefinition[] parents) : base("Super Saiyan God", TransformationDefinitionManager.godTransformationTextColor,
            3.5f, 3.5f, 16, 1.5f, 3.5f, 1.75f, -2f,
            new TransformationAppearanceDefinition(AuraAnimations.ssjgAura, new ReadOnlyColor(0.2f, 0f, 0f), 
                new HairAppearance(null, new ReadOnlyColor(255, 57, 74), 1, HairAppearance.BASE_HAIRSTYLE_KEY), Color.Red),
            typeof(SSJGBuff),
            buffIconGetter: () => GFX.ssjgButtonImage, hasMenuIcon: true,
            failureText: "The godlike power of the lunar star could awaken something beyond mortal comprehension.", extraTooltipText: "\nSlightly increased health regen.", canBeMastered: true,
            unlockRequirements: p => !p.IsLegendary(),
            parents: parents)
        {
        }

        public override float GetHealthDrainRate(MyPlayer player) => ModifiedHealthDrainRate * (player.PlayerTransformations[this].Mastery >= 1 ? 2f : 1f);

        public override bool MeetsSelectionRequirements(MyPlayer player) => PlayerHasTransformationAndNotLegendary(player);

        public override void OnMasteryGained(MyPlayer player, float mastery)
        {
            if (mastery < 1f) return;

            if (player.IsDivine())
                player.TransformationDefinitionManager.SSJRDefinition.TryUnlock(player);
            else
                player.TransformationDefinitionManager.SSJBDefinition.TryUnlock(player);
        }
    }

    public sealed class SSJGBuff : TransformationBuff
    {
        public SSJGBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition)
        {
        }
    }
}
