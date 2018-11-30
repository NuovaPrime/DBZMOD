﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class LuminousSectum : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It radiates with unstable energy." +
                "\n9% increased ki damage" +
                "\n+250 max ki" +
                "\nHitting enemies has a small chance to fire off homing ki sparks.");
            DisplayName.SetDefault("Luminous Sectum");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 320000;
            item.rare = 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiDamage += 0.09f;
                player.GetModPlayer<MyPlayer>(mod).KiMax2 += 250;
                player.GetModPlayer<MyPlayer>(mod).luminousSectum = true;
            }
        }
    }
}