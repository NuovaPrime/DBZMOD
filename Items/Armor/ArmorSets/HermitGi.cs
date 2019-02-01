using DBZMOD.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class HermitGi : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("5% Increased Ki Damage\n3% Increased Ki Crit Chance\nIncreased ki regen");
            DisplayName.SetDefault("Turtle Hermit Gi");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 18000;
            item.rare = 3;
            item.defense = 8;
        }
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("HermitPants");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "15% reduced ki usage and +200 Max Ki.";
            MyPlayer.ModPlayer(player).kiDrainMulti -= 0.15f;
            MyPlayer.ModPlayer(player).kiMax2 += 200;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.05f;
            MyPlayer.ModPlayer(player).kiCrit += 3;
            MyPlayer.ModPlayer(player).kiRegen += 1;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(null, "EarthenShard", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}