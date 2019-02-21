using Terraria;

namespace DBZMOD.Transformations.SSJ1
{
    public class SSJ1Buff : TransformationBuff
    {
        public SSJ1Buff() : base(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition)
        {
        }

        public override void SetDefaults()
        {
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

