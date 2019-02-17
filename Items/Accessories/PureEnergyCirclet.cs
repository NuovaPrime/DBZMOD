using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class PureEnergyCirclet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'It radiates a unbelievably pure presence.'\n12% Increased Ki damage\nIncreased Ki regen\n+300 Max Ki\nCharging grants a aura of inferno and frostburn around you.");
            DisplayName.SetDefault("Pure Energy Circlet");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 120000;
            item.rare = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiDamage += 0.12f;
                player.GetModPlayer<MyPlayer>(mod).kiRegen += 2;
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 300;
                player.GetModPlayer<MyPlayer>(mod).pureEnergyCirclet = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BurningEnergyAmulet", 1);
            recipe.AddIngredient(null, "IceTalisman", 1);
            recipe.AddIngredient(ItemID.LightShard, 1);
            recipe.AddIngredient(ItemID.DarkShard, 1);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}