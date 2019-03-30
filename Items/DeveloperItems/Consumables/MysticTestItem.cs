using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DeveloperItems.Consumables
{
    public sealed class MysticTestItem : ModItem
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
            DisplayName.SetDefault("Mystic Test Item");
            Tooltip.SetDefault("Manually activates the mystic transformation cutscene and unlocks it.");
        }


        public override bool UseItem(Player player)
        {
            if (!DBZMOD.allowDebugItem) return false;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.SelectedTransformation = DBZMOD.Instance.TransformationDefinitionManager.MysticDefinition;

            DBZMOD.Instance.TransformationDefinitionManager.MysticDefinition.Unlock(player);

            //modPlayer.isTransforming = true;
            return true;
        }
    }
}
