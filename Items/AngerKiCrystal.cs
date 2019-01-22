using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class AngerKiCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angerful Ki Crystal");
            Tooltip.SetDefault("'The corrupt rage of the world lives within.'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 9999;
            item.value = 2500;
            item.rare = 4;
        }
    }
}