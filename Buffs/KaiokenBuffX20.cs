using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX20 : TransBuff
    {
        private Player player;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x20");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 12f;
            Description.SetDefault("{0}x Damage, {0}x Speed, Rapidly Drains Life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (MyPlayer.RealismMode)
            {
                DamageMulti = 20;
                SpeedMulti = 20;
                HealthDrainRate = 120;
                KiDrainBuffMulti = 1f;
            }
            else if (!MyPlayer.RealismMode)
            {
                DamageMulti = 4f;
                SpeedMulti = 4f;
                HealthDrainRate = 140;
                KiDrainBuffMulti = 3f;
            }
            base.Update(player, ref buffIndex);
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (RealismModeOn)
            {
                tip = string.Format(tip, "20");
            }
            else
            {
                tip = string.Format(tip, "4");
            }

        }
    }
}

