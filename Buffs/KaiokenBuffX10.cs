using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX10 : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x10");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 10f;
            Description.SetDefault("{0}x Damage, {0}x Speed, Quickly Drains Life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (MyPlayer.RealismMode)
            {
                DamageMulti = 10;
                SpeedMulti = 10;
                HealthDrainRate = 60;
                KiDrainBuffMulti = 1f;
            }
            else if (!MyPlayer.RealismMode)
            {
                DamageMulti = 3f;
                SpeedMulti = 3f;
                HealthDrainRate = 72;
                KiDrainBuffMulti = 2f;
            }
            base.Update(player, ref buffIndex);
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (RealismModeOn)
            {
                tip = string.Format(tip, "10");
            }
            else
            {
                tip = string.Format(tip, "3");
            }

        }
    }
}

