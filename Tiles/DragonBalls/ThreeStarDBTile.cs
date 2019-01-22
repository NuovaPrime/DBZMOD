﻿using Microsoft.Xna.Framework;
 using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Tiles.DragonBalls
{
    public class ThreeStarDBTile : DragonBallTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("3 Star Dragon Ball");
            drop = mod.ItemType("ThreeStarDB");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            whichDragonBallAmI = 3;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
                modPlayer.threeStarDbNearby = true;
            }
        }
    }
}