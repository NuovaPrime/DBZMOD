using DBZMOD.Util;
using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class ASSJBuff : TransformationBuff
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

        // per Nova's design, mastery applies to ASSJ and USSJ
        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).masteryLevel1 >= 1;

            kiDrainRate = !isMastered ? kiDrainRate : kiDrainRateWithMastery;
            base.Update(player, ref buffIndex);
        }
    }
}

