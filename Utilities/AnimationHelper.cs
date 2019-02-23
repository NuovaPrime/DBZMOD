using DBZMOD.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Utilities
{
    public static class AnimationHelper
    {
        public static void SetSpriteBatchForPlayerLayerCustomDraw(BlendState blendState, SamplerState samplerState)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, blendState, samplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void ResetSpriteBatchForPlayerDrawLayers(SamplerState samplerState)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static readonly PlayerLayer auraEffect = new PlayerLayer("DBZMOD", "AuraEffects", null, delegate (PlayerDrawInfo drawInfo)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            var player = drawInfo.drawPlayer;
            var modPlayer = player.GetModPlayer<MyPlayer>();

            Mod mod = DBZMOD.Instance;
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            var aura = modPlayer.GetAuraEffectOnPlayer();

            if (aura != null)
            {
                // we don't do player draw data, we do a custom draw.                
                modPlayer.DrawAura(aura);
            }
        });

        // responsible for handling the custom draw routine of the charge bar during player charge attacks
        private const string CHARGE_BAR_FRAME_TEXTURE = "UI/ChargeBarFrame";
        private const string CHARGE_BAR_TEXTURE = "UI/ChargeBar";

        public static readonly PlayerLayer kiChargeAttackEffects = new PlayerLayer("DBZMOD", "KiChargeAttackEffects", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            // ki charge bar effects only show up for the player using the attack.
            if (drawInfo.drawPlayer.whoAmI != Main.myPlayer)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();

            // abort if for some reason the max charge level is 0, this would be a divide by zero error anyway.
            if (modPlayer.currentKiAttackMaxChargeLevel == 0f)
                return;

            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Mod mod = DBZMOD.Instance;
            modPlayer.DrawKiAttackChargeBar(mod.GetTexture(CHARGE_BAR_FRAME_TEXTURE), mod.GetTexture(CHARGE_BAR_TEXTURE));
        });

        public static readonly PlayerLayer transformationEffects = new PlayerLayer("DBZMOD", "TransformationEffects", null, delegate (PlayerDrawInfo drawInfo)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();

            Mod mod = DBZMOD.Instance;
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            if (!modPlayer.isTransformationAnimationPlaying)
                return;

            bool isAnyAnimationPlaying = false;
            // ssj 1 through 3. (forcibly exclude ssj3 and god form)
            if (modPlayer.IsPlayerTransformation(modPlayer.TransformationDefinitionManager.SSJ1Definition) && 
                !modPlayer.IsPlayerTransformation(modPlayer.TransformationDefinitionManager.SSJGDefinition) && 
                !modPlayer.IsPlayerTransformation(modPlayer.TransformationDefinitionManager.SSJ3Definition))
            {
                var frameCounterLimit = 4;
                var numberOfFrames = 4;
                var yOffset = -18;
                Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/SSJ", frameCounterLimit, numberOfFrames, yOffset));
                isAnyAnimationPlaying = modPlayer.isTransformationAnimationPlaying;
            }
            if (modPlayer.IsPlayerTransformation(modPlayer.TransformationDefinitionManager.SSJ3Definition) && !modPlayer.IsPlayerTransformation(modPlayer.TransformationDefinitionManager.SSJGDefinition))
            {
                var frameCounterLimit = 4;
                var numberOfFrames = 4;
                var yOffset = -18;
                Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/SSJ3", frameCounterLimit, numberOfFrames, yOffset));
                isAnyAnimationPlaying = modPlayer.isTransformationAnimationPlaying;
            }
            if (modPlayer.IsPlayerTransformation(modPlayer.TransformationDefinitionManager.SSJGDefinition))
            {
                var frameCounterLimit = 6;
                var numberOfFrames = 6;
                var yOffset = 35;
                Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/SSJG", frameCounterLimit, numberOfFrames, yOffset));
                isAnyAnimationPlaying = modPlayer.isTransformationAnimationPlaying;
            }
            /*if (Transformations.IsLSSJ(drawPlayer))
            {
				Main.playerDrawData.Add(TransformationAnimationDrawData(drawInfo, "Effects/Animations/Transform/LSSJ", frameCounterLimit, numberOfFrames, yOffset));
				isAnyAnimationPlaying = modPlayer.IsTransformationAnimationPlaying;
            }*/

            // if we made it this far, we don't want to get stuck in a transformation animation state just because one doesn't exist
            // cancel it so we can move on and show auras.
            if (!isAnyAnimationPlaying)
            {
                modPlayer.isTransformationAnimationPlaying = false;
            }
        });

        public static DrawData TransformationAnimationDrawData(PlayerDrawInfo drawInfo, string transformationSpriteSheet, int frameCounterLimit, int numberOfFrames, int yOffset)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = DBZMOD.Instance;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            int frame = modPlayer.transformationFrameTimer / frameCounterLimit;
            Texture2D texture = mod.GetTexture(transformationSpriteSheet);
            int frameSize = texture.Height / numberOfFrames;
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + frameSize + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            // we've hit the frame limit, so kill the animation
            if (frame == numberOfFrames)
            {
                modPlayer.isTransformationAnimationPlaying = false;
            }
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
        }

        public static DrawData LightningEffectDrawData(PlayerDrawInfo drawInfo, string lightningTexture)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = DBZMOD.Instance;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            int frame = modPlayer.lightningFrameTimer / 5;
            Texture2D texture = mod.GetTexture(lightningTexture);
            int frameSize = texture.Height / 3;
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
        }

        public static readonly PlayerLayer lightningEffects = new PlayerLayer("DBZMOD", "LightningEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            Mod mod = DBZMOD.Instance;
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.HasBuff(DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition.GetBuffId()))
            {
                Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningBlue"));
            }
            if (drawPlayer.HasBuff(DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition.GetBuffId()))
            {
                Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningYellow"));
            }

            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (modPlayer.TransformationDefinitionManager.IsKaioken(modPlayer.GetCurrentTransformation()) && drawPlayer.GetModPlayer<MyPlayer>().kaiokenLevel == 5)
            {
                Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningRed"));
            }
        });

        public static readonly PlayerLayer dragonRadarEffects = new PlayerLayer("DBZMOD", "DragonRadarEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            // ragon radar effects only show up for the player holding it.
            if (drawInfo.drawPlayer.whoAmI != Main.myPlayer)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            Mod mod = DBZMOD.Instance;
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Point closestLocation = Point.Zero;
            float closestDistance = float.MaxValue;
            for (int i = 1; i <= 7; i++)
            {
                var location = DBZWorld.GetWorld().GetCachedDragonBallLocation(i);
                if (location.Equals(Point.Zero))
                    continue;
                // skip this dragon ball if the player is holding a copy
                if (Main.LocalPlayer.inventory.IsDragonBallPresent(i))
                    continue;
                var coordVector = location.ToVector2() * 16f;
                var distance = Vector2.Distance(coordVector, drawPlayer.Center + Vector2.UnitY * -120f);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLocation = location;
                }
            }

            if (closestLocation.Equals(Point.Zero))
            {
                // not a valid location, abort.
                return;
            }

            Vector2 radarAngleVector = Vector2.Normalize((drawPlayer.Center + Vector2.UnitY * -120f) - (closestLocation.ToVector2() * 16f));
            float radarAngle = radarAngleVector.ToRotation();

            // player is too close to the dragon ball.
            if (closestDistance < (modPlayer.isHoldingDragonRadarMk1 ? 1280f : (modPlayer.isHoldingDragonRadarMk2 ? 640f : 320f)))
            {
                radarAngle += (float)(DBZMOD.GetTicks() % 59) * 6f;
            }
            radarAngle += MathHelper.ToRadians(radarAngle) - drawPlayer.fullRotation;
            var yOffset = -120;
            Main.playerDrawData.Add(DragonRadarDrawData(drawInfo, "Items/DragonBalls/DragonRadarPointer", yOffset, radarAngle - 1.57f, closestDistance, closestLocation.ToVector2() * 16f));

        });

        public static DrawData DragonRadarDrawData(PlayerDrawInfo drawInfo, string dragonRadarSprite, int yOffset, float angleInRadians, float distance, Vector2 location)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = DBZMOD.Instance;
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>(mod);
            float radarArrowScale = (modPlayer.isHoldingDragonRadarMk1 ? 1f : (modPlayer.isHoldingDragonRadarMk2 ? 1.25f : 1.5f));
            Texture2D texture = mod.GetTexture(dragonRadarSprite);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), radarArrowScale, SpriteEffects.None, 0);
        }
    }
}
