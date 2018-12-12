using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DBZMOD.Items
{
    public class CoarseRock : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coarse Rock");
            Tooltip.SetDefault("A dried out and compressed piece of sand.");
        }
        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 9999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.consumable = true;
            item.useTime = 10;
            item.useStyle = 1;
            item.createTile = mod.TileType("CoarseRockTile");
        }
    }
}