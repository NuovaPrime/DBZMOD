﻿using Microsoft.Xna.Framework;
 using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Tiles.DragonBalls
{
    public class TwoStarDBTile : DragonBallTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("2 Star Dragon Ball");
            drop = mod.ItemType("TwoStarDB");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            whichDragonBallAmI = 2;
        }
    }
}