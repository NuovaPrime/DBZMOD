using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ3Buff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan 3");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("{0}x Damage, {0}x Speed, Rapidly Drains Ki and slightly drains life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (DBZWorld.RealismMode)
            {
                DamageMulti = 20f;
                SpeedMulti = 20f;
                KiDrainRate = 6;
                KiDrainBuffMulti = 1f;
                HealthDrainRate = 10;
            }
            else if (!DBZWorld.RealismMode)
            {
                DamageMulti = 4f;
                SpeedMulti = 4f;
                KiDrainRate = 6;
                KiDrainBuffMulti = 2f;
                HealthDrainRate = 20;
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

