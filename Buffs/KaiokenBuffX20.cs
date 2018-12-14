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
            Main.debuff[Type] = true;
            KaioLightValue = 12f;
            DamageMulti = 4f;
            SpeedMulti = 4f;
            HealthDrainRate = 100;
            KiDrainBuffMulti = 3f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

