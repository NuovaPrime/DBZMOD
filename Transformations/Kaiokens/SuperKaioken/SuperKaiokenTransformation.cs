using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;

namespace DBZMOD.Transformations.Kaiokens.SuperKaioken
{
    public sealed class SuperKaiokenTransformation : TransformationDefinition
    {
        public SuperKaiokenTransformation() : base(BuffKeyNames.superKaioken, "Super Kaioken", TransformationDefinitionManager.defaultTransformationTextColor, 
            2.25f, 2.25f, 8, 1.625f, 2, 1, 16,
            new TransformationAppearanceDefinition(AuraAnimations.createSuperKaiokenAura, new ReadOnlyColor(0.2f, 0f, 0f), new HairAppearance("Hairs/SSJ/SSJKKHair", new ReadOnlyColor(0f, 0f, 0f), 0), HairStyleAppearance.SSJKKHairStyle, null),
            typeof(SuperKaiokenBuff))
        {
        }

        public override float GetHealthDrainRate(MyPlayer player) => ModifiedHealthDrainRate + (4 * player.kaiokenLevel - 1);
    }
}
