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
            DamageMulti = 3.2f;
            SpeedMulti = 3.2f;
            KiDrainBuffMulti = 2.9f;
            KiDrainRate = 3;
            BaseDefenceBonus = 56;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

