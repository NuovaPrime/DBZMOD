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
    public class BigBangKamehamehaBall : KiProjectile
    {
        public bool startingCharge = false;
        SoundEffectInstance chargeSound;

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = 1;
            projectile.light = 1f;
            projectile.timeLeft = 10;
            projectile.netUpdate = true;
            projectile.damage = 0;
            aiType = 14;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            ChargeBall = true;
            ChargeLimit = 10;
            KiDrainRate = 3;
            ballscale = 2f;
            color = Color.Blue;
            ChargeTimerMax = 80f;
            dusttype = 15;
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
            DisplayName.SetDefault("Big Bang Kamehameha Ball");
        }

        public override void AI()
        {

            if (!player.channel || (ChargeLevel >= ChargeLimit))
            {
                if (ChargeLevel >= 1)
                {
                    float rot = (float)Math.Atan2((Main.mouseY + Main.screenPosition.Y) - projectile.Center.Y, (Main.mouseX + Main.screenPosition.X) - projectile.Center.X);
                    Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2((float)((Math.Cos(rot) * 15)), (float)((Math.Sin(rot) * 15))), mod.ProjectileType("BigBangKamehamehaBlast"), projectile.damage + (ChargeLevel * 65), projectile.knockBack, projectile.owner);

                    //ChargeLevel = 0;
                    if (!Main.dedServ)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BasicBeamFire"));

                    projectile.Kill();

                    for (int i = 0; i < 100; i++)
                    {
                        float angle = Main.rand.NextFloat(360);
                        float angleRad = MathHelper.ToRadians(angle);
                        Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
                        Dust tDust = Dust.NewDustDirect(projectile.position + (position * (20 + 3.0f * projectile.scale)), projectile.width, projectile.height, dusttype, 0f, 0f, 213, default(Color), 3.0f);
                        tDust.velocity = -0.5f * Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                        tDust.noGravity = true;
                    }
                }

                if (chargeSound != null)
                {
                    chargeSound.Stop();
                }
            }

            if (!startingCharge)
            {
                startingCharge = true;
                if (!Main.dedServ)
                    chargeSound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyWaveCharge"));
            }

        }

    }
}