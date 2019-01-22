using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_5
{
    public class SuperEnergyBarrage : KiItem
    {
        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("SuperEnergyBarrageProj");
            item.shootSpeed = 36f;
            item.damage = 88;
            item.knockBack = 3f;
            item.useStyle = 5;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 15;
            item.useTime = 13;
            item.width = 50;
            item.noUseGraphic = true;
            item.height = 50;
			if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Kiblast2").WithPitchVariance(.2f);
            }
            item.autoReuse = true;
            item.value = 50000;
            item.rare = 7;
            kiDrain = 125;
            weaponType = "Barrage";
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Energy Barrage");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PureKiCrystal", 35);
            recipe.AddTile(null, "KaiTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
