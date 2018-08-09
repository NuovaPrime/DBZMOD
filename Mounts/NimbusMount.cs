using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
 
namespace DBZMOD.Mounts
{
    public class NimbusMount : ModMountData
    {
        public override void SetDefaults()
        {
            mountData.buff = mod.BuffType("NimbusBuff");
            mountData.heightBoost = 6;          //how high is the mount from the ground
            mountData.fallDamage = 0f;
            mountData.runSpeed = 4f;
            mountData.dashSpeed = 4f;
            mountData.flightTimeMax = 240;
            mountData.fatigueMax = 0;
            mountData.jumpHeight = 5;
            mountData.acceleration = 0.35f;
            mountData.jumpSpeed = 5f;
            mountData.blockExtraJumps = false;
            mountData.totalFrames = 1;            //mount frame/animation
            mountData.constantJump = true;
            int[] array = new int[mountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 1;
            }
            mountData.playerYOffsets = array;
            mountData.xOffset = -10;                    
            mountData.yOffset = 40;          //how high is the mount from the player
            mountData.bodyFrame = 3;          //player frame when it's on the mount
            mountData.playerHeadOffset = 10;        
            mountData.standingFrameCount = 1;
            mountData.standingFrameDelay = 1;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = 1;
            mountData.runningFrameDelay = 1;
            mountData.runningFrameStart = 0;
            mountData.flyingFrameCount = 1;
            mountData.flyingFrameDelay = 1;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 1;
            mountData.inAirFrameDelay = 1;
            mountData.inAirFrameStart = 0;
            mountData.idleFrameCount = 1;
            mountData.idleFrameDelay = 1;
            mountData.idleFrameStart = 0;
            mountData.idleFrameLoop = true;
            mountData.swimFrameCount = mountData.inAirFrameCount;
            mountData.swimFrameDelay = mountData.inAirFrameDelay;
            mountData.swimFrameStart = mountData.inAirFrameStart;
            if (Main.netMode != 2)
            {
                mountData.textureWidth = mountData.frontTexture.Width;
                mountData.textureHeight = mountData.frontTexture.Height;
            }
        }
 
        public override void UpdateEffects(Player player)
        {
            if (Math.Abs(player.velocity.X) > 4f)
            {
                Rectangle rect = player.getRect();
                Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, 10);
            }
        }
    }
}