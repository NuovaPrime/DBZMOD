using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DBZMOD.Items.Accessories
{
    public class SpiritCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'An emblem enscribed with markings of the dragon.'\n20% Increased Ki Damage\nAll damage increased by 14%\n+500 Maximum ki\n12% reduced damage\n+3 max minions\nGreatly increased life regen\nAll crit increased by 12%.");
            DisplayName.SetDefault("Spirit Charm");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 140000;
            item.rare = 5;
            item.accessory = true;
            item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.20f;
                player.GetModPlayer<MyPlayer>(mod).kiCrit += 12;
                player.GetModPlayer<MyPlayer>(mod).kiMax2 += 500;
                player.endurance += 0.12f;
                player.meleeDamage += 0.14f;
                player.meleeSpeed += 0.14f;
                player.meleeCrit += 12;
                player.magicDamage += 0.14f;
                player.magicCrit += 12;
                player.rangedDamage += 0.14f;
                player.rangedCrit += 12;
                player.lifeRegen += 3;
                player.minionDamage += 0.012f;
                player.maxMinions += 3;
                player.GetModPlayer<MyPlayer>(mod).spiritCharm = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritualEmblem");
            recipe.AddIngredient(null, "DragonGemNecklace");
            recipe.AddIngredient(ItemID.Ectoplasm, 12);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}