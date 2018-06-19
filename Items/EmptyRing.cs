using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class EmptyRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An empty ring.");
            DisplayName.SetDefault("Empty Ring");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = 0;
            item.rare = 2;
        }
    }
}