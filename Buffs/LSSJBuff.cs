using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class LSSJBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Legendary Super Saiyan");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            DamageMulti = 2.30f;
            SpeedMulti = 2.30f;
            KiDrainBuffMulti = 2.1f;
            KiDrainRate = 2.15f;
            BaseDefenceBonus = 30;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

