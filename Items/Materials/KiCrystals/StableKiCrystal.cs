namespace DBZMOD.Items.Materials.KiCrystals
{
    public class StableKiCrystal : KiCrystal
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stable Ki Crystal");
            Tooltip.SetDefault("'The common spirits of the world live within.'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 9999;
            item.value = 100;
            item.rare = 1;
        }
    }
}