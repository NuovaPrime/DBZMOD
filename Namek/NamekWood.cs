using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Namek
{
    public class NamekWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Namekian Wood");
        }
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.value = 100;
            item.maxStack = 999;
            item.rare = 2;
        }
    }
}