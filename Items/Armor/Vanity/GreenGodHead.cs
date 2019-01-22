using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class GreenGodHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The head of a god of morality.'");
            DisplayName.SetDefault("Green God Head");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 16;
            item.value = 30000;
            item.rare = 7;
            item.expert = true;
            item.vanity = true;
        }
    }
}