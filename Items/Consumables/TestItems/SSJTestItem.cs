using DBZMOD.Enums;
using DBZMOD;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.TestItems
{
    public class SSJTestItem : ModItem
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
            DisplayName.SetDefault("SSJ Test Item");
            Tooltip.SetDefault("Manually activates the ssj transformation cutscene and unlocks it.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).SSJTransformation();
            UI.TransformationMenu.menuSelection = MenuSelectionID.SSJ1;
            DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition.Unlock(player);
            MyPlayer.ModPlayer(player).isTransforming = true;
            return true;

        }
    }
}
