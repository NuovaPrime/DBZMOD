using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            IsKaioken = true;
            KaioLightValue = 5f;
            Description.SetDefault("50% More Damage, 2x Speed, Slowly Drains Life.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 1.5f;
            SpeedMulti = 2f;
            HealthDrainRate = 28;
            KiDrainBuffMulti = 1f;
            base.Update(player, ref buffIndex);
        }
    }
}

