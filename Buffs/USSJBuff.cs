using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class USSJBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ultra Super Saiyan");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("{0}x Damage, {0}x Speed, Drains Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (DBZWorld.RealismMode)
            {
                DamageMulti = 7f;
                SpeedMulti = 2f;
                KiDrainRate = 6;
                KiDrainBuffMulti = 1f;
            }
            else if (!DBZWorld.RealismMode)
            {
                DamageMulti = 2.7f;
                SpeedMulti = 2f;
                KiDrainRate = 5;
                KiDrainBuffMulti = 1f;
            }
            base.Update(player, ref buffIndex);
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (RealismModeOn)
            {
                tip = string.Format(tip, "7");
            }
            else
            {
                tip = string.Format(tip, "2.7");
            }

        }
    }
}

