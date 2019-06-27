﻿using Microsoft.Xna.Framework;
 using Terraria.ModLoader;

namespace DBZMOD.Tiles.DragonBalls
{
    public class OneStarDBTile : DragonBallTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("1 Star Dragon Ball");
            drop = mod.ItemType("OneStarDB");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            whichDragonBallAmI = 1;
        }
    }
}