using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuffX100 : TransBuff
    {
        private Player player;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken x100");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 10f;
            Description.SetDefault("5x Damage, 5x Speed, Drains life Extremely Quickly.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 5f;
            SpeedMulti = 5f;
            HealthDrainRate = 520;
            KiDrainBuffMulti = 4f;
            base.Update(player, ref buffIndex);
        }
    }
}

