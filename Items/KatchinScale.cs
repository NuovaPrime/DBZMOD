using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class KatchinScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Katchin Scale");
            Tooltip.SetDefault("'A scale with incredible durability ripped from a massive fish.'");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 16;
            item.maxStack = 9999;
            item.value = 2500;
            item.rare = 7;
        }
    }
}