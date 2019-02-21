using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class SSJ1Buff : TransformationBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;

            damageMulti = 1.50f;
            speedMulti = 1.50f;
            kiDrainBuffMulti = 1.25f;
            kiDrainRate = 1;
            kiDrainRateWithMastery = 0.5f;
            baseDefenceBonus = 4;
            Description.SetDefault(AssembleTransBuffDescription());
        }  
    }
}

