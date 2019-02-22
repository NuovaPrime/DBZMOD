using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.UI.HairMenu;

namespace DBZMOD.Items.Consumables
{
    public class HairStylingKit : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 30;
            item.consumable = true;
            item.maxStack = 99;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 50000;
            item.rare = 3;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hair Styling Kit");
            Tooltip.SetDefault("Allows a saiyan to restyle their haircut.");
        }

        public override bool UseItem(Player player)
        {
            HairMenu.menuVisible = true;
            return true;
        }
    }
}
