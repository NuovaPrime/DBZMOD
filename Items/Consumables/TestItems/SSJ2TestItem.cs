using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.TestItems
{
    public class SSJ2TestItem : ModItem
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
            DisplayName.SetDefault("SSJ2 Test Item");
            Tooltip.SetDefault("Manually activates the ssj2 transformation cutscene and unlocks it.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.SSJ2Transformation();
            modPlayer.SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition;

            DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition.Unlock(player);

            modPlayer.isTransforming = true;
            return true;

        }
    }
}
