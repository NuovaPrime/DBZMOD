using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KiPotionSickness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ki Potion Sickness");
            Description.SetDefault("You feel sick at the thought of another ki potion.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
