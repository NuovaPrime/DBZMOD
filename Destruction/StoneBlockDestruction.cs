﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Destruction
{
    public class StoneBlockDestruction : ModProjectile
    {
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
			DisplayName.SetDefault("StoneBlockDestruction");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 16;
			projectile.alpha = 1;
			projectile.aiStyle = -1;
			projectile.damage = 0;
            projectile.ignoreWater = true;
     		 projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 250;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void AI()
        {
			projectile.velocity *= 1f;
			projectile.rotation = projectile.ai[1]; 
            projectile.localAI[0] += 1f;
            projectile.alpha = (int)projectile.localAI[0] * 2;
            
            if (projectile.localAI[0] > 130f)
                projectile.Kill();
			projectile.rotation = (float)Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);
			float sinDelta = MathHelper.ToRadians(delta);
            sinDelta = (0.25f * (float)Math.Sin(sinDelta)) + 0.75f;
            if (sinDelta < 0.5f)
            {
                sinDelta = 0.5f;
            }
            projectile.scale = sinDelta;
            delta += 2;
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