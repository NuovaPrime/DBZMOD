using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class SSJ1KaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            KaioLightValue = 10f;
            Description.SetDefault("4x Damage, 4x Speed, Quickly Drains Life and Ki.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            DamageMulti = 4f;
            SpeedMulti = 4f;
            HealthDrainRate = 56;
            KiDrainRate = 6;
            KiDrainBuffMulti = 2f;
            base.Update(player, ref buffIndex);
        }
    }
}

