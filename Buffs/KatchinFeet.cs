using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class KatchinFeet : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Katchin Feet");
            Description.SetDefault("Your feet are infused with ki, making them as hard as katchin.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noFallDmg = true;
        }
    }
}
