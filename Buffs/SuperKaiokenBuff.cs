using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SuperKaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            DamageMulti = 2.25f;
            SpeedMulti = 2.25f;
            KiDrainBuffMulti = 1.625f;
            KiDrainRate = 2;
            KiDrainRateWithMastery = 1;
            KaiokenLevel = 1;
            BaseDefenceBonus = 8;
            Description.SetDefault(AssembleTransBuffDescription());
        }

        public override void Update(Player player, ref int buffIndex)
        {
            bool isMastered = MyPlayer.ModPlayer(player).MasteryLevel1 >= 1;

            KiDrainRate = !isMastered ? KiDrainRate : KiDrainRateWithMastery;
            HealthDrainRate = GetHealthDrain(player);
            base.Update(player, ref buffIndex);
        }

        public static int GetHealthDrain(Player player)
        {
            if (!Transformations.IsSuperKaioken(player))
                return 0;
            return 8;
        }
    }
}

