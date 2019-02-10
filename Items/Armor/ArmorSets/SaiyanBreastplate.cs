using DBZMOD;
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
            Tooltip.SetDefault("10% Increased Ki Damage\n6% Increased Ki Crit Chance\n+100 Max ki");
            DisplayName.SetDefault("Saiyan Battle Breastplate");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 22000;
            item.rare = 4;
            item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("SaiyanLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Drastically increased pickup range for ki orbs.\n+500 Max Ki and 8% Reduced Ki Usage";
            MyPlayer.ModPlayer(player).orbGrabRange += 9;
            MyPlayer.ModPlayer(player).kiMax2 += 500;
            MyPlayer.ModPlayer(player).kiDrainMulti -= 0.08f;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.10f;
            MyPlayer.ModPlayer(player).kiCrit += 6;
            MyPlayer.ModPlayer(player).kiMax2 += 100;

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