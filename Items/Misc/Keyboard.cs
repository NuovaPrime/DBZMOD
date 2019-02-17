using DBZMOD.Utilities;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Items.Misc
{
    public class Keyboard : KiItem
    {
        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 28;
            item.rare = ItemRarityID.Expert;
            item.useTime = 240;
            item.useAnimation = 240;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.magic = true;
            kiDrain = 2500;
            weaponType = "Forbidden";
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Keyboard");
            Tooltip.SetDefault("An old legend speaks of a mythical warrior who would loudly roar his way into battle, shattering his opponents' morale.\nHowever, what the warrior roared has long been to lost the ages...\nPeople used this device to search for clues of this legendary man so much, its a wonder it hasn't broken yet.");
        }

        public override bool UseItem(Player player)
        {
            SoundHelper.PlayCustomSound("Sounds/Keyboard", player);

            return true;
        }


    }
}
