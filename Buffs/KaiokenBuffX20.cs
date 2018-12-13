using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX20 : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x20");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            KaioLightValue = 12f;
            Description.SetDefault("4x Damage, 4x Speed, Rapidly Drains Life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 4f;
            SpeedMulti = 3f;
            HealthDrainRate = 100;
            KiDrainBuffMulti = 3f;
            base.Update(player, ref buffIndex);
        }
    }
}

