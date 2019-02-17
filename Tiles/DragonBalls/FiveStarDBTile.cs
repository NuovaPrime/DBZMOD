using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace DBZMOD.Tiles.DragonBalls
{
    public class FiveStarDBTile : DragonBallTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("5 Star Dragon Ball");
            drop = mod.ItemType("FiveStarDB");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            whichDragonBallAmI = 5;
        }
    }
}