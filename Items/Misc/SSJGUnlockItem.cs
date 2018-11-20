﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DBZMOD.Items.Misc
{
    public class SSJGUnlockItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Godly Spirit");
            ItemID.Sets.ItemNoGravity[item.type] = true;
	        Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 3));
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 20;
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
            Main.PlaySound(SoundID.NPCDeath7, player.position);
            MyPlayer.ModPlayer(player).SSJGTransformation();
            MyPlayer.ModPlayer(player).IsTransforming = true;
            MyPlayer.ModPlayer(player).SSJGAchieved = true;
            return false;
        }
			public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
 }