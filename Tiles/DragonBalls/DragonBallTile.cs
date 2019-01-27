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
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16f, j * 16f), 1, 1)];
            if (player == null)
                return true;
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
            if (modPlayer.firstDragonBallPickup)
                return true;
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("DBNote"));
            modPlayer.firstDragonBallPickup = true;
            return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            DBZWorld world = DBZWorld.GetWorld();
            world.CacheDragonBallLocation(whichDragonBallAmI, Point.Zero, false);
            world.TryPlacingDragonBall(whichDragonBallAmI);
        }
    }
}
