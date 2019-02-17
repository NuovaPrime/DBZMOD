using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class SSJ3Buff : TransformationBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan 3");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            damageMulti = 2.9f;
            speedMulti = 2.9f;
            kiDrainBuffMulti = 1.95f;
            kiDrainRate = 2.65f;
            kiDrainRateWithMastery = 1.325f;
            baseDefenceBonus = 12;
            Description.SetDefault($"{AssembleTransBuffDescription()}\n(Life drains when below 30% Max Ki)");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            bool isMastered = modPlayer.masteryLevels[DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition.MasteryBuffKeyName] >= 1f;

            float kiQuotient = modPlayer.GetKi() / modPlayer.OverallKiMax();
            if (kiQuotient <= 0.3f)
            {
                healthDrainRate = isMastered ? 10 : 20;
            } else
            {
                healthDrainRate = 0;
            }
            base.Update(player, ref buffIndex);
        }
    }
}

