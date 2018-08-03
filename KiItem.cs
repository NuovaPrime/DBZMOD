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

namespace DBZMOD
{
    public abstract class KiProjectile : ModProjectile
    {
        public int ChargeLevel;
        public int ChargeTimer;
        public int KiDrainTimer;
        public int SizeTimer;
        public int originalWidth;
        public int originalHeight;
        public bool ChargeBall;
        public bool KiWeapon = true;
        public bool BeamTrail;

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void PostAI()
        {
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
            if(KiWeapon)
            {
                if(npc.life < 0)
                {
                    if(Main.rand.Next(4) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KiOrb"), 1);
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
            projectile.Center = player.Center + new Vector2(0, AuraOffset);
            
            if (projectile.timeLeft < 2)
            {
                projectile.timeLeft = 10;
            }
            if(player.channel)
            {
                player.velocity = new Vector2(0, player.velocity.Y);
            }
            if(MyPlayer.ModPlayer(player).IsCharging)
            {
                projectile.scale = projectile.scale * 2;
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
            item.useTime = (int)(item.useTime / MyPlayer.ModPlayer(player).KiSpeedAddition);
            item.useAnimation = (int)(item.useAnimation / MyPlayer.ModPlayer(player).KiSpeedAddition);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine Indicate = new TooltipLine(mod, "", "");
            string[] Text = Indicate.text.Split(' ');
            Indicate.text = " Consumes " + KiDrain + " Ki ";
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
        public override bool CanUseItem(Player player)
        {
            int RealKiDrain = (int)(KiDrain * MyPlayer.ModPlayer(player).KiDrainMulti);
            if (RealKiDrain <= MyPlayer.ModPlayer(player).KiCurrent)
            {
                MyPlayer.ModPlayer(player).KiCurrent -= RealKiDrain;
                return true;
            }
            return false;
        }


    }
    public abstract class KiPotion : ModItem
    {
        public int KiHeal;
        public bool IsKiPotion;
        public override bool CloneNewInstances
        {
            get { return true; }
        }
        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).KiCurrent = MyPlayer.ModPlayer(player).KiCurrent + KiHeal;
            player.AddBuff(mod.BuffType("KiPotionCooldown"), 300);
            CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), KiHeal, false, false);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if(player.HasBuff(mod.BuffType("KiPotionCooldown")))
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
