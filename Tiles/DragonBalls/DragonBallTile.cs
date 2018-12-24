using DBZMOD;
using DBZMOD.Items.DragonBalls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Util;

namespace DBZMOD.Tiles.DragonBalls
{
    public abstract class DragonBallTile : ModTile
    {
        public int WhichDragonBallAmI;

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileShine[Type] = 1150;
            Main.tileShine2[Type] = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);
        }

        public override bool HasSmartInteract()
        {
            return true;
        }

        public override void RightClick(int i, int j)
        {
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
            if (modPlayer.AllDragonBallsNearby())
            {
                modPlayer.WishActive = true;
            }
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            base.PlaceInWorld(i, j, item);
            // if in debug mode, the cheated dragon ball replaces the original location instead of anti-cheat.
            if (DebugUtil.isDebug)
            {
                var oldTile = DBZWorld.GetWorld().DragonBallLocations[WhichDragonBallAmI - 1];
                if (oldTile != new Point(-1, -1))
                {
                    WorldGen.KillTile(oldTile.X, oldTile.Y, false, false, true);
                    Main.NewText("Replaced the Old Dragon ball with this one.");
                }                
                DBZWorld.GetWorld().DragonBallLocations[WhichDragonBallAmI - 1] = new Point(i, j);
            }
            else
            {
                if (DBZWorld.IsExistingDragonBall(WhichDragonBallAmI))
                {
                    WorldGen.KillTile(i, j, false, false, true);
                    Main.NewText("Cheated Dragon Balls taste awful.");
                }
                else
                {
                    DBZWorld.GetWorld().DragonBallLocations[WhichDragonBallAmI - 1] = new Point(i, j);
                }
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType(DragonBallItem.GetDragonBallItemTypeFromNumber(WhichDragonBallAmI));
        }
    }
}
