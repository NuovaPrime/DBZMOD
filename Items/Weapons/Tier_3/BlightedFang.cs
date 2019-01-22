using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Items.Weapons.Tier_3
{
    public class BlightedFang : KiItem
    {
        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("BlightedFangProj");
            item.shootSpeed = 16f;
            item.damage = 51;
            item.knockBack = 4f;
            item.useStyle = 5;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 22;
            item.useTime = 22;
            item.width = 40;
            item.noUseGraphic = true;
            item.height = 40;
            item.autoReuse = false;
            item.value = 8000;
            item.rare = 3;
            kiDrain = 75;
            weaponType = "Blast";
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Thief");
            Tooltip.SetDefault("Fires a Life-Stealing ki blast.");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("BlightedFangProj");
            int numberProjectiles = 2 + Main.rand.Next(2);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

    }
}
