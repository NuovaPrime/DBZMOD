using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class LegendaryTrait : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Legendary");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            Description.SetDefault("You are the saiyan of legend.");
        }
    }
}

