using Terraria;

namespace DBZMOD.Buffs
{
    public class SSJGBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Super Saiyan God");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            damageMulti = 3.5f;
            speedMulti = 3.5f;
            kiDrainRate = 2.25f;
            kiDrainRateWithMastery = 1.65f;
            kiDrainBuffMulti = 1.5f;
            baseDefenceBonus = 16;
            Description.SetDefault(AssembleTransBuffDescription() + "\nSlightly increased health regen.");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 2;
            base.Update(player, ref buffIndex);
        }
    }
}

