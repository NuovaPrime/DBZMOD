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
            KiDrainBuffMulti = 3f;
            KiDrainRate = 6;
            BaseDefenceBonus = 30;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

