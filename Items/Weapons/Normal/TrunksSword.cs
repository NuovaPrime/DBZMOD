using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Items.Weapons.Normal
{
    public class TrunksSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Has some armour penetration."
        + "\n-Tier 4-");
            DisplayName.SetDefault("Trunk's Sword");
        }

        public override void SetDefaults()
        {
            item.damage = 74;
            item.melee = true;
            item.width = 74;
            item.height = 74;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.melee = true;
            item.knockBack = 17;
            item.value = 140000;
            item.rare = 4;
            if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SwordSlash").WithPitchVariance(.3f);
            }
            item.autoReuse = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.reuseDelay = 16;
        }

        public override void HoldItem(Player player)
        {
            player.armorPenetration += 50;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 20);
            recipe.AddIngredient(null, "AngerKiCrystal", 30);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
