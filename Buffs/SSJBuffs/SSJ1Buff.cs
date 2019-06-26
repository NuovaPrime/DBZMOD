using Terraria;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class SSJ1Buff : TransBuff
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

        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).masteryLevel1 >= 1;
            
            kiDrainRate = !isMastered ? kiDrainRate : kiDrainRateWithMastery;
            base.Update(player, ref buffIndex);
        }        
    }
}

