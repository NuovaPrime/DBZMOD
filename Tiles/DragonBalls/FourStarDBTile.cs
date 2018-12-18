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
    public class FourStarDBTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileShine[Type] = 1150;
            Main.tileShine2[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.Style = 0;
            TileObjectData.newTile.RandomStyleRange = 0;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("4 Star Dragon Ball");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
        }

        public override bool Drop(int i, int j)
        {
            Player player = Main.LocalPlayer;
            MyPlayer modplayer = player.GetModPlayer<MyPlayer>(mod);

            if (!modplayer.FirstFourStarDBPickup)
            {
                Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("FourStarDB"));
                Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("DBNote"));
                modplayer.FirstFourStarDBPickup = true;
            }
            else
            {
                Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("FourStarDB"));
            }
            return false;
        }
    }
}