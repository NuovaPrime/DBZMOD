﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Util;

namespace DBZMOD.Projectiles
{
    public class SuperSpiritBombProjectile : KiProjectile
    {
        public override Color? GetAlpha(Color lightColor)
        {
		if (projectile.timeLeft < 85) 
			{
				byte b2 = (byte)(projectile.timeLeft * 3);
				byte a2 = (byte)(100f * ((float)b2 / 255f));
				return new Color((int)b2, (int)b2, (int)b2, (int)a2);
			}
            return new Color(100, 100, 100, 100);
        }
		
		public int localtimer = 0;
        public bool velocitySet = true;
        public bool init = true;
        public float angle = 0;
        public float angleIncrease = 0;
        public float delta = 0;
		public float width = 0;
		public float height = 0;
		
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Spirit Bomb Projectile");
		}
		
        public override void SetDefaults()
        {
            projectile.width = 226;
            projectile.height = 226;
			projectile.aiStyle = 1;
			projectile.light = 20f;
            projectile.friendly = true;
            projectile.netUpdate = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 500;
			aiType = 14;
			projectile.aiStyle = 1;
			projectile.tileCollide = false;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void AI()
        {
			
			projectile.ai[1]++;
            if(projectile.ai[1] % 7 == 0)		   
            Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
            Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);
            float sinDelta = MathHelper.ToRadians(delta);
            sinDelta = (0.25f * (float)Math.Sin(sinDelta)) + 0.75f;
            if (sinDelta < 0.5f)
            {
                sinDelta = 0.5f;
            }
            projectile.scale = sinDelta;
            delta += 1;
            for (int i = (int)projectile.position.X; i < projectile.width; i++)
            {
            {
                for (int j = (int)projectile.position.Y; j < projectile.height; j++)
                {
                    WorldGen.KillTile(i / 16, j / 16, false, false, true);
                }
            }
               int maxdusts = 20;
               for (int k = 0; k < maxdusts; k++)
            {
		     float dustDistance = 150 + Main.rand.Next(30);
             float dustSpeed = 10;
             Vector2 offset = Vector2.UnitX.RotateRandom(MathHelper.Pi) * dustDistance;
             Vector2 velocity = -offset.SafeNormalize(-Vector2.UnitY) * dustSpeed;
             Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 87, velocity, 0, default(Color), 1.5f);
		    }
        }
	}
   
        public override void Kill(int timeLeft)
        {
            int maxdusts = 20;
            SoundUtil.PlayCustomSound("Sounds/GroundRumble", projectile.Center);
              for (int i = 0; i < maxdusts; i++)
			{
				int num304 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 20, 0f, 0f, 100, default(Color), 0.8f);
				Main.dust[num304].noGravity = true;
				Main.dust[num304].velocity *= 1.2f;
				Main.dust[num304].velocity -= projectile.oldVelocity * 0.3f;
			}
        }
		
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;	
		}   
    }
}