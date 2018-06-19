using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class RubyRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% Increased magic damage and crit chance.");
            DisplayName.SetDefault("Ruby Ring");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = 30000;
            item.rare = 3;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.magicDamage += 0.09f;
                player.magicCrit += 9;
                player.GetModPlayer<MyPlayer>(mod).rubyRing = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EmptyRing");
            recipe.AddIngredient(ItemID.Ruby, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}