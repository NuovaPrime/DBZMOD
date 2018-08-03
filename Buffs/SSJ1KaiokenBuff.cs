using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ1KaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            IsSSJ = true;
            KaioLightValue = 10f;
            Description.SetDefault("{0}x Damage, {0}x Speed, Quickly Drains Life and Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (MyPlayer.RealismMode)
            {
                DamageMulti = 10;
                SpeedMulti = 10;
                HealthDrainRate = 68;
                KiDrainRate = 4;
                KiDrainBuffMulti = 1f;
            }
            else if (!MyPlayer.RealismMode)
            {
                DamageMulti = 4f;
                SpeedMulti = 4f;
                HealthDrainRate = 68;
                KiDrainRate = 8;
                KiDrainBuffMulti = 4f;
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
                tip = string.Format(tip, "4");
            }

        }
    }
}

