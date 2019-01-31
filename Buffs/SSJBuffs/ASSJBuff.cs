using DBZMOD.Util;
using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class ASSJBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault(FormBuffHelper.GetASSJNamePreference());
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            damageMulti = 1.75f;
            speedMulti = 1.75f;
            kiDrainRate = 1.15f;
            kiDrainRateWithMastery = 0.575f;
            kiDrainBuffMulti = 1.4f;
            baseDefenceBonus = 5;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

