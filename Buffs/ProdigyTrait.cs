using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class ProdigyTrait : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Prodigy");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            Description.SetDefault("You are truly gifted.\nFaster mastery gains.");
        }
    }
}

