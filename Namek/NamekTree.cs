using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace DBZMOD.Namek
{
    public class NamekTree : ModTree
    {
        private Mod mod
        {
            get
            {
                return ModLoader.GetMod("DBZMOD");
            }
        }
        public override int DropWood()
        {
            return mod.ItemType("NamekWood");
        }

        public override Texture2D GetTexture()
        {
            return mod.GetTexture("Tiles/NamekTree");
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            return mod.GetTexture("Tiles/NamekTreeTops");
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return mod.GetTexture("Tiles/NamekTreeBranches");
        }
    }
}