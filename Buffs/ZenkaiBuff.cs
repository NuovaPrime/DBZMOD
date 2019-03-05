using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    // TODO Make this work again
    public sealed class ZenkaiBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Zenkai");
            Description.SetDefault("Your inherent saiyan ability is active.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;     
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.zenkaiCharmActive = true;

            // TODO Add damage multiplier

            base.Update(player, ref buffIndex);
        }
    }
}
