using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class LSSJ2Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Legendary Super Saiyan 2");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            DamageMulti = 4.3f;
            SpeedMulti = 4.3f;
            KiDrainBuffMulti = 3f;
            KiDrainRate = 7;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

