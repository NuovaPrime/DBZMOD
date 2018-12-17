using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX10 : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x10");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            KaioLightValue = 10f;
            DamageMulti = 3f;
            SpeedMulti = 3f;
            HealthDrainRate = 58;
            KiDrainBuffMulti = 2f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

