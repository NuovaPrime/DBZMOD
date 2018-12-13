using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ1KaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            KaioLightValue = 10f;
            DamageMulti = 4f;
            SpeedMulti = 4f;
            HealthDrainRate = 56;
            KiDrainRate = 6;
            KiDrainBuffMulti = 2f;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

