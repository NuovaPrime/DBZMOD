using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class DBNote : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tattered Note");
            Tooltip.SetDefault("An old note attached to the dragon ball.\n'This dragon ball is a very 強力な item,\n'集める the other 6 散在 around the world\n'for チャンス to grant your 最も深い wishes.'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 0;
            item.rare = -1;
        }
    }
}