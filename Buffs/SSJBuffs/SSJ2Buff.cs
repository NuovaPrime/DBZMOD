using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class SSJ2Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan 2");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            damageMulti = 2.25f;
            speedMulti = 2.25f;
            kiDrainBuffMulti = 1.625f;
            kiDrainRate = 2;
            kiDrainRateWithMastery = 1;
            baseDefenceBonus = 8;
            Description.SetDefault(AssembleTransBuffDescription());
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).masteryLevel2 >= 1;

            kiDrainRate = !isMastered ? kiDrainRate : kiDrainRateWithMastery;
            base.Update(player, ref buffIndex);
        }
    }
}

