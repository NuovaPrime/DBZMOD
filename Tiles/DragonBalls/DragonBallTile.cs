using DBZMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Tiles.DragonBalls
{
    public abstract class DragonBallTile : ModTile
    {        
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileWaterDeath[Type] = false;
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
        }                

        public override void RightClick(int i, int j)
        {
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
            if (modPlayer.AllDBNearby)
            {
                modPlayer.WishActive = true;
            }
            base.RightClick(i, j);
        }
    }
}
