using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DBZMOD.Items.Armor.ArmorSets
{
    [AutoloadEquip(EquipType.Body)]
    public class RadiantScalepiece : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("24% Increased Ki Damage"
                + "\n16% Increased Ki knockback" +
                               "\n+1500 Max Ki" +
                               "\n+4 Maximum Charges" +
                               "\nReduced flight ki usage");
            DisplayName.SetDefault("Radiant Scalepiece");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.value = 68000;
            item.rare = 10;
            item.defense = 26;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("RadiantVisor") && legs.type == mod.ItemType("RadiantGreaves");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Hitting enemies heals a random amount of ki and fires off homing projectiles at nearby enemies." +
                "\nIncreased Ki Regen and +200 Max Life";
            player.statLifeMax2 += 200;
            MyPlayer.ModPlayer(player).radiantBonus = true;
            MyPlayer.ModPlayer(player).kiRegen += 3;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).kiDamage += 0.24f;
            MyPlayer.ModPlayer(player).kiKbAddition += 16;
            MyPlayer.ModPlayer(player).kiMax2 += 1500;
            MyPlayer.ModPlayer(player).chargeLimitAdd += 4;
            MyPlayer.ModPlayer(player).flightUsageAdd += 1;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(null, "RadiantFragment", 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}