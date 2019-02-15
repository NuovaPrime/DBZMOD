using Terraria;
using Terraria.ModLoader;
 
namespace DBZMOD.Buffs
{
    public class NimbusBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Nimbus Cloud");
            Description.SetDefault("Only Those With True Hearts Can Ride This.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("NimbusMount"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}