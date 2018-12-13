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
            Description.SetDefault("2.8x Damage, 2.8x Speed, Quickly Drains Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 2.8f;
            SpeedMulti = 1.8f;
            KiDrainBuffMulti = 3f;
            KiDrainRate = 5;
            base.Update(player, ref buffIndex);
        }
    }
}

