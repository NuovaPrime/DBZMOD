using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class ZenkaiCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Zenkai Cooldown");
            Description.SetDefault("Your zenkai ability is on cooldown.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}
