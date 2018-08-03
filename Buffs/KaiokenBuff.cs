using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 5f;
            Description.SetDefault("{0}x Damage, {0}x Speed, Slowly Drains Life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (MyPlayer.RealismMode)
            {
                DamageMulti = 2;
                SpeedMulti = 2;
                HealthDrainRate = 24;
                KiDrainBuffMulti = 1f;
            }
            else if (!MyPlayer.RealismMode)
            {
                DamageMulti = 2f;
                SpeedMulti = 2f;
                HealthDrainRate = 24;
                KiDrainBuffMulti = 1f;
            }
            base.Update(player, ref buffIndex);
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (RealismModeOn)
            {
                tip = string.Format(tip, "2");
            }
            else
            {
                tip = string.Format(tip, "2");
            }

        }
    }
}

