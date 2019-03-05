using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Transformations.Exhaustion
{
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
