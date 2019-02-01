using DBZMOD.Extensions;
using DBZMOD.Players;
using DBZMOD.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_4
{
	public class SpiritBomb : KiItem
	{
		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shoot = mod.ProjectileType("SpiritBombBall");
            item.shootSpeed = 6f;
            item.damage = 80;
            item.knockBack = 12f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 300;
            item.useTime = 300;
            item.width = 40;
            item.channel = true;
            item.noUseGraphic = true;
            item.height = 40;
            item.autoReuse = false;
            item.value = 35000;
            item.rare = 4;
            kiDrain = 200;
            weaponType = "Massive Blast";
        }

	    public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Spirit Bomb");
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PridefulKiCrystal", 50);
            recipe.AddIngredient(null, "AngerKiCrystal", 50);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return true;
            var modPlayer = player.GetModPlayer<MyPlayer>();
            //var inUse = modPlayer.isMassiveBlastInUse;
            var inUse = player.IsMassiveBlastInUse();
            DebugHelper.Log(string.Format("Player is trying to use {0} and Massive Blast In Use? {1}", DisplayName, inUse));
            return !inUse;
        }
    }
}
