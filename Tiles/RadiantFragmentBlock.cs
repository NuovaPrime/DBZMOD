using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Tiles
{
    public class RadiantFragmentBlock : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            drop = mod.ItemType("RadiantFragmentBlockItem");
            AddMapEntry(new Color(115, 204, 32));
            dustType = 75;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.451f;
            g = 0.8f;
            b = 0.125f;
        }
    }
}