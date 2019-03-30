using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DeveloperItems
{
    public sealed class TransformationCountForPlayers : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.consumable = false;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 1;
            item.value = 0;
            item.expert = true;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Count Transformations");
        }


        public override bool UseItem(Player player)
        {
            if (!DBZMOD.allowDebugItem) return false;

            foreach (Player ply in Main.player)
            {
                MyPlayer myPlayer = ply.GetModPlayer<MyPlayer>();
                if (myPlayer == null || string.IsNullOrWhiteSpace(myPlayer.player.name)) return false;

                Main.NewText($"{ply.name} -- {string.Join(", ", myPlayer.ActiveTransformations)}");
            }


            return true;
        }
    }
}
