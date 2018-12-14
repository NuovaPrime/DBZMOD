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
            Main.debuff[Type] = true;
            DamageMulti = 2.8f;
            SpeedMulti = 2.8f;
            KiDrainBuffMulti = 3f;
            KiDrainRate = 5;
            Description.SetDefault(AssembleTransBuffDescription());
        }
    }
}

