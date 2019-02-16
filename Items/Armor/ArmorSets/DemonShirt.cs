using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class DemonShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("18% Increased Ki Damage\n14% Increased Ki Crit Chance\n+700 Max Ki\n+1 Maximum Charges");
            DisplayName.SetDefault("Demon Shirt");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 30000;
            item.rare = 9;
            item.defense = 20;
        }
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("DemonLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Pressing `Armor Bonus` grants you Demonic Overdrive, granting infinite ki for a limited time.";
            MyPlayer.ModPlayer(player).demonBonus = true;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).kiDamage += 0.18f;
            MyPlayer.ModPlayer(player).kiCrit += 14;
            MyPlayer.ModPlayer(player).kiMax2 += 700;
            MyPlayer.ModPlayer(player).chargeLimitAdd += 1;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SatanicCloth", 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}