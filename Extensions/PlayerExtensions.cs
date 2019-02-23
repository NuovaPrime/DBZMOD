using System;
using System.Collections.Generic;
using System.Linq;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Enums;
using DBZMOD.Items.Consumables.Potions;
using DBZMOD.Items.DragonBalls;
using DBZMOD.Models;
using DBZMOD.Network;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Extensions
{
    /// <summary>
    ///     A class housing all the player/ModPlayer/MyPlayer based extensions
    /// </summary>
    public static class PlayerExtensions
    {
        /// <summary>
        ///     checks if the player has a vanilla item equipped in a non-vanity slot.
        /// </summary>
        /// <param name="player">The player being checked.</param>
        /// <param name="itemName">The name of the item to check for.</param>
        /// <returns></returns>
        public static bool IsAccessoryEquipped(this Player player, string itemName)
        {
            // switched to using an index so it's easier to detect vanity slots.
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].IsItemNamed(itemName))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Find a single ki potion (first found) and consume it.
        /// </summary>
        /// <param name="player"></param>
        public static void FindAndConsumeKiPotion(this Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item == null)
                    continue;
                if (item.modItem == null)
                    continue;
                if (item.modItem is KiPotion)
                {
                    KiPotion potion = (KiPotion)item.modItem;
                    potion.ConsumeItem(player);
                }
            }
        }

        /// <summary>
        ///     Return true if the player is carrying one of each dragon ball.
        /// </summary>
        /// <param name="player">The player being checked.</param>
        /// <returns></returns>
        public static bool IsCarryingAllDragonBalls(this Player player)
        {
            bool[] dragonBallsPresent = Enumerable.Repeat(false, 7).ToArray();
            for (int i = 0; i < dragonBallsPresent.Length; i++)
            {
                dragonBallsPresent[i] = player.inventory.IsDragonBallPresent(i + 1);
            }

            return dragonBallsPresent.All(x => x);
        }

        /// <summary>
        ///     Find and destroy exactly one of each dragon ball type in a player's inventory.
        ///     Called after making a wish.
        /// </summary>
        /// <param name="player">The player being checked.</param>
        public static void DestroyOneOfEachDragonBall(this Player player)
        {
            List<int> dragonBallTypeAlreadyRemoved = new List<int>();
            foreach (var item in player.inventory)
            {
                if (item == null)
                    continue;
                if (item.modItem == null)
                    continue;
                if (item.modItem is DragonBallItem)
                {
                    // only remove one of each type of dragon ball. If the player has extras, leave them. Lucky them.
                    if (dragonBallTypeAlreadyRemoved.Contains(item.type))
                        continue;
                    dragonBallTypeAlreadyRemoved.Add(item.type);
                    item.TurnToAir();
                }
            }
        }

        /// <summary>
        ///     Return the aura effect currently active on the player.
        /// </summary>
        /// <param name="modPlayer">The player being checked</param>
        public static AuraAnimationInfo GetAuraEffectOnPlayer(this MyPlayer modPlayer)
        {
            TransformationDefinition transformation = modPlayer.GetCurrentTransformation();
            if (modPlayer.player.dead)
                return null;

            if (transformation != null)
                return transformation.Appearance.auraAnimationInfo;

            if (modPlayer.isCharging)
                return AuraAnimations.createChargeAura;

            return null;
        }

        public static void ApplyChannelingSlowdown(this Player player)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            if (modPlayer.isFlying)
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                float yVelocity = -(player.gravity + 0.001f);
                if (modPlayer.isDownHeld || modPlayer.isUpHeld)
                {
                    yVelocity = player.velocity.Y / (1.2f - chargeMoveSpeedBonus);
                }
                else
                {
                    yVelocity = Math.Min(-0.4f, player.velocity.Y / (1.2f - chargeMoveSpeedBonus));
                }
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), yVelocity);
            }
            else
            {
                float chargeMoveSpeedBonus = modPlayer.chargeMoveSpeed / 10f;
                // don't neuter falling - keep the positive Y velocity if it's greater - if the player is jumping, this reduces their height. if falling, falling is always greater.                        
                player.velocity = new Vector2(player.velocity.X / (1.2f - chargeMoveSpeedBonus), Math.Max(player.velocity.Y, player.velocity.Y / (1.2f - chargeMoveSpeedBonus)));
            }
        }

        public static bool IsMassiveBlastInUse(this Player player)
        {            
            foreach(int massiveBlastType in ProjectileHelper.MassiveBlastProjectileTypes)
            {
                if (player.ownedProjectileCounts[massiveBlastType] > 0)
                    return true;
            }
            return false;
        }

        public static bool PlayerHasBuffIn(this Player player, TransformationDefinition[] buffs)
        {
            foreach (TransformationDefinition buff in buffs)
            {
                if (player.HasBuff(buff.GetBuffId()))
                    return true;
            }

            return false;
        }

        public static void AddKaiokenExhaustion(this Player player, int multiplier)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            player.AddBuff(DBZMOD.Instance.TransformationDefinitionManager.KaiokenFatigueDefinition.GetBuffId(), (int)Math.Ceiling(modPlayer.kaiokenTimer * multiplier));
            modPlayer.kaiokenTimer = 0f;
        }

        public static void AddTransformationExhaustion(this Player player)
        {
            player.AddBuff(DBZMOD.Instance.TransformationDefinitionManager.TransformationExhaustionDefinition.GetBuffId(), 600);
        }

        

        public static void DrawAura(this MyPlayer modPlayer, AuraAnimationInfo aura)
        {
            Player player = modPlayer.player;
            Texture2D texture = aura.GetTexture();
            Rectangle textureRectangle = new Rectangle(0, aura.GetHeight() * modPlayer.auraCurrentFrame, texture.Width, aura.GetHeight());
            float scale = aura.GetAuraScale(modPlayer);
            Tuple<float, Vector2> rotationAndPosition = aura.GetAuraRotationAndPosition(modPlayer);
            float rotation = rotationAndPosition.Item1;
            Vector2 position = rotationAndPosition.Item2;

            AnimationHelper.SetSpriteBatchForPlayerLayerCustomDraw(aura.blendState, player.GetPlayerSamplerState());

            // custom draw routine
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, textureRectangle, Color.White, rotation, new Vector2(aura.GetWidth(), aura.GetHeight()) * 0.5f, scale, SpriteEffects.None, 0f);

            AnimationHelper.ResetSpriteBatchForPlayerDrawLayers(player.GetPlayerSamplerState());
        }

        public static SamplerState GetPlayerSamplerState(this Player player)
        {
            return player.mount.Active ? Main.MountedSamplerState : Main.DefaultSamplerState;
        }

        public static void DrawKiAttackChargeBar(this MyPlayer modPlayer, Texture2D frameTexture, Texture2D fillTexture)
        {
            Player player = modPlayer.player;
            float quotient = modPlayer.currentKiAttackChargeLevel / modPlayer.currentKiAttackMaxChargeLevel;
            int drawX = (int)(player.position.X + player.width / 2f - Main.screenPosition.X);
            int drawY = (int)(player.position.Y + player.height + 4f - Main.screenPosition.Y);
            Vector2 frameDrawVector = new Vector2(drawX, drawY + 8);
            Rectangle frameRectangle = new Rectangle(2, 2, frameTexture.Width - 4, frameTexture.Height - 4);
            Vector2 fillDrawVector = new Vector2(drawX - 1, drawY + 10);
            int fillFactor = (int)Math.Ceiling(quotient * (fillTexture.Width - 4));
            Rectangle fillRectangle = new Rectangle(2, 2, fillFactor, fillTexture.Height - 4);

            AnimationHelper.SetSpriteBatchForPlayerLayerCustomDraw(BlendState.AlphaBlend, player.GetPlayerSamplerState());

            Main.spriteBatch.Draw(frameTexture, frameDrawVector, frameRectangle, Color.White, 0f, frameTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(fillTexture, fillDrawVector, fillRectangle, Color.White, 0f, fillTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

            AnimationHelper.ResetSpriteBatchForPlayerDrawLayers(player.GetPlayerSamplerState());
        }
    }
}