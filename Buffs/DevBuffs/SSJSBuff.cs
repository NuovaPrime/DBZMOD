using DBZMOD.Players;
using Terraria;

namespace DBZMOD.Buffs.DevBuffs
{
    public class SSJSBuff : TransformationBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan Spectrum");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Description.SetDefault("69x Damage, 69x Speed, Gives infinite ki regen\n'A form far beyond comprehension, only available to the true god Nuova.'");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            damageMulti = 69f;
            speedMulti = 69f;
            
            kiDrainBuffMulti = 1f;
            modPlayer.kiRegen += 9999;
            modPlayer.kiMax2 += 999999;
            base.Update(player, ref buffIndex);
        }
    }
}

