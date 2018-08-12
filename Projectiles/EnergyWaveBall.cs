using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace DBZMOD.Projectiles
{
	public class EnergyWaveBall : KiProjectile
	{
        private Player player;
        public bool startingCharge = false;
        SoundEffectInstance chargeSound;

		public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
			projectile.tileCollide = false;
            projectile.width = 10;
            projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.light = 1f;
            projectile.timeLeft = 10;
            projectile.netUpdate = true;
            projectile.damage = 0;
			aiType = 14;
            projectile.ignoreWater = true;
			projectile.penetrate = -1;
            ChargeBall = true;
            ChargeLimit = 4;
        }

		 public override Color? GetAlpha(Color lightColor)
        {
			//if (projectile.timeLeft < 85) 
			//{
			//	byte b2 = (byte)(projectile.timeLeft * 3);
			//	byte a2 = (byte)(100f * ((float)b2 / 255f));
			//	return new Color((int)b2, (int)b2, (int)b2, (int)a2);
			//}
			return new Color(255, 255, 255, 100);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energy Wave Ball");
        }
   
        public override void AI()
        {
            if(projectile.timeLeft < 4)
            {
                projectile.timeLeft = 10;
            }
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X + (player.direction * 20) - 5;
            projectile.position.Y = player.Center.Y - 3;

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

            if(ChargeTimer > 90)
            {
                ChargeLevel += 1;
                ChargeTimer = 0;
                projectile.scale += 0.4f;
            }

            for (int d = 0; d < 4; d++)
            {
                float angle = Main.rand.NextFloat(360);
                float angleRad = MathHelper.ToRadians(angle);
                Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                Dust tDust = Dust.NewDustDirect(projectile.position + (position * (20 + 3.0f * projectile.scale)), projectile.width, projectile.height, 15, 0f, 0f, 213, default(Color), 2.0f);
                tDust.velocity = Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                tDust.noGravity = true;
            }


            if ((!player.channel || ChargeLevel >= ChargeLimit) && ChargeLevel >= 1)
            {
                float rot = (float)Math.Atan2((Main.mouseY + Main.screenPosition.Y) - projectile.Center.Y, (Main.mouseX + Main.screenPosition.X) - projectile.Center.X);
                Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2((float)((Math.Cos(rot) * 10)), (float)((Math.Sin(rot) * 10))), mod.ProjectileType("EnergyWaveBlast"), projectile.damage + (ChargeLevel * 10), projectile.knockBack, projectile.owner);
                //shotProjectile.scale = projectile.scale;
                ChargeLevel = 0;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BasicBeamFire").WithVolume(2.0f));
                chargeSound.Stop();
                projectile.Kill();

                for(int i = 0; i < 100; i++)
                {
                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
                    Dust tDust = Dust.NewDustDirect(projectile.position + (position * (20 + 3.0f * projectile.scale)), projectile.width, projectile.height, 15, 0f, 0f, 213, default(Color), 2.0f);
                    tDust.velocity = -0.5f * Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                    tDust.noGravity = true;
                }

            }

            if (KiDrainTimer > 1 && MyPlayer.ModPlayer(player).KiCurrent >= 0)
            {
                MyPlayer.ModPlayer(player).KiCurrent -= 1;
                KiDrainTimer = 0;
            }

            if(!startingCharge)
            {
                startingCharge = true;
                chargeSound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyWaveCharge").WithVolume(3.0f));
            }

        }

	}
}