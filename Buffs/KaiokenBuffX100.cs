using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX100 : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x100");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            KaioLightValue = 10f;
            DamageMulti = 2f;
            SpeedMulti = 2f;
            HealthDrainRate = 80;
            KiDrainBuffMulti = 2f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

