using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class BlackFusionShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("24% Increased Ki Damage"
                + "\n20% Increased Ki Crit Chance" +
                               "\n+1000 Max Ki" +
                               "\n+2 Maximum Charges" +
                               "\nIncreased Ki Charge Rate");
            DisplayName.SetDefault("Black Fusion Shirt");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 64000;
            item.rare = 9;
            item.defense = 24;
        }
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("BlackFusionPants");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "All damage is slowly increased while a boss is alive, limits at 300%." +
                              "\n+100 Max Life";
            player.statLifeMax2 += 100;
            MyPlayer.ModPlayer(player).blackFusionBonus = true;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.24f;
            MyPlayer.ModPlayer(player).KiCrit += 20;
            MyPlayer.ModPlayer(player).KiMax2 += 1000;
            MyPlayer.ModPlayer(player).KiChargeRate += 2;
            MyPlayer.ModPlayer(player).ChargeLimitAdd += 2;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DivineThreads", 22);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}