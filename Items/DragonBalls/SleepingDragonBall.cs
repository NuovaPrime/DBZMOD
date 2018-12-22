using DBZMOD.Items.DragonBalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBZMOD.Items.DragonBalls
{
    public class SleepingDragonBall : DragonBallItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sleeping Dragon Ball");
            Tooltip.SetDefault("This Dragon Ball is dormant, but will awaken in time.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.createTile = mod.TileType("SleepingBallTile");
        }
    }
}
