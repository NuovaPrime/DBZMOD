using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class StableKiCrystal : ModItem
    {
        public int kiValue = 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stable Ki Crystal");
            Tooltip.SetDefault("-Tier 1 Material-");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.maxStack = 9999;
            item.value = 100;
            item.rare = 1;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).KiMax += kiValue;
            return true;
        }
    }
}