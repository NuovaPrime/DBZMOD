﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories
{
    public class MajinNucleus : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The pulsing nucleus of a invicible being." +
                "\nMassivly increased health regen" +
                "\nMassivly increased ki regen" +
                "\n-1500 max ki");
            DisplayName.SetDefault("Majin Nucleus");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 240000;
            item.rare = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).KiRegen += 6;
                player.lifeRegen += 12;
                player.GetModPlayer<MyPlayer>(mod).majinNucleus = true;
            }
        }
    }
}