using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DBZMOD.Util
{
    public static class AnimationHelper
    {
        public static readonly PlayerLayer TransformationEffects = new PlayerLayer("DBZMOD", "TransformationEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            Mod mod = DBZMOD.instance;
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            if (!modPlayer.IsTransformationAnimationPlaying)
            {
                modPlayer.TransformationFrameTimer = 0;
                return;
            }

            modPlayer.TransformationFrameTimer++;

            bool isAnyAnimationPlaying = false;
            // ssj 1 through 3. (forcibly exclude ssj3 and god form)
            if (Transformations.IsSSJ(drawPlayer) && !Transformations.IsGodlike(drawPlayer) && !Transformations.IsSSJ3(drawPlayer))
            {
                var frameCounterLimit = 4;
                var numberOfFrames = 4;
                var yOffset = -18;
                Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/SSJ", frameCounterLimit, numberOfFrames, yOffset));
                isAnyAnimationPlaying = modPlayer.IsTransformationAnimationPlaying;
            }
            if (Transformations.IsSSJ3(drawPlayer) && !Transformations.IsGodlike(drawPlayer))
            {
                var frameCounterLimit = 4;
                var numberOfFrames = 4;
                var yOffset = -18;
                Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/SSJ3", frameCounterLimit, numberOfFrames, yOffset));
                isAnyAnimationPlaying = modPlayer.IsTransformationAnimationPlaying;
            }
            if (Transformations.IsGodlike(drawPlayer))
            {
                var frameCounterLimit = 6;
                var numberOfFrames = 6;
                var yOffset = 35;
                Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/SSJG", frameCounterLimit, numberOfFrames, yOffset));
                isAnyAnimationPlaying = modPlayer.IsTransformationAnimationPlaying;
            }
            /*if (Transformations.IsLSSJ(drawPlayer))
            {
				Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/LSSJ", frameCounterLimit, numberOfFrames, yOffset));
				isAnyAnimationPlaying = modPlayer.IsTransformationAnimationPlaying;
            }*/
            if (Transformations.IsSpectrum(drawPlayer))
            {
                var frameCounterLimit = 4;
                var numberOfFrames = 7;
                var yOffset = -18;
                Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/SSJSpectrum", frameCounterLimit, numberOfFrames, yOffset));
                isAnyAnimationPlaying = modPlayer.IsTransformationAnimationPlaying;
            }

            // if we made it this far, we don't want to get stuck in a transformation animation state just because one doesn't exist
            // cancel it so we can move on and show auras.
            if (!isAnyAnimationPlaying)
            {
                modPlayer.IsTransformationAnimationPlaying = false;
            }
        });

        public static DrawData TransformationAnimationDrawData(PlayerDrawInfo drawInfo, string transformationSpriteSheet, int frameCounterLimit, int numberOfFrames, int yOffset)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = DBZMOD.instance;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            int frame = modPlayer.TransformationFrameTimer / frameCounterLimit;
            Texture2D texture = mod.GetTexture(transformationSpriteSheet);
            int frameSize = texture.Height / numberOfFrames;
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + frameSize + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            // we've hit the frame limit, so kill the animation
            if (frame == numberOfFrames)
            {
                modPlayer.IsTransformationAnimationPlaying = false;
            }
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
        }

        public static DrawData LightningEffectDrawData(PlayerDrawInfo drawInfo, string lightningTexture)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = DBZMOD.instance;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            int frame = modPlayer.LightningFrameTimer / 5;
            Texture2D texture = mod.GetTexture(lightningTexture);
            int frameSize = texture.Height / 3;
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
        }

        public static readonly PlayerLayer LightningEffects = new PlayerLayer("DBZMOD", "LightningEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Mod mod = DBZMOD.instance;
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.HasBuff(Transformations.SSJ2.GetBuffId()))
            {
                Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningBlue"));
            }
            if (drawPlayer.HasBuff(Transformations.LSSJ.GetBuffId()) || drawPlayer.HasBuff(Transformations.LSSJ2.GetBuffId()))
            {
                Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningGreen"));
            }
            if (drawPlayer.HasBuff(Transformations.SSJ3.GetBuffId()))
            {
                Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningYellow"));
            }
            if ((Transformations.IsKaioken(drawPlayer) && drawPlayer.GetModPlayer<MyPlayer>().KaiokenLevel == 5) || drawPlayer.HasBuff(mod.BuffType("SSJ4Buff")))
            {
                Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningRed"));
            }
        });


        public static readonly PlayerLayer DragonRadarEffects = new PlayerLayer("DBZMOD", "DragonRadarEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo) {

            Player drawPlayer = drawInfo.drawPlayer;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            Mod mod = DBZMOD.instance;
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Point closestLocation = new Point(-1, -1);
            float closestDistance = float.MaxValue;
            for (int i = 0; i < 7; i++)
            {
                var location = DBZWorld.GetWorld().DragonBallLocations[i];
                if (location == new Point(-1, -1))
                    continue;
                var coordVector = location.ToVector2() * 16f;
                var distance = Vector2.Distance(coordVector, drawPlayer.Center + Vector2.UnitY * -120f);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLocation = location;
                }
            }

            Vector2 radarAngleVector = Vector2.Normalize((drawPlayer.Center + Vector2.UnitY * -120f) - (closestLocation.ToVector2() * 16f));
            float radarAngle = radarAngleVector.ToRotation();

            // player is too close to the dragon ball.
            if (closestDistance < (modPlayer.IsHoldingDragonRadarMk1 ? 1280f : (modPlayer.IsHoldingDragonRadarMk2 ? 640f : 320f)))
            {
                radarAngle += (float)(Main.time % 59) * 6f;
            }
            radarAngle += MathHelper.ToRadians(radarAngle) - drawPlayer.fullRotation;
            var yOffset = -120;
            Main.playerDrawData.Add(DragonRadarDrawData(drawInfo, "Items/DragonBalls/DragonRadarPointer", yOffset, radarAngle - 1.57f, closestDistance, closestLocation.ToVector2() * 16f));

        });

        public static DrawData DragonRadarDrawData(PlayerDrawInfo drawInfo, string dragonRadarSprite, int yOffset, float angleInRadians, float distance, Vector2 location)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = DBZMOD.instance;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            float radarArrowScale = (modPlayer.IsHoldingDragonRadarMk1 ? 1f : (modPlayer.IsHoldingDragonRadarMk2 ? 1.25f : 1.5f));
            Texture2D texture = mod.GetTexture(dragonRadarSprite);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), radarArrowScale, SpriteEffects.None, 0);
        }
    }
}
