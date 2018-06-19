using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Namek
{
    public class NamekDirtTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = false;  //true for block to emit light
            Main.tileLighted[Type] = true;
            drop = mod.ItemType("NamekDirt");   //put your CustomBlock name
        }
    }
}