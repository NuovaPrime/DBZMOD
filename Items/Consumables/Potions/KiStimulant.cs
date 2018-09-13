using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.Potions
{
	public class KiStimulant : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.consumable = true;
			item.maxStack = 99;
			item.UseSound = SoundID.Item1;
			item.useStyle = 2;
			item.useTurn = true;
			item.useAnimation = 17;
			item.useTime = 17;
			item.value = 2500;
			item.rare = 3;
			item.potion = false;
            item.buffType = mod.BuffType("KiStimulantBuff");
            item.buffTime = 7200;
		}
    
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ki Stimulant");
            Tooltip.SetDefault("Stimulates your body into enhancing ki.");
        }
       

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 5);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddIngredient(ItemID.Waterleaf, 5);
            recipe.AddIngredient(null, "CalmKiCrystal", 10);
            recipe.AddTile(TileID.Bottles);
            recipe.alchemy = true;
            recipe.SetResult(this, 3);
            recipe.AddRecipe();
        }
    }
}
