using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class OrichalcumHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% Increased Ki Damage"
                + "\n6% Increased Ki Crit Chance" +
                "\nMaximum Ki increased by 100.");
            DisplayName.SetDefault("Orichalcum Hat");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 16;
            item.value = 8000;
            item.rare = 4;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.AdamantiteBreastplate && legs.type == ItemID.AdamantiteLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Flower petals will fall on your target for extra damage";
            MyPlayer.ModPlayer(player).KiDamage += 0.09f;
            MyPlayer.ModPlayer(player).KiCrit += 6;
            MyPlayer.ModPlayer(player).KiMax += 100;
            player.onHitPetal = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}