using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;

namespace DBZMOD.Items.Weapons.Fist
{
	public class KiFist1 : KiItem
	{
        private string LightPunchProjectile;
        // private Player player; // conflicts with parent class public Player property, I think you can dump this, you inherit it from KiItem afaict
        private int ChannelTimer;
        private TriggersSet triggersSet;
        private string tooltip;
		public override void SetDefaults()
		{
			item.damage = 10;
			item.knockBack = 5f;
			item.useStyle = 3;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 1;
			item.width = 12;
			item.noUseGraphic = true;
			item.height = 12;
			item.autoReuse = false;
            item.channel = true;
			item.value = 0;
			item.rare = 1;
	    }
	    public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Fist");
            Tooltip.SetDefault(tooltip);
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if(MyPlayer.ModPlayer(player).CanUseFlurry)
            {
                tooltip = "The most essential skill of any martial artist." +
                "\nLeft click to punch, has no cooldown between attacks." +
                "\nHolding Left click allows you to do a flurry attack.";
            }
            if (MyPlayer.ModPlayer(player).CanUseHeavyHit)
            {
                tooltip = "The most essential skill of any martial artist." +
                "\nLeft click to punch, has no cooldown between attacks." +
                "\nHolding Left click allows you to do a flurry attack." +
                "\nRight click to do a ki infused punch, this sends anything flying.";
            }
            if (MyPlayer.ModPlayer(player).CanUseZanzoken)
            {
                tooltip = "The most essential skill of any martial artist." +
                "\nLeft click to punch, has no cooldown between attacks." +
                "\nHolding Left click allows you to do a flurry attack." +
                "\nRight click to do a ki infused punch, this sends anything flying." +
                "\nDouble tap in any direction to do a short ranged teleport, teleporting towards a enemy makes you teleport to them." +
                "\nHeavy hitting after teleporting to a enemy will send them flying even harder.";
            }
            else
            {
                tooltip = "The most essential skill of any martial artist." +
                "\nLeft click to punch, has no cooldown between attacks.";
            }
        }

        /*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 20);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}*/
        public override void HoldItem(Player player)
        {
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KiFistProj"), 0, 0, player.whoAmI);
        }
    }
}
