using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Mounts.SpacePods
{
    public class SpacePodMount : ModMountData
    {
        public override void SetDefaults()
        {
            mountData.buff = mod.BuffType("NimbusBuff");
            mountData.heightBoost = 5;          //how high is the mount from the ground
            mountData.fallDamage = 0f;
            mountData.runSpeed = 4f;
            mountData.dashSpeed = 10f;
            mountData.flightTimeMax = 400;
            mountData.fatigueMax = 0;
            mountData.jumpHeight = 5;
            mountData.acceleration = 0.20f;
            mountData.jumpSpeed = 6f;
            mountData.blockExtraJumps = false;
            mountData.MinecartDirectional = true;
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
    }
}