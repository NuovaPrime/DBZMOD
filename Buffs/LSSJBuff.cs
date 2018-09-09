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
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = false;
            IsSSJ = true;
            Description.SetDefault("3.7x Damage, 3.7x Speed, Quickly Drains Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 3.7f;
            SpeedMulti = 3.7f;
            KiDrainBuffMulti = 3f;
            KiDrainRate = 5;
            base.Update(player, ref buffIndex);
        }
    }
}

