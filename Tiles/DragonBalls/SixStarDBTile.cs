﻿using Microsoft.Xna.Framework;
 using Terraria.ModLoader;

namespace DBZMOD.Tiles.DragonBalls
{
    public class SixStarDBTile : DragonBallTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("6 Star Dragon Ball");
            drop = mod.ItemType("SixStarDB");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            whichDragonBallAmI = 6;
        }
    }
}