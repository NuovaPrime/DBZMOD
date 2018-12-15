using DBZMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Items.Weapons.Tier_6
{
    public class BrolyMouthBeam : KiItem
    {
        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("BrolyMouthBeamProj");
            item.shootSpeed = 0f;
            item.damage = 200;
            item.knockBack = 2f;
            item.useStyle = 5;
            item.UseSound = SoundID.Item12;
            item.channel = true;
            item.useAnimation = 170;
            item.useTime = 170;
            item.width = 40;
            item.noUseGraphic = true;
            item.height = 40;
            item.autoReuse = false;
            item.value = 120000;
            item.rare = 8;
            KiDrain = 1;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("TEST ITEM");
            DisplayName.SetDefault("Broly Mouth Beam");
        }

        //public override void AddRecipes()
        //{
        //    ModRecipe recipe = new ModRecipe(mod);
        //    recipe.AddIngredient(ItemID.FragmentStardust, 18);
        //    recipe.AddIngredient(null, "Kamehamehax10");
        //    recipe.AddTile(null, "KaiTable");
        //    recipe.SetResult(this);
        //    recipe.AddRecipe();
        //}

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("BrolyMouthBeamProj")] > 1)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
