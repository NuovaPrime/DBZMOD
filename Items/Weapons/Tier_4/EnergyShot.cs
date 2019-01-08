using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_4
{
	public class EnergyShot : KiItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Shot");
			Tooltip.SetDefault("An enhanced version of a regular ki blast");
		}
		public override void SetDefaults()
		{
			item.damage = 77;
			item.width = 40;
			item.height = 40;
			item.useTime = 14;
            item.shoot = mod.ProjectileType("EnergyShotBlast");
            item.shootSpeed = 20f;
			item.useAnimation = 14;
			item.useStyle = 5;
			item.knockBack = 6f;
			item.value = 35000;
			item.rare = 4;
            item.noUseGraphic = true;
			item.autoReuse = true;
            KiDrain = 105;
            WeaponType = "Blast";
            if (!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Kiblast1").WithPitchVariance(.3f);
            }
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "KiBlast");
            recipe.AddIngredient(ItemID.PixieDust, 18);
            recipe.AddIngredient(null, "AngerKiCrystal", 15);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
