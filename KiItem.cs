﻿using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using DBZMOD;
using System;

namespace DBZMOD
{
    public abstract class KiProjectile : ModProjectile
    {
        public int ChargeLevel;
        public int ChargeTimer;
        public int ChargeLimit = 4;
        public int KiDrainTimer;
        public int SizeTimer;
        public int originalWidth;
        public int originalHeight;
        public bool ChargeBall;
        public bool KiWeapon = true;
        public bool BeamTrail;
        public int KiDrainRate;
        public Color color;
        public Player player;
        public MyPlayer myPlayer;
        public int yoffset;
        public int xoffset;
        public float ballscale = 1f;


        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override bool PreAI()
        {
            if(player == null)
                player = Main.player[projectile.owner];

            if(myPlayer == null && player != null)
                myPlayer = MyPlayer.ModPlayer(player);

            return base.PreAI();
        }

        public override void PostAI()
        {
            projectile.netUpdate = true;
            player = Main.player[projectile.owner];
            ChargeLimit += MyPlayer.ModPlayer(player).ChargeLimitAdd;
            if (!ChargeBall)
            {
                projectile.scale = projectile.scale + ChargeLevel;
            }

            if (BeamTrail && projectile.scale > 0 && SizeTimer > 0)
            {
                SizeTimer = 120;
                SizeTimer--;
                projectile.scale = (projectile.scale * SizeTimer / 120f);
            }
            if (ChargeBall)
            {
                if (MyPlayer.ModPlayer(player).KiCurrent <= 0)
                {
                    projectile.active = false;
                }

                if (projectile.timeLeft < 4)
                {
                    projectile.timeLeft = 10;
                }

                projectile.position.X = player.Center.X + (player.direction * 20 + xoffset) - 5;
                projectile.position.Y = player.Center.Y - 3 + yoffset;
                projectile.netUpdate2 = true;

                if (!player.channel && ChargeLevel < 1)
                {
                    projectile.Kill();
                }
                if (player.channel && projectile.active)
                {
                    ChargeTimer++;
                    KiDrainTimer++;
                    player.velocity = new Vector2(player.velocity.X / 3, player.velocity.Y);
                }
                if (KiDrainTimer > 1 && MyPlayer.ModPlayer(player).KiCurrent >= 0)
                {
                    MyPlayer.ModPlayer(player).KiCurrent -= KiDrainRate;
                    KiDrainTimer = 0;
                }
                for (int d = 0; d < 4; d++)
                {
                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.position + (position * (10 + 2.0f * projectile.scale)), projectile.width, projectile.height, 15, 0f, 0f, 213, color, ballscale);
                    tDust.velocity = Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                    tDust.noGravity = true;
                }
            }
        }

        public void SetScale(float scale)
        {
            projectile.scale = scale;
            projectile.width = (int)(originalWidth * scale);
            projectile.height = (int)(originalHeight * scale);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player owner = null;
            if (projectile.owner != -1)
                owner = Main.player[projectile.owner];
            else if (projectile.owner == 255)
                owner = Main.LocalPlayer;
        }
        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            int item = 0;
            if(KiWeapon)
            {
                if(npc.life < 0)
                {
                    if(Main.rand.Next(MyPlayer.ModPlayer(player).KiOrbDropChance) == 0)
                    {
                        item = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiOrb"), 1);
                    }
                    if (Main.netMode == 1 && item >= 0)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            if (KiWeapon)
            {
                if (MyPlayer.ModPlayer(player).palladiumBonus)
                {
                    if (Main.rand.Next(9) == 0)
                    {
                        item = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiOrb"), 1);
                    }
                    if (Main.netMode == 1 && item >= 0)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
        }
    }

    public abstract class AuraProjectile : ModProjectile
    {
        public bool IsSSJAura;
        public bool IsKaioAura;
        public bool IsGodAura;
        public bool AuraActive;
        public int AuraOffset;
        public float ScaleExtra;
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;
            
            if (projectile.timeLeft < 2)
            {
                projectile.timeLeft = 10;
            }
            if(player.channel)
            {
                player.velocity = new Vector2(0, player.velocity.Y);
            }            

            if (IsSSJAura)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter > 5)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }

            else if (IsKaioAura)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter > 5)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            if (MyPlayer.ModPlayer(player).IsFlying)
            {
                projectile.alpha = 255;
            }
            else
            {
                projectile.alpha = 50;
            }
            float chargingScale = 0.0f;
            float chargingAuraOffset = 0.0f;
            if (IsSSJAura || IsKaioAura)
            {
                if (MyPlayer.ModPlayer(player).IsCharging)
                {
                    chargingScale = 0.3f;
                    chargingAuraOffset = -18;
                }
                else
                {
                    chargingScale = 0.0f;
                    chargingAuraOffset = 0;
                }
            }

            projectile.scale = 1.0f + ScaleExtra + chargingScale;
            projectile.Center = player.Center + new Vector2(0, (AuraOffset + chargingAuraOffset));
        }

    }

    public abstract class KiItem : ModItem
    {
        private Player player;
        private NPC npc;
        public bool IsFistWeapon;
        public bool CanUseHeavyHit;
        #region Boss bool checks
        public bool EyeDowned;
        public bool BeeDowned;
        public bool WallDowned;
        public bool PlantDowned;
        public bool DukeDowned;
        public bool MoodlordDowned;
        public override void PostUpdate()
        {
            GetAttackSpeed(player);
            if(NPC.downedBoss1)
            {
                EyeDowned = true;
            }
            if(NPC.downedQueenBee)
            {
                BeeDowned = true;
            }
            if(Main.hardMode)
            {
                WallDowned = true;
            }
            if(NPC.downedPlantBoss)
            {
                PlantDowned = true;
            }
            if(NPC.downedFishron)
            {
                DukeDowned = true;
            }
            if(NPC.downedMoonlord)
            {
                MoodlordDowned = true;
            }
            if(item.channel)
            {
                item.autoReuse = false;
            }
        }
        #endregion
        #region FistAdditions 

        #endregion

        public override void SetDefaults()
        {
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.summon = false;
        }
        public float KiDrain;
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            knockback = knockback + MyPlayer.ModPlayer(player).KiKbAddition;
        }
        public override void GetWeaponDamage(Player player, ref int damage)
        {   
            damage = (int)(damage * MyPlayer.ModPlayer(player).KiDamage);
        }
        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = crit + MyPlayer.ModPlayer(player).KiCrit;
        }
        public void GetAttackSpeed(Player player)
        {
            item.useTime = (item.useTime - MyPlayer.ModPlayer(player).KiSpeedAddition);
            item.useAnimation = (item.useAnimation - MyPlayer.ModPlayer(player).KiSpeedAddition);
        }
        public int RealKiDrain(Player player)
        {
            return (int)(KiDrain * MyPlayer.ModPlayer(player).KiDrainMulti);
        }
        public override bool CanUseItem(Player player)
        {          
            if (RealKiDrain(Main.LocalPlayer) <= MyPlayer.ModPlayer(player).KiCurrent)
            {
                MyPlayer.ModPlayer(player).KiCurrent -= RealKiDrain(Main.LocalPlayer);
                return true;
            }
            return false;
        }
        public override void HoldItem(Player player)
        {
            MyPlayer.ModPlayer(player).IsHoldingKiWeapon = true;
            base.HoldItem(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine Indicate = new TooltipLine(mod, "", "");
            string[] Text = Indicate.text.Split(' ');
            Indicate.text = " Consumes " + RealKiDrain(Main.LocalPlayer) + " Ki ";

            tooltips.Add(Indicate);
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] splitText = tt.text.Split(' ');
                string damageValue = splitText.First();
                string damageWord = splitText.Last();
                tt.text = damageValue + " ki " + damageWord;
            }
            if (item.damage > 0)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.mod == "Terraria" && line.Name == "Tooltip")
                    {
                        line.overrideColor = Color.Cyan;
                    }
                }
            }
        }
    }
    public abstract class KiPotion : ModItem
    {
        public int KiHeal;
        public int potioncooldown = 3600;
        public bool IsKiPotion;
        public override bool CloneNewInstances
        {
            get { return true; }
        }
        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).KiCurrent += KiHeal;
            player.AddBuff(mod.BuffType("KiPotionSickness"), potioncooldown);
            CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), KiHeal, false, false);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if(player.HasBuff(mod.BuffType("KiPotionSickness")))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
