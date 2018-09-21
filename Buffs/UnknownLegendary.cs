using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class UnknownLegendary : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("???");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            Description.SetDefault("Your body seems a bit special.");
        }
    }
}

