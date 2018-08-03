using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX100 : TransBuff
    {
        private Player player;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x100");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 10f;
            Description.SetDefault("{0}x Damage, {0}x Speed, Drains life Extremely Quickly.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (MyPlayer.RealismMode)
            {
                DamageMulti = 100;
                SpeedMulti = 100;
                HealthDrainRate = 480;
                KiDrainBuffMulti = 1f;
            }
            else if (!MyPlayer.RealismMode)
            {
                DamageMulti = 5f;
                SpeedMulti = 5f;
                HealthDrainRate = 520;
                KiDrainBuffMulti = 4f;
            }
            base.Update(player, ref buffIndex);
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (RealismModeOn)
            {
                tip = string.Format(tip, "100");
            }
            else
            {
                tip = string.Format(tip, "5");
            }

        }
    }
}

