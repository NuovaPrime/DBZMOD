using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Util;

namespace DBZMOD
{
    public abstract class KiProjectile : ModProjectile
    {
        public static float DefaultBeamKnockback = 1.5f;
        public int ChargeLevel;
        public int ChargeTimer;
        public float ChargeTimerMax;
        public int ChargeLimit = 4;
        public int FinalChargeLimit = 4;
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
        public int DustType;

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override bool PreAI()
        {
            if (player == null)
                player = Main.player[projectile.owner];

            if (myPlayer == null && player != null)
                myPlayer = MyPlayer.ModPlayer(player);

            return base.PreAI();
        }

        public override void PostAI()
        {
            projectile.netUpdate = true;
            player = Main.player[projectile.owner];
            FinalChargeLimit = ChargeLimit + MyPlayer.ModPlayer(player).ChargeLimitAdd;
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
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
                }
                projectile.hide = true;

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

                // if the player is channeling, increment the timer and apply some slowdown
                if (player.channel && projectile.active)
                {
                    ChargeTimer++;
                    ProjectileUtil.ApplyChannelingSlowdown(player);
                }

                //ChargeTimerMax -= MyPlayer.ModPlayer(player).chargeTimerMaxAdd;

                if (ChargeTimer > ChargeTimerMax && ChargeLevel < FinalChargeLimit)
                {
                    ChargeLevel += 1;
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), ChargeLevel, false, false);
                    ChargeTimer = 0;
                }
                if (Main.time > 0 && Math.Ceiling(Main.time % 2) == 0 && !MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    MyPlayer.ModPlayer(player).AddKi(-KiDrainRate);
                }
                for (int d = 0; d < 4; d++)
                {
                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.position + (position * (10 + 2.0f * projectile.scale)), projectile.width, projectile.height, DustType, 0f, 0f, 213, default(Color), ballscale);
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

            if (Main.expertMode)
            {
                if ((target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) || (target.type >= NPCID.TheDestroyer && target.type <= NPCID.TheDestroyerTail))
                {
                    damage /= 2;
                }
            }
        }

        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            int item = 0;
            if (KiWeapon)
            {
                if (npc.life <= 0)
                {
                    if (npc.boss && MyPlayer.ModPlayer(player).RageCurrent >= 1)
                    {
                        MyPlayer.ModPlayer(player).RageCurrent -= 1;
                    }
                    if (Main.rand.Next(MyPlayer.ModPlayer(player).KiOrbDropChance) == 0)
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
                    if (Main.rand.Next(10) == 0)
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
                if (MyPlayer.ModPlayer(player).luminousSectum)
                {
                    if (Main.rand.Next(8) == 0)
                    {
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 20, mod.ProjectileType("KiSpark"),
                            damage / 2, 3, player.whoAmI);
                    }
                }
            }
            if (MyPlayer.ModPlayer(player).infuserSapphire)
            {
                npc.AddBuff(BuffID.Frostburn, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserRuby)
            {
                npc.AddBuff(BuffID.OnFire, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserEmerald)
            {
                npc.AddBuff(BuffID.Poisoned, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserDiamond)
            {
                npc.AddBuff(BuffID.Confused, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserAmethyst)
            {
                npc.AddBuff(BuffID.ShadowFlame, 300);
            }
            if (MyPlayer.ModPlayer(player).infuserTopaz)
            {
                npc.AddBuff(BuffID.OnFire, 180);
            }
            if (MyPlayer.ModPlayer(player).infuserAmber)
            {
                npc.AddBuff(BuffID.Ichor, 300);
            }
            if (MyPlayer.ModPlayer(player).infuserRainbow)
            {
                npc.AddBuff(BuffID.Ichor, 300);
                npc.AddBuff(BuffID.OnFire, 180);
                npc.AddBuff(BuffID.ShadowFlame, 300);
                npc.AddBuff(BuffID.Confused, 180);
                npc.AddBuff(BuffID.Poisoned, 180);
                npc.AddBuff(BuffID.Bleeding, 180);
                npc.AddBuff(BuffID.Frostburn, 180);

            }
        }
    }
}
