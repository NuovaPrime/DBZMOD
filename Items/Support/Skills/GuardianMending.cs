using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Support.Skills
{
	public class GuardianMending : SupportItem
	{
		public override void SetDefaults()
		{
            item.shoot = mod.ProjectileType("GuardianMendingProj");
            item.shootSpeed = 0f;
            item.damage = 0;
			item.knockBack = 0f;
			item.useStyle = 5;
			item.useAnimation = 2;
			item.useTime = 2;
			item.width = 20;
			item.noUseGraphic = true;
			item.height = 20;
			/*if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Kiblast1").WithPitchVariance(.3f);
            }*/
			item.autoReuse = false;
			item.value = 500;
			item.rare = 4;
            kiDrain = 100;
			weaponType = "Healing";
            item.channel = true;
	    }
	    public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Guardian Mending");
            Tooltip.SetDefault("'The ancient healing technique of the dragon clan.'\nUses ki to restore the life of players directly in front of you");
		}

        public override bool CanUseItem(Player player)
        {
            if(player.ownedProjectileCounts[mod.ProjectileType("GuardianMendingProj")] > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 25);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}*/
    }
}
