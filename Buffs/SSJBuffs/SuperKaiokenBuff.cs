using DBZMOD.Extensions;
using DBZMOD.Util;
using Terraria;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

namespace DBZMOD.Buffs.SSJBuffs
{
    public class SuperKaiokenBuff : TransformationBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            damageMulti = 2.25f;
            speedMulti = 2.25f;
            kiDrainBuffMulti = 1.625f;
            kiDrainRate = 2;
            kiDrainRateWithMastery = 1;
            kaiokenLevel = 1;
            baseDefenceBonus = 8;
            Description.SetDefault(AssembleTransBuffDescription());
        }

        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).masteryLevel1 >= 1;

            kiDrainRate = !isMastered ? kiDrainRate : kiDrainRateWithMastery;
            healthDrainRate = GetHealthDrain(player);
            base.Update(player, ref buffIndex);
        }

        public static int GetHealthDrain(Player player)
        {
            if (!player.IsSuperKaioken())
                return 0;
            return 8;
        }
    }
}

