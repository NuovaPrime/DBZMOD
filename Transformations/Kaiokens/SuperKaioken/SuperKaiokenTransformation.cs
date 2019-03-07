using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;

namespace DBZMOD.Transformations.Kaiokens.SuperKaioken
{
    public sealed class SuperKaiokenTransformation : TransformationDefinition
    {
        public SuperKaiokenTransformation() : base("Super Kaioken", TransformationDefinitionManager.defaultTransformationTextColor, 
            2.25f, 2.25f, 8, 1.625f, 2, 1, 16,
            new TransformationAppearanceDefinition(AuraAnimations.createSuperKaiokenAura, new ReadOnlyColor(0.2f, 0f, 0f), 
                new HairAppearance("Hairs/SSJ1/SSJ1KaiokenHair", null, null, null), null),
            typeof(SuperKaiokenBuff), hasMenuIcon: false)
        {
        }

        public override float GetHealthDrainRate(MyPlayer player) => ModifiedHealthDrainRate + (4 * player.kaiokenLevel - 1);
    }

    public class SuperKaiokenBuff : TransformationBuff
    {
        public SuperKaiokenBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.SuperKaiokenDefinition)
        {
        }
    }
}
