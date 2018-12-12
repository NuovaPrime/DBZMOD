using DBZMOD;
using DBZMOD.Projectiles.Auras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace Projectiles.Auras
{
    public abstract class AuraProjectile : ModProjectile
    {
        public bool IsSSJAura;
        public bool IsKaioAura;
        public bool IsGodAura;
        public bool AuraActive;
        public Vector2 AuraOffset;
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

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            int frameCounterLimit = 3;

            // normal frame progression
            projectile.frameCounter++;

            // double the frame counter speed if charging
            if (modPlayer.IsCharging)
                projectile.frameCounter++;

            if (projectile.frameCounter > frameCounterLimit)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
            
            projectile.scale = 1.0f + ScaleExtra;


            // update handler to reorient the charge up aura after the aura offsets are defined.
            bool isPlayerMostlyStationary = Math.Abs(player.velocity.X) <= 6F && Math.Abs(player.velocity.Y) <= 6F;
            if (MyPlayer.ModPlayer(player).IsFlying && !isPlayerMostlyStationary)
            {
                double rotationOffset = player.fullRotation <= 0f ? (float)Math.PI : -(float)Math.PI;
                projectile.rotation = (float)(player.fullRotation + rotationOffset);

                // using the angle of attack, construct the cartesian offsets of the aura based on the height of both things
                double widthRadius = player.width / 4;
                double heightRadius = player.height / 4;
                double auraWidthRadius = projectile.width / 4;
                double auraHeightRadius = projectile.height / 4;

                // for right now, I'm just doing this with some hard coding. When we get more aura work done
                // we can try to unify this code a bit.
                bool isSSJ1Aura = projectile.modProjectile.GetType().IsAssignableFrom(typeof(SSJ1AuraProj));
                double forwardOffset =  isSSJ1Aura ? 32 : 24;
                double widthOffset = auraWidthRadius - (widthRadius + (AuraOffset.Y + forwardOffset));
                double heightOffset = auraHeightRadius - (heightRadius + (AuraOffset.Y + forwardOffset));
                double cartesianOffsetX = widthOffset * Math.Cos(player.fullRotation);
                double cartesianOffsetY = heightOffset * Math.Sin(player.fullRotation);

                Vector2 cartesianOffset = player.Center + new Vector2((float)-cartesianOffsetY, (float)cartesianOffsetX);

                // offset the aura
                projectile.Center = cartesianOffset;
            }
            else
            {
                projectile.Center = player.Center + new Vector2(AuraOffset.X, (AuraOffset.Y));
                projectile.rotation = 0;
            }
        }
    }
}
