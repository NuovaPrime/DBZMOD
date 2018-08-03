using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX3 : TransBuff
    {
        private Player player;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x3");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 7f;
            Description.SetDefault("2.5x Damage, 2.5x Speed, Drains Life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (MyPlayer.RealismMode)
            {
                DamageMulti = 3;
                SpeedMulti = 3;
                HealthDrainRate = 30;
                KiDrainBuffMulti = 1f;
            }
            else if (!MyPlayer.RealismMode)
            {
                DamageMulti = 2.5f;
                SpeedMulti = 2.5f;
                HealthDrainRate = 36;
                KiDrainBuffMulti = 1.5f;
            }
            base.Update(player, ref buffIndex);
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (RealismModeOn)
            {
                tip = string.Format(tip, "3");
            }
            else
            {
                tip = string.Format(tip, "2.5");
            }

        }
    }
}

