using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX20 : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x20");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            KaioLightValue = 12f;
            DamageMulti = 1.8f;
            SpeedMulti = 1.8f;
            HealthDrainRate = 32;
            KiDrainBuffMulti = 1.8f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

