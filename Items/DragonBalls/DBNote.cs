using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class DBNote : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tattered Note");
            Tooltip.SetDefault("An old note attached to the dragon ball." +
                "\n'This dragon ball is _~kきけんだｓda a very powerful item, " +
                "\n'collect the ;41aたｓけてkdk other 6 scattered around the world" +
                "\n'for a chance to grant your　むだ deepest wishes.'");
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