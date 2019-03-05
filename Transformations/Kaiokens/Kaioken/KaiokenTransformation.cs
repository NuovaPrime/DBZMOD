using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Extensions;

namespace DBZMOD.Transformations.Kaiokens.Kaioken
{
    public sealed class KaiokenTransformation : TransformationDefinition
    {
        public KaiokenTransformation() : base(BuffKeyNames.kaioken, null, TransformationDefinitionManager.defaultTransformationTextColor, 
            1f, 1f, 0, 1f, 0f, 0f, 8,
            new TransformationAppearanceDefinition(AuraAnimations.createKaiokenAura, new ReadOnlyColor(0.35f, 0, 0), null, null, null, null),
            typeof(KaiokenBuff))
        {
        }

        public override float GetHealthDrainRate(MyPlayer player)
        {
            float drain = 8 + (4 * (player.kaiokenLevel) - 1);

            // TODO Remove this
            if (player.player.IsAccessoryEquipped("Kaio Crystal"))
                drain /= 2f;

            return drain;
        }
    }
}
