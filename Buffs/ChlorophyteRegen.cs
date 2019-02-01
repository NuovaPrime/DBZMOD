using DBZMOD;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class ChlorophyteRegen : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Regen");
            Description.SetDefault("Ki and Health regen");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 3;
            MyPlayer.ModPlayer(player).AddKi(0.1f, false, false);
        }
    }
}
