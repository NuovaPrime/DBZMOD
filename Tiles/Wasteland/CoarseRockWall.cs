using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Tiles.Wasteland
{
	public class CoarseRockWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(242, 179, 70));
		}
	}
}