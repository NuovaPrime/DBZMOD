﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items
{
    public class AstralEssentia : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Essentia");
            Tooltip.SetDefault("The sky's astral energy emanates from within.");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 22;
            item.maxStack = 9999;
            item.value = 150;
            item.rare = 3;
        }
    }
	public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}