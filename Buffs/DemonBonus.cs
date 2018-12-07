using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

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
            MyPlayer.ModPlayer(player).AddKi(MyPlayer.ModPlayer(player).OverallKiMax());
        }
    }
}
