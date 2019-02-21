using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class LSSJBuff : TransformationBuff
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            DisplayName.SetDefault("Legendary Super Saiyan");
            
            damageMulti = 2.30f;
            speedMulti = 2.30f;
            kiDrainBuffMulti = 2.1f;
            kiDrainRate = 2.15f;
            kiDrainRateWithMastery = 1.65f;
            baseDefenceBonus = 6;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

