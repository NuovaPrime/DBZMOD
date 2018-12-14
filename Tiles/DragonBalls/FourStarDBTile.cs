﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DBZMOD.Tiles.DragonBalls
{
    public class FourStarDBTile : ModTile
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
            name.SetDefault("4 Star Dragon Ball");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            Main.tileLighted[Type] = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Player player = Main.LocalPlayer;
            MyPlayer modplayer = player.GetModPlayer<MyPlayer>(mod);

            if(!modplayer.FirstFourStarDBPickup)
            {
                Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("FourStarDB"));
                Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("DBNote"));
                modplayer.FirstFourStarDBPickup = true;
            }
            else
            {
                Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("FourStarDB"));
            }
            
        }
        public override void RightClick(int i, int j)
        {
            KillMultiTile(i, j, frameX, frameY);
            KillSound(i, j);
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("FourStarDB");
        }
    }
}