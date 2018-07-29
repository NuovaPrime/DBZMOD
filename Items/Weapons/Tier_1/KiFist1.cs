using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_1
{
	public class KiFist1 : KiItem
	{
        private string LightPunchProjectile;
        private Player player;
        private int ChannelTimer;
		public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("KiFistProj");
			item.shootSpeed = 0f;
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
		}

        /*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "StableKiCrystal", 20);
            recipe.AddTile(null, "KiManipulator");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}*/
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanRightClick()
        {
            if(KiDrain <= MyPlayer.ModPlayer(player).KiCurrent && !player.HasBuff(mod.BuffType("HeavyPunchCooldown")) && CanUseHeavyHit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int BasicFistProjSelect()
        {
            switch (Main.rand.Next((4)))
            {
                case 0:
                    return mod.ProjectileType("KiFistProj1");
                case 1:
                    return mod.ProjectileType("KiFistProj2");
                case 2:
                    return mod.ProjectileType("KiFistProj3");
                case 3:
                    return mod.ProjectileType("KiFistProj4");
                default:
                    return 0;
                    
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = 3;
                #region HeavyPunchChecks
                if (!EyeDowned && !BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 20;
                    item.useTime = 40;
                    item.useAnimation = 40;
                    item.autoReuse = false;
                    KiDrain = 250;
                }
                if (EyeDowned && !BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 32;
                    item.useTime = 36;
                    item.useAnimation = 36;
                    item.autoReuse = false;
                    KiDrain = 250;
                }
                if (EyeDowned && BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 54;
                    item.useTime = 32;
                    item.useAnimation = 32;
                    KiDrain = 250;
                }
                if (EyeDowned && BeeDowned && WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 86;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.autoReuse = false;
                    KiDrain = 230;
                }
                if (EyeDowned && BeeDowned && WallDowned && PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 138;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.autoReuse = false;
                    KiDrain = 230;
                }
                if (EyeDowned && BeeDowned && WallDowned && PlantDowned && DukeDowned && !MoodlordDowned)
                {
                    item.damage = 164;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.autoReuse = false;
                    KiDrain = 230;
                }
                if (EyeDowned && BeeDowned && WallDowned && PlantDowned && DukeDowned && MoodlordDowned)
                {
                    item.damage = 242;
                    item.useTime = 28;
                    item.useAnimation = 28;
                    item.autoReuse = false;
                    KiDrain = 200;
                }

                #endregion
                item.shoot = mod.ProjectileType("KiFistProjHeavy");
                item.shootSpeed = 10f;
                player.AddBuff(mod.BuffType("HeavyPunchCooldown"), 300);
            }
            else
            {
                item.useStyle = 3;
                #region BasicPunchChecks
                if(!EyeDowned && !BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 10;
                    item.useTime = 4;
                    item.useAnimation = 4;
                    item.autoReuse = false;
                    item.shoot = BasicFistProjSelect();
                    KiDrain = 0;
                }
                if (EyeDowned && !BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 16;
                    item.useTime = 4;
                    item.useAnimation = 4;
                    item.autoReuse = false;
                    item.shoot = BasicFistProjSelect();
                    KiDrain = 0;
                }
                if (EyeDowned && BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 26;
                    item.useTime = 3;
                    item.useAnimation = 3;
                    item.autoReuse = false;
                    item.shoot = BasicFistProjSelect();
                    KiDrain = 0;
                }
                if (EyeDowned && BeeDowned && WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 45;
                    item.useTime = 3;
                    item.useAnimation = 3;
                    item.autoReuse = false;
                    item.shoot = BasicFistProjSelect();
                    KiDrain = 0;
                }
                if (EyeDowned && BeeDowned && WallDowned && PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 78;
                    item.useTime = 2;
                    item.useAnimation = 2;
                    item.autoReuse = false;
                    item.shoot = BasicFistProjSelect();
                    KiDrain = 0;
                }
                if (EyeDowned && BeeDowned && WallDowned && PlantDowned && DukeDowned && !MoodlordDowned)
                {
                    item.damage = 94;
                    item.useTime = 2;
                    item.useAnimation = 2;
                    item.autoReuse = false;
                    item.shoot = BasicFistProjSelect();
                    KiDrain = 0;
                }
                if (EyeDowned && BeeDowned && WallDowned && PlantDowned && DukeDowned && MoodlordDowned)
                {
                    item.damage = 164;
                    item.useTime = 1;
                    item.useAnimation = 1;
                    item.autoReuse = false;
                    item.shoot = BasicFistProjSelect();
                    KiDrain = 0;
                }

                #endregion
                
                #region FlurryChecks
                if (player.channel)
                {
                    ChannelTimer++;
                }
                if (ChannelTimer > 180 && !EyeDowned && !BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 7;
                    item.useTime = 1;
                    item.useAnimation = 1;
                    KiDrain = 0;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("KiFistFlurryProj");
                }
                if (ChannelTimer > 180 && EyeDowned && !BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 16;
                    item.useTime = 4;
                    item.useAnimation = 4;
                    KiDrain = 0;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("KiFistFlurryProj");
                }
                if (ChannelTimer > 180 && EyeDowned && BeeDowned && !WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 24;
                    item.useTime = 4;
                    item.useAnimation = 4;
                    KiDrain = 0;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("KiFistFlurryProj");
                }
                if (ChannelTimer > 180 && EyeDowned && BeeDowned && WallDowned && !PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 38;
                    item.useTime = 3;
                    item.useAnimation = 3;
                    KiDrain = 0;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("KiFistFlurryProj");
                }
                if (ChannelTimer > 180 && EyeDowned && BeeDowned && WallDowned && PlantDowned && !DukeDowned && !MoodlordDowned)
                {
                    item.damage = 57;
                    item.useTime = 2;
                    item.useAnimation = 2;
                    KiDrain = 0;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("KiFistFlurryProj");
                }
                if (ChannelTimer > 180 && EyeDowned && BeeDowned && WallDowned && PlantDowned && DukeDowned && !MoodlordDowned)
                {
                    item.damage = 75;
                    item.useTime = 2;
                    item.useAnimation = 2;
                    KiDrain = 0;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("KiFistFlurryProj");
                }
                if (ChannelTimer > 180 && EyeDowned && BeeDowned && WallDowned && PlantDowned && DukeDowned && MoodlordDowned)
                {
                    item.damage = 89;
                    item.useTime = 1;
                    item.useAnimation = 1;
                    KiDrain = 0;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("KiFistFlurryProj");
                }
                #endregion
            }
	return true;
        }
    }
}
