using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using DBZMOD.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics;
using Microsoft.Xna.Framework;
using DBZMOD.Projectiles;
using Terraria.ModLoader.IO;
using Terraria.ID;
using DBZMOD;
using DBZMOD.Util;
using DBZMOD.Enums;

namespace DBZMOD
{
    class FistSystem
    {
        #region Variables
        public bool EyeDowned;
        public bool BeeDowned;
        public bool WallDowned;
        public bool PlantDowned;
        public bool DukeDowned;
        public bool MoonlordDowned;
        private int BasicPunchDamage;
        private int HeavyPunchDamage;
        private int FlurryPunchDamage;
        private int FlurryActiveTimer;
        private int FlurryCooldownTimer;
        private int ShootSpeed;        
        private int ZanzokenCooldownTimer;
        private int ZanzokenHeavyInputTimer;
        private int ZanzokenHeavyCooldownTimer;
        private float ZanzokenKiCostMultiplier = 1f;
        private float ZanzokenDistanceMultiplier = 1f;
        private int LightAttackCooldownTimer;

        #endregion

        #region Constants
        // change how far the player can teleport.
        public const float ZANZOKEN_TRAVEL_DISTANCE = 200f;

        // change the number of frames you have to wait for zanzoken to refresh.
        public const int ZANZOKEN_COOLDOWN = 60;

        // change the base ki cost of zanzoken
        public const int ZANZOKEN_KI_COST = 400;

        // change the base limits for ki cost and distance
        public const float ZANZOKEN_KI_COST_MINIMUM = 1f;
        public const float ZANZOKEN_DISTANCE_MAXIMUM = 1f;

        // change how much the ki cost increases by each time you use zanzoken
        public const float ZANZOKEN_KI_COST_DELTA = 1.35f;

        // change how much distance is lost each time you use zanzoken
        public const float ZANZOKEN_DISTANCE_DELTA = 0.65f;

        // change how much ki cost is recovered from penalties each frame
        public const float ZANZOKEN_KI_COST_RECOVERY = 0.995f;

        // change how much distance is recovered from penalties each frame
        public const float ZANZOKEN_DISTANCE_RECOVERY = 1.005f;

        // change the amount of frames you have to execute a high speed heavy combo.
        public const int ZANZOKEN_HEAVY_TIMER = 15;

        // change the safe distance to teleport to an enemy by
        public const int ZANZOKEN_ENEMY_SAFE_DISTANCE = 16;

        // change the base duration of the flurry attack
        public const int FLURRY_ACTIVE_TIMER = 15;

        // change the base cooldown of the flurry attack
        public const int FLURRY_COOLDOWN_TIMER = 300;

        // change the base cooldown of the light attack
        public const int LIGHT_ATTACK_COOLDOWN = 5;        
        #endregion

        public void HandleZanzokenAndComboRecovery()
        {
            // PUT STUFF HERE FOR BONUSES TO ZANZOKEN DISTANCE AND KI COST, IF DESIRED.

            // take the greater of two numbers: the current ki cost multiplier after some decay, or the minimum ki cost multiplier.
            ZanzokenKiCostMultiplier = Math.Max(ZANZOKEN_KI_COST_MINIMUM, ZanzokenKiCostMultiplier * ZANZOKEN_KI_COST_RECOVERY);

            // take the lesser of two numbers: the current distance multiplier after some regrowth, or the maximum distance multiplier.
            ZanzokenDistanceMultiplier = Math.Min(ZANZOKEN_DISTANCE_MAXIMUM, ZanzokenDistanceMultiplier * ZANZOKEN_DISTANCE_RECOVERY);

            // PUT STUFF HERE FOR BONUSES TO COOLDOWNS (There's a few places to do this below also)

            // reduce the cooldown on Zanzoken
            ZanzokenCooldownTimer = Math.Max(0, ZanzokenCooldownTimer - 1);

            // also reduce the window for the Zanzoken Heavy combo
            ZanzokenHeavyInputTimer = Math.Max(0, ZanzokenHeavyInputTimer - 1);

            // also reduce the cooldown for the Zanzoken Heavy Combo
            ZanzokenHeavyCooldownTimer = Math.Max(0, ZanzokenHeavyCooldownTimer - 1);

            // also reduce the cooldown for flurries
            FlurryCooldownTimer = Math.Max(0, FlurryCooldownTimer - 1);

            // and a cooldown for light attacks.
            LightAttackCooldownTimer = Math.Max(0, LightAttackCooldownTimer - 1);
        }

        // return whether the player is in a fit state to use a zan heavy combo
        public bool CanUseZanzokenHeavy(Player player)
        {
            // Heavy input timer is a nonzero, meaning it's ticking down the frames they can use a heavy combo
            // Meanwhile the cooldown timer must be zero, meaning not on cooldown.
            return ZanzokenHeavyInputTimer > 0 && ZanzokenHeavyCooldownTimer == 0;
        }

        public bool CanPerformFlurry(Player player)
        {
            // Is the flurry on cooldown?
            return FlurryCooldownTimer == 0 && MyPlayer.ModPlayer(player).CanUseFlurry;
        }

        public bool CanPerformLightAttack(Player player)
        {
            return LightAttackCooldownTimer == 0;
        }

        public int GetFlurryDuration(Player player)
        {
            return FLURRY_ACTIVE_TIMER;
        }

        public int GetFlurryCooldownDuration(Player player)
        {
            return FLURRY_COOLDOWN_TIMER;
        }

        public Vector2 GetProjectileVelocity(Player player)
        {
            // as this function is occurring, make the player face the correct direction...
            Vector2 normalizedVector = Vector2.Normalize(Main.MouseWorld - player.Center);

            if (Math.Abs(normalizedVector.ToRotation()) < Math.PI / 2f)
            {
                player.direction = 1;
            } else
            {
                player.direction = -1;
            }

            return (normalizedVector * ShootSpeed);
        }

        public Vector2 GetProjectilePosition(Player player)
        {
            float randX = Main.rand.NextFloat(-4f, 4f);
            float randY = Main.rand.NextFloat(-4f, 4f);
            Vector2 randVector = new Vector2(randX, randY);
            return player.Center + randVector + Vector2.Normalize(Main.MouseWorld - player.) * 16f;
        }

        public int GetLightAttackCooldown(Player player)
        {
            // Get the use speed of the player's light attack from wherever or whatever determines it.
            var baseAttackSpeed = LIGHT_ATTACK_COOLDOWN;
            return baseAttackSpeed;
        }

        public void PerformLightAttack(Mod mod, Player player)
        {
            ShootSpeed = 7;
            Projectile.NewProjectile(GetProjectilePosition(player), GetProjectileVelocity(player), BasicFistProjSelect(mod), BasicPunchDamage, 5);
            LightAttackCooldownTimer = GetLightAttackCooldown(player);
        }

        public void Update(TriggersSet triggersSet, Player player, Mod mod)
        {
            HandleZanzokenAndComboRecovery();

            if (FlurryActiveTimer > 0)
            {
                if (FlurryActiveTimer % 2 == 0)
                {
                    // spawn flurry attack                    
                    Projectile.NewProjectile(GetProjectilePosition(player), GetProjectileVelocity(player), BasicFistProjSelect(mod), FlurryPunchDamage, 3);
                }
                FlurryActiveTimer--;

                // process no other triggers. 
                return;
            }

            // returns a list of actions to be performed based on trigger states.            
            var actionsToPerform = ControlHelper.ProcessInputs(triggersSet);

            #region Mouse Clicks
            if (actionsToPerform.BlockPhase1)//both click, for blocking
            {
                MyPlayer.ModPlayer(player).BlockState = 1;
            }
            else if (actionsToPerform.BlockPhase2)
            {
                MyPlayer.ModPlayer(player).BlockState = 2;
            }
            else if (actionsToPerform.BlockPhase3)
            {
                MyPlayer.ModPlayer(player).BlockState = 3;
            } else
            {
                MyPlayer.ModPlayer(player).BlockState = 0;
                if (actionsToPerform.Flurry && CanPerformFlurry(player))
                {
                    ShootSpeed = 2;
                    FlurryActiveTimer = GetFlurryDuration(player);
                    FlurryCooldownTimer = GetFlurryCooldownDuration(player);
                }
                else if (actionsToPerform.LightAttack && CanPerformLightAttack(player))
                {
                    PerformLightAttack(mod, player);
                }
                else if (actionsToPerform.HeavyAttack)
                {
                    if (!player.HasBuff(mod.BuffType("HeavyPunchCooldown")) && MyPlayer.ModPlayer(player).CanUseHeavyHit)
                    {
                        if (CanUseZanzokenHeavy(player))
                        {
                            // do zanzoken heavy attack combo
                        }
                        else
                        {
                            Projectile.NewProjectile(player.position, GetProjectileVelocity(player), mod.ProjectileType("KiFistProjHeavy"), HeavyPunchDamage, 50);
                        }
                    }
                }
            }
            #endregion

            #region Dash Checks
            if (actionsToPerform.DashUp)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Up);
            }
            if (actionsToPerform.DashDown)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Down);
            }
            if (actionsToPerform.DashLeft)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Left);
            }
            if (actionsToPerform.DashRight)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Right);
            }
            if (actionsToPerform.DashUpLeft)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Up, Controls.Left);
            }
            if (actionsToPerform.DashUpRight)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Up, Controls.Right);
            }
            if (actionsToPerform.DashDownLeft)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Down, Controls.Left);
            }
            if (actionsToPerform.DashDownRight)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(mod, player, Controls.Down, Controls.Right);
            }
            #endregion

            #region boss downed bools
            if (NPC.downedBoss1)
            {
                EyeDowned = true;
            }
            if (NPC.downedQueenBee)
            {
                BeeDowned = true;
            }
            if (Main.hardMode)
            {
                WallDowned = true;
            }
            if (NPC.downedPlantBoss)
            {
                PlantDowned = true;
            }
            if (NPC.downedFishron)
            {
                DukeDowned = true;
            }
            if (NPC.downedMoonlord)
            {
                MoonlordDowned = true;
            }
            #endregion

            #region Stat Checks
            BasicPunchDamage = 8;
            HeavyPunchDamage = BasicPunchDamage * 3;
            FlurryPunchDamage = BasicPunchDamage / 2;
            if (EyeDowned)
            {
                BasicPunchDamage += 6;
            }
            if (BeeDowned)
            {
                BasicPunchDamage += 8;
            }
            if (WallDowned)
            {
                BasicPunchDamage += 26;
            }
            if (PlantDowned)
            {
                BasicPunchDamage += 32;
            }
            if (DukeDowned)
            {
                BasicPunchDamage += 28;
            }
            if (MoonlordDowned)
            {
                BasicPunchDamage += 124;
            }

            #endregion

        }
        public int BasicFistProjSelect(Mod mod)
        {
            return mod.ProjectileType("KiFistProj");
            //switch (Main.rand.Next((4)))
            //{
            //    case 0:
            //        return mod.ProjectileType("KiFistProj1");
            //    case 1:
            //        return mod.ProjectileType("KiFistProj2");
            //    case 2:
            //        return mod.ProjectileType("KiFistProj3");
            //    case 3:
            //        return mod.ProjectileType("KiFistProj4");
            //    default:
            //        return 0;

            //}
        }


        public float GetZanzokenDistance()
        {
            
            // TODO
            // insert things that affect zanzoken distance here, like accessories or unlocked abilities.
            return ZANZOKEN_TRAVEL_DISTANCE;
        }

        // returns the vertical/horizontal Vector offsets of a 45 degree angle that travels ZANZOKEN_TRAVEL_DISTANCE.
        private float GetZanzokenDiagonalDistanceComponent()
        {
            var hypotenuse = GetZanzokenDistance();
            var componentDistance = (float)Math.Sqrt((hypotenuse * hypotenuse) / 2);
            return componentDistance;
        }

        private bool CanZanzoken(Player player)
        {
            return !player.frozen && !player.stoned && !player.HasBuff(BuffID.Cursed) && HasKiForZanzoken(player) && !IsZanzokenOnCooldown(player);
        }

        private bool HasKiForZanzoken(Player player)
        {
            return MyPlayer.ModPlayer(player).KiCurrent >= GetZanzokenKiCost(player);
        }

        private int GetZanzokenKiCost(Player player)
        {
            // PUT STUFF HERE TO IMPACT THE ZANZOKEN KI COST IF DESIRED.
            return (int)Math.Ceiling(ZANZOKEN_KI_COST * ZanzokenKiCostMultiplier);
        }

        private bool IsZanzokenOnCooldown(Player player)
        {
            // PUT STUFF HERE TO IMPACT THE ZANZOKEN COOLDOWN IF DESIRED.
            return ZanzokenCooldownTimer > 0;
        }

        private int GetZanzokenHeavyTimer(Player player)
        {
            // PUT STUFF HERE TO INCREASE THE WINDOW THE PLAYER CAN EXECUTE A HEAVY + ZAN COMBO
            return ZANZOKEN_HEAVY_TIMER;
        }

        private int GetZanzokenCooldownDuration(Player player)
        {
            // OR HERE
            return ZANZOKEN_COOLDOWN;
        }

        private void DeductKiForZanzoken(Player player)
        {
            MyPlayer.ModPlayer(player).KiCurrent -= GetZanzokenKiCost(player);
        }

        private Rectangle GetProjectedHitboxForSafeDistance(Vector2 vector, Player player)
        {
            return new Rectangle((int)vector.X - ZANZOKEN_ENEMY_SAFE_DISTANCE, 
                (int)vector.Y - ZANZOKEN_ENEMY_SAFE_DISTANCE, 
                player.width + (ZANZOKEN_ENEMY_SAFE_DISTANCE * 2), 
                player.height + (ZANZOKEN_ENEMY_SAFE_DISTANCE * 2));
        }

        private void ApplyZanzokenReusePenalties(Player player)
        {
            ZanzokenKiCostMultiplier *= ZANZOKEN_KI_COST_DELTA;
            ZanzokenDistanceMultiplier *= ZANZOKEN_DISTANCE_DELTA;
        }

        public void PerformZanzoken(Mod mod, Player player, params Controls[] directions)
        {
            // checks that would prevent you from using Zanzoken
            if (!CanZanzoken(player))
            {
                return;
            }

            DeductKiForZanzoken(player);

            // if the directions array contains more than one parameter, this is a diagonal zanzoken.
            float offset = GetZanzokenDistance();
            if (directions.Length > 1)
            {
                offset = GetZanzokenDiagonalDistanceComponent();
            }

            // get the player's origin
            Vector2 origin = new Vector2(player.position.X, player.position.Y);
            Vector2 originCenter = new Vector2(player.Center.X, player.Center.Y);

            // lazy switch to list so I can use Contains.
            List<Controls> directionList = new List<Controls>(directions);

            // figure out where the player is going to try and end up.
            float yOffset = (directionList.Contains(Controls.Up) ? -1 : (directionList.Contains(Controls.Down) ? 1 : 0)) * offset;
            float xOffset = (directionList.Contains(Controls.Left) ? -1 : (directionList.Contains(Controls.Right) ? 1 : 0)) * offset;

            // this is a lazy hack, do a pixel by pixel "scan" until the player hits something, check if its an enemy and then place them in an uncollided position
            float stepMaximum = Math.Max(Math.Abs(xOffset), Math.Abs(yOffset));
            float xStep = xOffset / stepMaximum;
            float yStep = yOffset / stepMaximum;
            Vector2 finalVelocity = new Vector2(0, 0);
            Vector2 stepVelocity = new Vector2(xStep, yStep);
            Vector2 newPosition = origin;
            Vector2 adaptiveOrigin = origin;

            // the enemy you would collide with during a zanzoken movement. Move to this enemy if it isn't null.
            NPC enemyZanzokenTarget = null;

            for (float f = 0f; f < stepMaximum; f += 1.0f)
            {
                float xPos = xStep * f;
                float yPos = yStep * f;
                finalVelocity = new Vector2(xPos, yPos);
                newPosition = adaptiveOrigin + finalVelocity;

                // do a tile collision check. if this returns anything other than new position, we have collision.
                bool isCollided = Collision.SolidCollision(newPosition, player.width, player.height);
                
                if (isCollided)
                {
                    // let's make the assumption we're running into a slope or halfblock. If moving us up by one causes us to be clear, we go there instead.
                    if (yPos == 0f)
                    {
                        newPosition.Y -= 16f;
                        adaptiveOrigin.Y -= 16f;
                    } else
                    {
                        newPosition = newPosition - finalVelocity;
                        break;
                    }
                    // we still collided. abandon all hope.
                    if (Collision.SolidCollision(newPosition, player.width, player.height))
                    {
                        newPosition = newPosition - finalVelocity;
                        break;
                    }
                }

                Rectangle playerProjectedHitbox = new Rectangle((int)newPosition.X, (int)newPosition.Y, player.width, player.height);
                                
                foreach(NPC npc in Main.npc)
                {
                    if (!npc.active || npc.friendly)
                        continue;
                    var npcRect = npc.getRect();
                    var playerRect = playerProjectedHitbox;
                    if (npcRect.Intersects(playerRect))
                    {
                        enemyZanzokenTarget = npc;
                        break;
                    }
                }

                // we found an enemy to run up to so we abort any future movement.
                if (enemyZanzokenTarget != null)
                {
                    break;
                }
            }

            // invert the velocity of the zanzoken until you're a few pixels away
            if (enemyZanzokenTarget != null)
            {

                Rectangle playerProjectedHitbox = GetProjectedHitboxForSafeDistance(newPosition, player);
                while(enemyZanzokenTarget.getRect().Intersects(playerProjectedHitbox))
                {
                    newPosition -= stepVelocity;
                    // refresh the hitbox with a backwards step
                    playerProjectedHitbox = GetProjectedHitboxForSafeDistance(newPosition, player);
                }
            }

            // tone down velocity until it isn't insane.
            bool isVelocityNormalized = false;
            while (!isVelocityNormalized)
            {                
                finalVelocity *= 0.9f;
                isVelocityNormalized = (finalVelocity.X * finalVelocity.X) + (finalVelocity.Y * finalVelocity.Y) <= 16f;
            }

            if (newPosition != origin)
            {
                // the player has moved. Spawn the visual and audio effects.                
                Projectile.NewProjectile(originCenter.X, originCenter.Y, finalVelocity.X, finalVelocity.Y, mod.ProjectileType("TransmissionLinesProj"), 0, 0, player.whoAmI);

                player.position = newPosition;
            }

            // if teleporting to an enemy, don't get velocity, or you'd probably run into them.
            if (enemyZanzokenTarget == null)
                player.velocity += finalVelocity;

            // Penalize repeated uses of Zanzoken in both distance and ki cost
            ApplyZanzokenReusePenalties(player);

            // enable the player to execute a zanzoken heavy attack
            ZanzokenHeavyInputTimer = GetZanzokenHeavyTimer(player);

            ZanzokenCooldownTimer = GetZanzokenCooldownDuration(player);
        }
    }
}
