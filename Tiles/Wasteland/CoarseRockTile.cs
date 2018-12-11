using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DBZMOD.Tiles.Wasteland
{
    public class CoarseRockTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            drop = mod.ItemType("CoarseRock");
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coarse Rock");
            //AddMapEntry(new Color(242, 179, 70), name);
            AddMapEntry(new Color(0, 0, 200), name);
            dustType = 75;
        }
    }
}