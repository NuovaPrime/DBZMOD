using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class MythrilGlasses : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("11% Increased Ki Damage\n6% Increased Ki Crit Chance\nMaximum Ki increased by 100.");
            DisplayName.SetDefault("Mythril Glasses");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 16;
            item.value = 9000;
            item.rare = 4;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.MythrilChainmail && legs.type == ItemID.MythrilGreaves;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Ki orbs regenerate twice as much ki.";
            MyPlayer.ModPlayer(player).orbHealAmount *= 2;
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer.ModPlayer(player).kiDamage += 0.11f;
            MyPlayer.ModPlayer(player).kiCrit += 6;
            MyPlayer.ModPlayer(player).kiMax2 += 100;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MythrilBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}