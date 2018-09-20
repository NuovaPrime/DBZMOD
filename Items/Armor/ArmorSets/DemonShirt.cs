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
            Tooltip.SetDefault("24% Increased Ki Damage"
                + "\n20% Increased Ki Crit Chance" +
                               "\n+1000 Max Ki" +
                               "\n+2 Maximum Charges");
            DisplayName.SetDefault("Demon Shirt");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 64000;
            item.rare = 9;
            item.defense = 11;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("DemonLeggings");
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
            MyPlayer.ModPlayer(player).KiMax += 1000;
            MyPlayer.ModPlayer(player).ChargeLimitAdd += 2;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpectreBar, 20);
            recipe.AddIngredient(null, "SatanicCloth", 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}