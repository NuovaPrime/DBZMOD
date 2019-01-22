using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class EliteSaiyanBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("26% Increased Ki Damage"
                + "\n24% Increased Ki Crit Chance" +
                               "\n+1000 Max Ki" +
                               "\n+3 Maximum Charges" +
                               "\nIncreased Ki Regen");
            DisplayName.SetDefault("Elite Saiyan Breastplate");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 72000;
            item.rare = 9;
            item.defense = 30;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("EliteSaiyanLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Taking fatal damage will instead restore you to max life with x2 damage for a short time," +
                "\n1 minute cooldown." +
                "\n+150 Max Life";
            player.statLifeMax2 += 150;
            MyPlayer.ModPlayer(player).eliteSaiyanBonus = true;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).kiDamage += 0.26f;
            MyPlayer.ModPlayer(player).kiCrit += 24;
            MyPlayer.ModPlayer(player).kiMax2 += 1000;
            MyPlayer.ModPlayer(player).kiRegen += 2;
            MyPlayer.ModPlayer(player).chargeLimitAdd += 3;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SaiyanBreastplate", 1);
            recipe.AddIngredient(null, "KatchinScale", 16);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}