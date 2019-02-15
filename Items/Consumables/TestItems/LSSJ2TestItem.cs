using DBZMOD.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.TestItems
{
    public class LSSJ2TestItem : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.consumable = false;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.expert = true;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LSSJ2 Test Item");
            Tooltip.SetDefault("Manually activates the lssj2 transformation cutscene and unlocks it.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).LSSJ2Transformation();
            UI.TransformationMenu.menuSelection = MenuSelectionID.LSSJ2;

            DBZMOD.Instance.TransformationDefinitionManager.LSSJ2Definition.Unlock(player);

            MyPlayer.ModPlayer(player).isTransforming = true;
            return true;

        }
    }
}
