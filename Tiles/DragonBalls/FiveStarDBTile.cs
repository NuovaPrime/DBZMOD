﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DBZMOD.Tiles.DragonBalls
{
    public class FiveStarDBTile : ModTile
    {
        private int frameX;
        private int frameY;
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("5 Star Dragon Ball");
            drop = mod.ItemType("FiveStarDB");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            Main.tileLighted[Type] = true;
        }
    }
}