using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DBZMOD.Items.DragonBalls
{
    public class FiveStarDB : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("5 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 5 stars inscribed on it.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.createTile = mod.TileType("FiveStarDBTile");
        }

        public override int GetWhichDragonBall()
        {
            return 5;
        }
    }
}