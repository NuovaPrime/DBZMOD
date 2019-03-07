using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.TestItems
{
    public sealed class SSJBTestItem : ModItem
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
            item.noUseGraphic = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SSJB Test Item");
            Tooltip.SetDefault("Manually activates the ssjb transformation cutscene and unlocks it.");
        }


        public override bool UseItem(Player player)
        {
            if (!DBZMOD.allowDebugItem) return false;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.SelectedTransformation = modPlayer.TransformationDefinitionManager.SSJBDefinition;

            modPlayer.TransformationDefinitionManager.SSJBDefinition.Unlock(player);

            return true;
        }
    }
}
