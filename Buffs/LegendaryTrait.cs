using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class LegendaryTrait : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Legendary");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Description.SetDefault("You are the saiyan of legend.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer.ModPlayer(player).KiMax *= 2;
            base.Update(player, ref buffIndex);
        }
    }
}

