using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class ScrapMetal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scrap Metal");
            Tooltip.SetDefault("An old piece of capsule corp metal.");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 9999;
            item.value = 500;
            item.rare = 2;
        }
    }
}