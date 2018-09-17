using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX10 : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x10");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 10f;
            Description.SetDefault("3x Damage, 3x Speed, Quickly Drains Life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 3f;
            SpeedMulti = 3f;
            HealthDrainRate = 58;
            KiDrainBuffMulti = 2f;
            base.Update(player, ref buffIndex);
        }
    }
}

