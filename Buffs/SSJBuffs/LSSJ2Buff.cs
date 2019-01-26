using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class LSSJ2Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Legendary Super Saiyan 2");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            damageMulti = 3.2f;
            speedMulti = 3.2f;
            kiDrainBuffMulti = 2.9f;
            kiDrainRate = 3;
            baseDefenceBonus = 12;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

