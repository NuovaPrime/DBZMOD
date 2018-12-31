using Terraria;

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

        public string GetKaiokenNameFromKaiokenLevel(int kaiokenLevel)
        {
            switch (kaiokenLevel)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return "Kaioken";
                case 2:
                    return "Kaioken x3";
                case 3:
                    return "Kaioken x10";
                case 4:
                    return "Kaioken x20";
                case 5:
                    return "Kaioken x100";
            }
            return string.Empty;
        }

        public void CheckKaiokenName(MyPlayer player)
        {
            string kaiokenName = GetKaiokenNameFromKaiokenLevel(player.KaiokenLevel);
            this.DisplayName.SetDefault(kaiokenName);
        }

        public override void Update(Player player, ref int buffIndex)
        {   
            // makes it so that kaioken is basically just one buff.
            MyPlayer modPlayer = player.GetModPlayer <MyPlayer>();
            CheckKaiokenName(modPlayer);
            if (modPlayer.KaiokenLevel == 0)
            {
                player.ClearBuff(buffIndex);
                return;
            }
            else
            {
                if (KaiokenLevel == 0)
                {
                    KaiokenLevel = modPlayer.KaiokenLevel;
                }
                else
                {
                    if (KaiokenLevel != modPlayer.KaiokenLevel)
                    {
                        KaiokenLevel = modPlayer.KaiokenLevel;
                    }
                }
            }

            DamageMulti = 1f + (0.2f * KaiokenLevel);
            SpeedMulti = 1f + (0.2f * KaiokenLevel);
            HealthDrainRate = 8 * KaiokenLevel;
            KiDrainBuffMulti = 1f + (0.2f * KaiokenLevel);
            
            base.Update(player, ref buffIndex);
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            base.ModifyBuffTip(ref tip, ref rare);
            tip = AssembleTransBuffDescription();
        }
    }
}

