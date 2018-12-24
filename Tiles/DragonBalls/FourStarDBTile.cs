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
    public class FourStarDBTile : DragonBallTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("4 Star Dragon Ball");
            AddMapEntry(new Color(249, 193, 49), name);
            disableSmartCursor = true;
            WhichDragonBallAmI = 4;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
                modPlayer.FourStarDBNearby = true;
            }
        }

        // the four star is special, it has its own drop method
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