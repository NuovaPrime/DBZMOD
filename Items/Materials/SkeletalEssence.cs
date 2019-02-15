namespace DBZMOD.Items.Materials
{
    public class SkeletalEssence : DBItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletal Essence");
            Tooltip.SetDefault("'A chunk of the dungeon's inhabitants.'");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 9999;
            item.value = 300;
            item.rare = 3;
        }
    }
}