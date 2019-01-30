using DBZMOD.Util;
using Terraria;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

namespace DBZMOD.Buffs
{
    public class KaiokenBuff : TransBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Kaioken");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;            
            Description.SetDefault(AssembleTransBuffDescription());
        }

        public string GetKaiokenNameFromKaiokenLevel(int displayKaiokenLevel)
        {
            switch (displayKaiokenLevel)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return "Kaioken";
                case 2:
                    return "Kaioken x3";
                case 3:
                    return "Kaioken x4";
                case 4:
                    return "Kaioken x10";
                case 5:
                    return "Kaioken x20";
                default:
                    return string.Empty;
            }
        }

        public void CheckKaiokenName(MyPlayer player)
        {
            var kaiokenName = GetKaiokenNameFromKaiokenLevel(player.kaiokenLevel);
            this.DisplayName.SetDefault(kaiokenName);
        }

        public override void Update(Player player, ref int buffIndex)
        {   
            // makes it so that kaioken is basically just one buff.
            var modPlayer = player.GetModPlayer <MyPlayer>();
            CheckKaiokenName(modPlayer);
            if (modPlayer.kaiokenLevel == 0)
            {
                player.ClearBuff(buffIndex);
                return;
            }

            kaiokenLevel = modPlayer.kaiokenLevel;

            damageMulti = 1f + (0.1f * kaiokenLevel);
            speedMulti = 1f + (0.1f * kaiokenLevel);
            healthDrainRate = GetHealthDrain(modPlayer);
            kiDrainBuffMulti = 1f + (0.1f * kaiokenLevel);
            
            base.Update(player, ref buffIndex);
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            base.ModifyBuffTip(ref tip, ref rare);
            tip = AssembleTransBuffDescription();
        }

        public static int GetHealthDrain(MyPlayer modPlayer)
        {
            if (!PlayerExtensions.IsKaioken(modPlayer.player))
                return 0;
            return 8 + (4 * (modPlayer.kaiokenLevel) - 1);
        }        
    }
}

