using System;
using System.Collections.Generic;
using System.Linq;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Enums;
using DBZMOD.Items.Consumables.Potions;
using DBZMOD.Items.DragonBalls;
using DBZMOD.Models;
using DBZMOD.Network;
using DBZMOD.Projectiles;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuffInfoExtensions = DBZMOD.Extensions.BuffInfoExtensions;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

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
            if (modPlayer.player.dead)
                return null;
            if (PlayerExtensions.IsKaioken(modPlayer.player))
                return AuraAnimations.createKaiokenAura;
            if (PlayerExtensions.IsSuperKaioken(modPlayer.player))
                return AuraAnimations.createSuperKaiokenAura;
            if (PlayerExtensions.IsSSJ1(modPlayer.player))
                return AuraAnimations.ssj1Aura;
            if (PlayerExtensions.IsAssj(modPlayer.player))
                return AuraAnimations.assjAura;
            if (PlayerExtensions.IsUssj(modPlayer.player))
                return AuraAnimations.ussjAura;
            if (PlayerExtensions.IsSSJ2(modPlayer.player))
                return AuraAnimations.ssj2Aura;
            if (PlayerExtensions.IsSSJ3(modPlayer.player))
                return AuraAnimations.ssj3Aura;
            if (PlayerExtensions.IsSSJG(modPlayer.player))
                return AuraAnimations.ssjgAura;
            if (PlayerExtensions.IsLSSJ1(modPlayer.player))
                return AuraAnimations.lssjAura;
            if (PlayerExtensions.IsLSSJ2(modPlayer.player))
                return AuraAnimations.lssj2Aura;
            if (PlayerExtensions.IsSpectrum(modPlayer.player))
                return AuraAnimations.spectrumAura;
            // handle charging last
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

        public static Projectile FindNearestOwnedProjectileOfType(this Player player, int type)
        {
            int closestProjectile = -1;
            float distance = Single.MaxValue;
            for(var i = 0; i < Main.projectile.Length; i++)
            {
                var proj = Main.projectile[i];

                // abort if the projectile is invalid, the player isn't the owner, the projectile is inactive or the type doesn't match what we want.
                if (proj == null || proj.owner != player.whoAmI || !proj.active || proj.type != type)
                    continue;               
                
                var projDistance = proj.Distance(player.Center);
                if (projDistance < distance)
                {
                    distance = projDistance;
                    closestProjectile = i;
                }
            }
            return closestProjectile == -1 ? null : Main.projectile[closestProjectile];
        }

        public static bool IsChargeBallRecaptured(this Player player, int type)
        {   
            // assume first that the player's already holding a proj
            if (player.heldProj != -1)
            {
                var heldProj = Main.projectile[player.heldProj];
                if (heldProj.modProjectile is AbstractChargeBall)
                {
                    return true;
                }
            }

            // otherwise try to recapture the held projectile if possible.
            var proj = player.FindNearestOwnedProjectileOfType(type);
            if (proj != null)
            {
                // the part that matters
                player.heldProj = proj.whoAmI;
                return true;
            }
            return false;
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

        public static bool IsPlayerTransformed(this Player player)
        {
            foreach (BuffInfo buff in FormBuffHelper.AllBuffs())
            {
                if (player.HasBuff(buff.GetBuffId()))
                    return true;
            }

            return false;
        }

        public static bool PlayerHasBuffIn(this Player player, BuffInfo[] buffs)
        {
            foreach (BuffInfo buff in buffs)
            {
                if (player.HasBuff(buff.GetBuffId()))
                    return true;
            }

            return false;
        }

        public static bool IsSSJ(this Player player)
        {
            return player.PlayerHasBuffIn(FormBuffHelper.ssjBuffs);
        }

        public static bool IsSSJ2(this Player player)
        {
            return player.HasBuff(FormBuffHelper.ssj2.GetBuffId());
        }

        public static bool IsSSJ3(this Player player)
        {
            return player.HasBuff(FormBuffHelper.ssj3.GetBuffId());
        }

        public static bool IsSpectrum(this Player player)
        {
            return player.HasBuff(FormBuffHelper.spectrum.GetBuffId());
        }

        public static bool IsKaioken(this Player player)
        {
            return player.HasBuff(FormBuffHelper.kaioken.GetBuffId());
        }

        public static bool IsAnyKaioken(this Player player)
        {
            return player.IsKaioken() || player.IsSuperKaioken();
        }

        public static bool IsDevBuffed(this Player player)
        {
            return player.IsSpectrum();
        }

        public static bool IsAnythingOtherThanKaioken(this Player player)
        {
            return player.IsLSSJ() || player.IsSSJ() || player.IsSSJG() || player.IsDevBuffed() || player.IsAscended() || player.IsSuperKaioken();
        }

        public static bool IsValidKaiokenForm(this Player player)
        {
            return player.IsSSJ1(); // || IsSSJB(player) || IsSSJR(player)
        }

        public static bool CanTransform(this Player player, MenuSelectionID menuId)
        {
            return CanTransform(player, FormBuffHelper.GetBuffFromMenuSelection(menuId));
        }

        public static bool IsAscended(this Player player)
        {
            return player.HasBuff(FormBuffHelper.assj.GetBuffId()) || player.HasBuff(FormBuffHelper.ussj.GetBuffId());
        }

        public static bool IsTransformBlocked(this Player player)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            return modPlayer.isTransforming || modPlayer.IsPlayerImmobilized() || modPlayer.IsKiDepleted();
        }

        public static bool CanTransform(this Player player, BuffInfo buff)
        {
            if (buff == null)
                return false;

            if (player.IsTransformBlocked())
                return false;
            
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            if (buff == FormBuffHelper.ssj1)
                return modPlayer.ssj1Achieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.ssj2)
                return !modPlayer.IsPlayerLegendary() && modPlayer.ssj2Achieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.ssj3)
                return !modPlayer.IsPlayerLegendary() && modPlayer.ssj3Achieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.ssjg)
                return !modPlayer.IsPlayerLegendary() && modPlayer.ssjgAchieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.lssj)
                return modPlayer.IsPlayerLegendary() && modPlayer.lssjAchieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.lssj2)
                return modPlayer.IsPlayerLegendary() && modPlayer.lssj2Achieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.assj)
                return (player.IsSSJ1() || player.IsUssj()) && modPlayer.assjAchieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.ussj)
                return player.IsAssj() && modPlayer.ussjAchieved && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.kaioken)
                return modPlayer.kaioAchieved && !player.IsTiredFromKaioken();
            if (buff == FormBuffHelper.superKaioken)
                return modPlayer.kaioAchieved && !player.IsTiredFromKaioken() && !player.IsExhaustedFromTransformation();
            if (buff == FormBuffHelper.spectrum)
                return player.name == "Nuova";
            return false;
        }

        public static void AddKaiokenExhaustion(this Player player, int multiplier)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            player.AddBuff(FormBuffHelper.kaiokenFatigue.GetBuffId(), (int)Math.Ceiling(modPlayer.kaiokenTimer * multiplier));
            modPlayer.kaiokenTimer = 0f;
        }

        public static void AddTransformationExhaustion(this Player player)
        {
            player.AddBuff(FormBuffHelper.transformationExhaustion.GetBuffId(), 600);
        }

        public static bool IsExhaustedFromTransformation(this Player player) { return player.HasBuff(FormBuffHelper.transformationExhaustion.GetBuffId()); }
        public static bool IsTiredFromKaioken(this Player player) { return player.HasBuff(FormBuffHelper.kaiokenFatigue.GetBuffId()); }

        public static void ClearAllTransformations(this Player player)
        {
            foreach (BuffInfo buff in FormBuffHelper.AllBuffs())
            {
                // don't clear buffs the player doesn't have, obviously.
                if (!player.HasBuff(buff.GetBuffId()))
                    continue;

                player.RemoveTransformation(buff.buffKeyName);
            }
        }

        public static void RemoveTransformation(this Player player, string buffKeyName)
        {
            BuffInfo buff = FormBuffHelper.GetBuffByKeyName(buffKeyName);

            player.ClearBuff(buff.GetBuffId());

            if (!Main.dedServ && Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
            {                
                NetworkHelper.formSync.SendFormChanges(256, player.whoAmI, player.whoAmI, buffKeyName, 0);                
            }
        }

        public static void DoTransform(this Player player, BuffInfo buff, Mod mod)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            
            // don't.. try to apply the same transformation. This just stacks projectile auras and looks dumb.
            if (buff == player.GetCurrentTransformation(true, false) || buff == player.GetCurrentTransformation(false, true))
                return;

            // make sure to swap kaioken with super kaioken when appropriate.
            if (buff == FormBuffHelper.superKaioken)
            {
                player.RemoveTransformation(FormBuffHelper.kaioken.buffKeyName);
            }

            // remove all *transformation* buffs from the player.
            // this needs to know we're powering down a step or not
            player.EndTransformations();

            // add whatever buff it is for a really long time.
            player.AddTransformation(buff.buffKeyName, FormBuffHelper.ABSURDLY_LONG_BUFF_DURATION);
        }

        public static void EndTransformations(this Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            // automatically applies debuffs.
            // skk qualifies as "non" kaioken.
            var currentBuff = player.GetCurrentTransformation(false, true);
            player.ClearAllTransformations();
            modPlayer.isTransformationAnimationPlaying = false;
            modPlayer.transformationFrameTimer = 0;
            
            modPlayer.isTransforming = false;
        }

        public static void AddTransformation(this Player player, string buffKeyName, int duration)
        {            
            BuffInfo buff = FormBuffHelper.GetBuffByKeyName(buffKeyName);
            player.AddBuff(buff.GetBuffId(), FormBuffHelper.ABSURDLY_LONG_BUFF_DURATION, false);

            if (!String.IsNullOrEmpty(buff.transformationText))
                CombatText.NewText(player.Hitbox, buff.transformationTextColor, buff.transformationText, false, false);

            if (!Main.dedServ && Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer) {
                NetworkHelper.formSync.SendFormChanges(256, player.whoAmI, player.whoAmI, buffKeyName, duration);
            }

            // start the transformation animation, if one exists. This auto cancels if nothing is there to play.
            player.GetModPlayer<MyPlayer>().isTransformationAnimationPlaying = true;
        }

        public static BuffInfo GetCurrentTransformation(this Player player, bool isIgnoringKaioken, bool isIgnoringNonKaioken)
        {
            foreach (BuffInfo buff in FormBuffHelper.AllBuffs())
            {
                if (BuffInfoExtensions.IsKaioken(buff) && isIgnoringKaioken)
                    continue;

                if (BuffInfoExtensions.IsAnythingOtherThanKaioken(buff) && isIgnoringNonKaioken)
                    continue;

                if (player.HasBuff(buff.GetBuffId()))
                {
                    return buff;
                }
            }

            // is the player transformed? Something bad may have happened.
            return null;
        }

        public static BuffInfo GetNextTransformationStep(this Player player)
        {
            BuffInfo currentTransformation = player.GetCurrentTransformation(false, false);
            BuffInfo currentNonKaioTransformation = player.GetCurrentTransformation(true, false);
            if (BuffInfoExtensions.IsKaioken(currentTransformation))
            {
                // player was in kaioken, trying to power up. Go to super kaioken but set the player's kaioken level to 1 because that's how things are now.
                if (currentNonKaioTransformation == null && player.GetModPlayer<MyPlayer>().hasSSJ1)
                {
                    player.GetModPlayer<MyPlayer>().kaiokenLevel = 1;
                    return FormBuffHelper.superKaioken;
                }

                // insert handler for SSJBK here

                // insert handler for SSJRK here
            }

            // SSJ1 is always the starting point if there isn't a current form tree to step through.
            if (currentTransformation == null)
                return FormBuffHelper.ssj1;

            // the player is legendary and doing a legendary step up.
            if (BuffInfoExtensions.IsLssj(currentTransformation) && MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < FormBuffHelper.legendaryBuffs.Length; i++)
                {
                    if (FormBuffHelper.legendaryBuffs[i] == currentTransformation && i < FormBuffHelper.legendaryBuffs.Length - 1)
                    {
                        return FormBuffHelper.legendaryBuffs[i + 1];
                    }
                }
            }

            // the player isn't legendary and is doing a normal step up.
            if (BuffInfoExtensions.IsSsj(currentTransformation) && !MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < FormBuffHelper.ssjBuffs.Length; i++)
                {
                    if (FormBuffHelper.ssjBuffs[i] == currentTransformation && i < FormBuffHelper.ssjBuffs.Length - 1)
                    {
                        return FormBuffHelper.ssjBuffs[i + 1];
                    }
                }
            }

            // whatever happened here, the function couldn't find a next step. Either the player is maxed in their steps, or something bad happened.
            return null;
        }

        public static BuffInfo GetPreviousTransformationStep(this Player player)
        {
            BuffInfo currentTransformation = player.GetCurrentTransformation(true, false);

            // the player is legendary and doing a legendary step down.
            if (currentTransformation.IsLssj() && MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < FormBuffHelper.legendaryBuffs.Length; i++)
                {
                    if (FormBuffHelper.legendaryBuffs[i] == currentTransformation && i > 0)
                    {
                        return FormBuffHelper.legendaryBuffs[i - 1];
                    }
                }
            }

            // the player isn't legendary and is doing a normal step down.
            if (currentTransformation.IsSsj() && !MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < FormBuffHelper.ssjBuffs.Length; i++)
                {
                    if (FormBuffHelper.ssjBuffs[i] == currentTransformation && i > 0)
                    {
                        return FormBuffHelper.ssjBuffs[i - 1];
                    }
                }
            }

            // figure out what the step down for ascension should be, if the player is in an ascended form.
            if (currentTransformation.IsAscended())
            {
                for (int i = 0; i < FormBuffHelper.ascensionBuffs.Length; i++)
                {
                    if (FormBuffHelper.ascensionBuffs[i] == currentTransformation && i > 0)
                    {
                        return FormBuffHelper.ascensionBuffs[i - 1];
                    }
                }
            }

            // either the player is at minimum or something bad has happened.
            return null;
        }

        public static BuffInfo GetNextAscensionStep(this Player player)
        {
            BuffInfo currentTransformation = player.GetCurrentTransformation(true, false);

            if (currentTransformation.IsAscended() || player.IsSSJ1())
            {
                for (int i = 0; i < FormBuffHelper.ascensionBuffs.Length; i++)
                {
                    if (FormBuffHelper.ascensionBuffs[i] == currentTransformation && i < FormBuffHelper.ascensionBuffs.Length - 1)
                    {
                        return FormBuffHelper.ascensionBuffs[i + 1];
                    }
                }
            }

            return currentTransformation;
        }

        public static bool IsSuperKaioken(this Player player)
        {
            return player.HasBuff(FormBuffHelper.superKaioken.GetBuffId());
        }

        public static bool IsLSSJ(this Player player)
        {
            return player.PlayerHasBuffIn(FormBuffHelper.legendaryBuffs);
        }

        public static bool IsLSSJ1(this Player player)
        {
            return player.HasBuff(FormBuffHelper.lssj.GetBuffId());
        }

        public static bool IsLSSJ2(this Player player)
        {
            return player.HasBuff(FormBuffHelper.lssj2.GetBuffId());
        }

        public static bool IsSSJG(this Player player)
        {
            return player.HasBuff(FormBuffHelper.ssjg.GetBuffId());
        }

        public static bool IsSSJ1(this Player player)
        {
            return player.HasBuff(FormBuffHelper.ssj1.GetBuffId());
        }

        public static bool IsAssj(this Player player)
        {
            return player.HasBuff(FormBuffHelper.assj.GetBuffId());
        }

        public static bool IsUssj(this Player player)
        {
            return player.HasBuff(FormBuffHelper.ussj.GetBuffId());
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

            SamplerState samplerState = Main.DefaultSamplerState;
            if (player.mount.Active)
            {
                samplerState = Main.MountedSamplerState;
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, aura.blendState, samplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // custom draw routine
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, textureRectangle, Color.White, rotation, new Vector2(aura.GetWidth(), aura.GetHeight()) * 0.5f, scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}