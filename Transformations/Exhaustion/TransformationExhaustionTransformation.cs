using DBZMOD.Buffs;
using Terraria;

namespace DBZMOD.Transformations.Exhaustion
{
    public sealed class TransformationExhaustionTransformation : TransformationDefinition
    {
        public TransformationExhaustionTransformation() : base(null, TransformationDefinitionManager.defaultTransformationTextColor,
            1f, 1f, 0, 1f, 0f, 0f, 0f,
            new TransformationAppearanceDefinition(null, null, new HairAppearance(null, null, null, null), null),
            typeof(TransformationExhaustionBuff),
            exhaustsPlayer: false, hasMenuIcon: false)
        {
        }
    }

    public sealed class TransformationExhaustionBuff : TransformationBuff
    {
        public TransformationExhaustionBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.TransformationExhaustionDefinition)
        {
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Transformation Exhaustion");
            Description.SetDefault("Your body can't handle another transformation.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
