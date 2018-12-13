using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX100 : TransBuff
    {
        private Player player;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x100");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            KaioLightValue = 10f;
            DamageMulti = 5f;
            SpeedMulti = 5f;
            HealthDrainRate = 380;
            KiDrainBuffMulti = 4f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

