using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX3 : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x3");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            KaioLightValue = 7f;
            DamageMulti = 1.4f;
            SpeedMulti = 1.4f;
            HealthDrainRate = 32;
            KiDrainBuffMulti = 1.4f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

