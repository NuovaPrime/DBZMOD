using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class EnhancedReserves : ModBuff
    {
        private int RegenTimer;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Enhanced Reserves");
            Description.SetDefault("Your ki reserves have been enhanced.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer.ModPlayer(player).AddKi(0.2f, false, false);
        }
    }
}
