using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables
{
    public class DisgustingGoop : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 20;
            item.consumable = true;
            item.maxStack = 99;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 24000;
            item.rare = 3;
            item.potion = false;
            item.buffType = mod.BuffType("DisgustingGoopBuff");
            item.buffTime = 5400;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disgusting Goop");
            Tooltip.SetDefault("Stablizes Ki but tastes disgusting.");
        }
    }
}
