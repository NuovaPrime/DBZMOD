using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class SaiyanScoutScalemail : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("3% Increased Ki Damage\n2% Increased Ki Crit Chance\n5% reduced ki usage");
            DisplayName.SetDefault("Saiyan Scout Scalemail");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 8000;
            item.rare = 2;
            item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == mod.ItemType("SaiyanScoutPants");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+100 Max Ki.";
            MyPlayer.ModPlayer(player).kiMax2 += 100;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).KiDamage += 0.03f;
            MyPlayer.ModPlayer(player).kiCrit += 2;
            MyPlayer.ModPlayer(player).kiDrainMulti -= 0.05f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 18);
            recipe.AddIngredient(ItemID.CopperBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.Silk, 18);
            recipe2.AddIngredient(ItemID.TinBar, 8);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}