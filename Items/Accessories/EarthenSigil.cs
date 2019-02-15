using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class EarthenSigil : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The soul of the land lives within.'\n6% Increased ki damage\nIncreased ki regen\nReduced flight ki usage by 25%\n+1 Max Charges");
            DisplayName.SetDefault("Earthen Sigil");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 34;
            item.value = 300000;
            item.rare = 4;
            item.accessory = true;
            item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.06f;
                player.GetModPlayer<MyPlayer>(mod).kiRegen += 1;
                player.GetModPlayer<MyPlayer>(mod).flightKiConsumptionMultiplier *= 0.75f;
                player.GetModPlayer<MyPlayer>(mod).chargeLimitAdd += 1;
                player.GetModPlayer<MyPlayer>(mod).earthenSigil = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddIngredient(null, "EarthenShard", 10);
			recipe.AddIngredient(null, "PridefulKiCrystal", 25);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}