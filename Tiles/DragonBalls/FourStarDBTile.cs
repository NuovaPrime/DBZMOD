﻿using Microsoft.Xna.Framework;
 using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Tiles.DragonBalls
{
    public class FourStarDBTile : DragonBallTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("4 Star Dragon Ball");
            drop = mod.ItemType("FourStarDB");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            whichDragonBallAmI = 4;
        }
    }
}