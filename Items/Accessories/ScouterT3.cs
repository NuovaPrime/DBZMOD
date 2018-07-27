using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class ScouterT3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A Piece of equipment used for scanning powerlevels."
               + "\nGives Increased Ki Damage and Hunter effect."
               + "\n--Tier 3--");
            DisplayName.SetDefault("Blue Scouter");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 70000;
            item.rare = 4;
            item.accessory = true;
            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {

                player.GetModPlayer<MyPlayer>(mod).KiDamage *= 1.08f;
                player.GetModPlayer<MyPlayer>(mod).scouterT3 = true;
                player.detectCreature = true;
            }
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 20);
            recipe.AddIngredient(null, "ScouterT2");
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}