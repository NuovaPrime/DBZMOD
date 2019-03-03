using Terraria;

namespace DBZMOD.Transformations.Kaiokens.Kaioken
{
    public class KaiokenBuff : TransformationBuff
    {
        public KaiokenBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.KaiokenDefinition)
        {
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
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            CheckKaiokenName(modPlayer);

            if (modPlayer.kaiokenLevel == 0)
            {
                player.ClearBuff(buffIndex);
                return;
            }

            base.Update(player, ref buffIndex);
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            base.ModifyBuffTip(ref tip, ref rare);
            tip = AssembleTransBuffDescription(Main.LocalPlayer.GetModPlayer<MyPlayer>());
        }   
    }
}

