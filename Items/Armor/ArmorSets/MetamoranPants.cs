using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Legs)]
    public class MetamoranPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% Increased Ki Damage\n20% Increased Ki Knockback\n+25% Increased movement speed");
            DisplayName.SetDefault("Metamoran Pants");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 11000;
            item.rare = 10;
            item.defense = 6;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.1f;
            MyPlayer.ModPlayer(player).kiKbAddition += 0.2f;
            player.moveSpeed += 0.25f;
        }
    }
}