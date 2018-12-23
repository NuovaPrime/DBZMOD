using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            KaioLightValue = 5f;
            DamageMulti = 1.2f;
            SpeedMulti = 1.2f;
            HealthDrainRate = 8;
            KiDrainBuffMulti = 1.2f;            
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

