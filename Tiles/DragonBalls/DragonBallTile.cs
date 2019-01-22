using DBZMOD.Items.DragonBalls;
using DBZMOD.Network;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DBZMOD.Tiles.DragonBalls
{
    public abstract class DragonBallTile : ModTile
    {
        public int whichDragonBallAmI;

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
            if (modPlayer.AllDragonBallsNearby() && NobodyHasWishActive())
            {
                modPlayer.wishActive = true;
            }
        }

        public bool NobodyHasWishActive()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i] == null)
                    continue;

                if (Main.player[i].whoAmI != i)
                    continue;

                if (Main.player[i].GetModPlayer<MyPlayer>().wishActive)
                    return false;
            }

            return true;
        }        

        public override void PlaceInWorld(int i, int j, Item item)
        {
            base.PlaceInWorld(i, j, item);
            if (DebugUtil.IsDebugModeOn())
            {
                var oldTile = DBZWorld.GetWorld().GetDragonBallLocation(whichDragonBallAmI);
                if (oldTile != new Point(-1, -1) && oldTile != new Point(i, j))
                {
                    WorldGen.KillTile(oldTile.X, oldTile.Y, false, false, true);
                    Main.NewText("Replaced the Old Dragon ball with this one.");
                }
                DebugUtil.Log(string.Format("Placing db in world: {0} {1}", i, j));
                DBZWorld.GetWorld().SetDragonBallLocation(whichDragonBallAmI, new Point(i, j), true);                
            }
            else
            {
                if (DBZWorld.IsExistingDragonBall(whichDragonBallAmI))
                {
                    WorldGen.KillTile(i, j, false, false, true);
                    Main.NewText("Cheated Dragon Balls taste awful.");
                }
                else
                {
                    DebugUtil.Log(string.Format("Placing db in world: {0} {1}", i, j));
                    DBZWorld.GetWorld().SetDragonBallLocation(whichDragonBallAmI, new Point(i, j), true);
                }
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType(DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonBallAmI));
        }

        // the four star is special, it has its own drop method
        public override bool Drop(int i, int j)
        {
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            // make sure the MP client still gets the signal that the tile was killed.
            if (!fail)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetworkHelper.playerSync.SendDragonBallRemove(256, Main.myPlayer, whichDragonBallAmI);
                }
                DBZWorld.GetWorld().RemoveDragonBallLocation(whichDragonBallAmI, true);
            }
            // if we're in multiplayer let the server handle item drops.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            if (!noItem)
            {
                Item.NewItem(i * 16, j * 16, 32, 48, drop);

                Player player = Main.player[Player.FindClosest(new Vector2(i * 16f, j * 16f), 1, 1)];
                if (player != null && whichDragonBallAmI == 4)
                {
                    MyPlayer modplayer = player.GetModPlayer<MyPlayer>(mod);
                    if (!modplayer.firstFourStarDbPickup && !noItem)
                    {
                        Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("DBNote"));
                        modplayer.firstFourStarDbPickup = true;
                    }
                }
            }
            noItem = true; // kill tile behavior overrides typical drops.
        }
    }
}
