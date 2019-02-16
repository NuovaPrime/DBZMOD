using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class MetamoranVest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("15% Increased Ki Damage\nIncreased ki regen");
            DisplayName.SetDefault("Metamoran Vest");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 18000;
            item.rare = 10;
            item.defense = 8;
        }
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("MetamoranPants");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "8% reduced ki usage and +400 Max Ki\n5% chance to do double damage.";
            MyPlayer.ModPlayer(player).kiDrainMulti -= 0.08f;
            MyPlayer.ModPlayer(player).kiMax2 += 200;
			player.GetModPlayer<MyPlayer>(mod).metamoranSet = true;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.15f;
            MyPlayer.ModPlayer(player).kiRegen += 1;
        }
    }
}