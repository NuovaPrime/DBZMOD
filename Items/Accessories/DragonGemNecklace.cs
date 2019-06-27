using Terraria;
 using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class DragonGemNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Infused with the essence of the dragon.'\nAll effects of the previous necklaces, some enhanced.");
            DisplayName.SetDefault("Dragon Gem Necklace");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 34;
            item.value = 300000;
            item.rare = 4;
            item.accessory = true;
            item.defense = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).kiDamage += 0.07f;
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 250;
                player.endurance += 0.07f;
                player.meleeDamage += 0.07f;
                player.meleeSpeed += 0.07f;
                player.magicDamage += 0.07f;
                player.magicCrit += 7;
                player.rangedDamage += 0.07f;
                player.rangedCrit += 7;
				player.lifeRegen += 2;
				player.minionDamage += 0.07f;
                player.maxMinions += 2;
                player.statDefense += 6;
                player.GetModPlayer<MyPlayer>(mod).dragongemNecklace = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AmberNecklace");
			recipe.AddIngredient(null, "TopazNecklace");
			recipe.AddIngredient(null, "AmethystNecklace");
			recipe.AddIngredient(null, "SapphireNecklace");
			recipe.AddIngredient(null, "EmeraldNecklace");
			recipe.AddIngredient(null, "RubyNecklace");
			recipe.AddIngredient(null, "DiamondNecklace");
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}