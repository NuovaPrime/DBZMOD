using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class TransformationExhaustionBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Transformation Exhaustion");
            Description.SetDefault("Your body can't handle another transformation.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
