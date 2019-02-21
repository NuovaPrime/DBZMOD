using System;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Traits
{
    public sealed class UnknownTraitBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("???");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            Description.SetDefault("Your body seems a bit special." + (new Random().Next(0, 100) > 90 ? "\nYou should go get that checked out." : ""));
        }
    }
}

