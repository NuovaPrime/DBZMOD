using Terraria;

namespace DBZMOD.Buffs
{
    public class ZenkaiBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Zenkai");
            Description.SetDefault("Your inherent saiyan ability is active.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            damageMulti = 2f;
            speedMulti = 1f;          
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer.ModPlayer(player).zenkaiCharmActive = true;
            base.Update(player, ref buffIndex);
        }
    }
}
