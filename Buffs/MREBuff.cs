using DBZMOD.Players;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class MREBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("MRE");
            Description.SetDefault("You feel extremely full and super refreshed.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer.ModPlayer(player).kiRegen += 1;
            player.lifeRegen += 2;
            player.statDefense += 7;
            player.lifeMagnet = true;
            MyPlayer.ModPlayer(player).orbGrabRange += 2;
        }
    }
}
