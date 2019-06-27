using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class BloodstainedBandana : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Change the future.'\n14% Increased Ki damage\nThorns effect.");
            DisplayName.SetDefault("Bloodstained Bandana");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 40000;
            item.rare = 4;
            item.defense = 2;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiDamage += 0.14f;
                player.thorns = .3f;
                player.GetModPlayer<MyPlayer>(mod).bloodstainedBandana = true;
            }
        }
    }
}