using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class CanadianDemonShirt : PatreonItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("24% Increased Ki Damage"
                + "\n20% Increased Ki Crit Chance" +
                               "\n+1000 Max Ki" +
                               "\n+2 Maximum Charges");
            DisplayName.SetDefault("Canadian Demon Shirt");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 64000;
            item.rare = 9;
            item.defense = 22;
            PatreonName = "CanadianMRE";
        }
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("CanadianDemonLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Pressing `Armor Bonus` grants you Demonic Overdrive, granting infinite ki for a limited time.";
            MyPlayer.ModPlayer(player).DemonBonus = true;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.24f;
            MyPlayer.ModPlayer(player).KiCrit += 20;
            MyPlayer.ModPlayer(player).KiMax2 += 1000;
            MyPlayer.ModPlayer(player).ChargeLimitAdd += 2;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DemonShirt");
            recipe.AddIngredient(ItemID.RedandSilverDye);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}