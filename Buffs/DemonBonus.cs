using DBZMOD.Players;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class DemonBonus : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Demonic Overdrive");
            Description.SetDefault("Your energy feels limitless");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer.ModPlayer(player).SetKi(MyPlayer.ModPlayer(player).OverallKiMax());
        }
    }
}
