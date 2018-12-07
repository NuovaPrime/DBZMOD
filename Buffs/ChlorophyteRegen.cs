using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DBZMOD.Buffs
{
    public class ChlorophyteRegen : ModBuff
    {
        private int RegenTimer;
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
            RegenTimer++;
            if(RegenTimer > 10)
            {
                MyPlayer.ModPlayer(player).AddKi(1);
                RegenTimer = 0;
            }
        }
    }
}
