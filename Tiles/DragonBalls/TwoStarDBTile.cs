﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

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
            WhichDragonBallAmI = 2;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
                modPlayer.TwoStarDBNearby = true;
            }
        }
    }
}