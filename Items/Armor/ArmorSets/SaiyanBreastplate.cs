using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class SaiyanBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("7% Increased Ki Damage"
                + "\n3% Increased Ki Crit Chance");
            DisplayName.SetDefault("Saiyan Breastplate");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 11000;
            item.rare = 4;
            item.defense = 11;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("SaiyanLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Drastically increased pickup range for ki orbs.";
            MyPlayer.ModPlayer(player).OrbGrabRange += 3;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.7f;
            MyPlayer.ModPlayer(player).KiCrit += 3;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddIngredient(null, "SkeletalEssence", 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}