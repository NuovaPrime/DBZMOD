using DBZMOD.Extensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_5
{
	public class Supernova : KiItem
	{
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("SupernovaBall");
            item.shootSpeed = 4f;
            item.damage = 140;
            item.knockBack = 12f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 250;
            item.useTime = 250;
            item.width = 40;
            item.channel = true;
            item.noUseGraphic = true;
            item.height = 40;
            item.autoReuse = false;
            item.value = 90000;
            item.rare = 7;
            kiDrain = 350;
            weaponType = "Massive Blast";
        }

	    public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Supernova");
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBomb", 1);
            recipe.AddIngredient(null, "PureKiCrystal", 50);
            recipe.AddIngredient(null, "DemonicSoul", 20);
            recipe.AddTile(null, "KaiTable");
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
            // DebugHelper.Log(string.Format("Player is trying to use {0} and Massive Blast In Use? {1}", DisplayName, inUse));
            return !inUse;
        }
    }
}
