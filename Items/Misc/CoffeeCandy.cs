﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DBZMOD.Util;

namespace DBZMOD.Items.Misc
{
    public class CoffeeCandy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coffee Candy");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 8;
        }

        public override bool ItemSpace(Player player)
        {
            return true;
        }

        public override bool CanPickup(Player player)
        {
            return true;
        }

        public override bool OnPickup(Player player)
        {
            SoundUtil.PlayVanillaSound(SoundID.NPCDeath7, player);
            MyPlayer.ModPlayer(player).AddKi(25);
            player.statLife += 15;
            player.HealEffect(10);
            CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), 25, false, false);
            return false;
        }
			public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
 }