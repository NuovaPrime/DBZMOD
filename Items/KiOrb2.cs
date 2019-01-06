﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DBZMOD.Util;

namespace DBZMOD.Items
{
    public class KiOrb2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Ki Orb");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 3));
            Lighting.AddLight(item.Center, 0.9f, 0.9f, 0.1f);
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 66;
        }

        public override bool ItemSpace(Player player)
        {
            return true;
        }

        public override bool CanPickup(Player player)
        {
            return true;
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange *= MyPlayer.ModPlayer(player).OrbGrabRange;
        }

        public override bool OnPickup(Player player)
        {
            SoundUtil.PlayVanillaSound(SoundID.NPCDeath7, player);
            MyPlayer.ModPlayer(player).AddKi(MyPlayer.ModPlayer(player).OrbHealAmount, false, false);
            CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), 50, false, false);
            return false;
        }
			public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
 }