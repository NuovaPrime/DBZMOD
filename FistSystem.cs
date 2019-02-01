using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using DBZMOD.Util;
using DBZMOD.Enums;
using DBZMOD;

namespace DBZMOD
{
    class FistSystem
    {
        #region Variables
        public bool eyeDowned;
        public bool beeDowned;
        public bool wallDowned;
        public bool plantDowned;
        public bool dukeDowned;
        public bool moonlordDowned;
        private int _basicPunchDamage;
        private int _heavyPunchDamage;
        private int _flurryPunchDamage;
        private int _flurryActiveTimer;
        private int _flurryCooldownTimer;
        private int _shootSpeed;        
        private int _zanzokenCooldownTimer;
        private int _zanzokenHeavyInputTimer;
        private int _zanzokenHeavyCooldownTimer;
        private float _zanzokenKiCostMultiplier = 1f;
        private float _zanzokenDistanceMultiplier = 1f;
        private int _lightAttackCooldownTimer;

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
            _zanzokenKiCostMultiplier = Math.Max(ZANZOKEN_KI_COST_MINIMUM, _zanzokenKiCostMultiplier * ZANZOKEN_KI_COST_RECOVERY);

            // take the lesser of two numbers: the current distance multiplier after some regrowth, or the maximum distance multiplier.
            _zanzokenDistanceMultiplier = Math.Min(ZANZOKEN_DISTANCE_MAXIMUM, _zanzokenDistanceMultiplier * ZANZOKEN_DISTANCE_RECOVERY);

            // PUT STUFF HERE FOR BONUSES TO COOLDOWNS (There's a few places to do this below also)

            // reduce the cooldown on Zanzoken
            _zanzokenCooldownTimer = Math.Max(0, _zanzokenCooldownTimer - 1);

            // also reduce the window for the Zanzoken Heavy combo
            _zanzokenHeavyInputTimer = Math.Max(0, _zanzokenHeavyInputTimer - 1);

            // also reduce the cooldown for the Zanzoken Heavy Combo
            _zanzokenHeavyCooldownTimer = Math.Max(0, _zanzokenHeavyCooldownTimer - 1);

            // also reduce the cooldown for flurries
            _flurryCooldownTimer = Math.Max(0, _flurryCooldownTimer - 1);

            // and a cooldown for light attacks.
            _lightAttackCooldownTimer = Math.Max(0, _lightAttackCooldownTimer - 1);
        }

        // return whether the player is in a fit state to use a zan heavy combo
        public bool CanUseZanzokenHeavy(Player player)
        {
            // Heavy input timer is a nonzero, meaning it's ticking down the frames they can use a heavy combo
            // Meanwhile the cooldown timer must be zero, meaning not on cooldown.
            return _zanzokenHeavyInputTimer > 0 && _zanzokenHeavyCooldownTimer == 0;
        }

        public bool CanPerformFlurry(Player player)
        {
            // Is the flurry on cooldown?
            return _flurryCooldownTimer == 0 && MyPlayer.ModPlayer(player).canUseFlurry;
        }

        public bool CanPerformLightAttack(Player player)
        {
            return _lightAttackCooldownTimer == 0;
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

            return (normalizedVector * _shootSpeed);
        }

        public Vector2 GetProjectilePosition(Player player)
        {
            float randX = Main.rand.NextFloat(-4f, 4f);
            float randY = Main.rand.NextFloat(-4f, 4f);
            Vector2 randVector = new Vector2(randX, randY);
            return player.Center + randVector + Vector2.Normalize(Main.MouseWorld - player.Center) * 16f;
        }

        public int GetLightAttackCooldown(Player player)
        {
            // Get the use speed of the player's light attack from wherever or whatever determines it.
            var baseAttackSpeed = LIGHT_ATTACK_COOLDOWN;
            return baseAttackSpeed;
        }

        public void PerformLightAttack(Mod mod, Player player)
        {
            _shootSpeed = 7;
            Projectile.NewProjectile(GetProjectilePosition(player), GetProjectileVelocity(player), BasicFistProjSelect(mod), _basicPunchDamage, 5);
            _lightAttackCooldownTimer = GetLightAttackCooldown(player);
        }

        public void Update(TriggersSet triggersSet, Player player, Mod mod)
        {
            HandleZanzokenAndComboRecovery();

            if (_flurryActiveTimer > 0)
            {
                if (_flurryActiveTimer % 2 == 0)
                {
                    // spawn flurry attack                    
                    Projectile.NewProjectile(GetProjectilePosition(player), GetProjectileVelocity(player), BasicFistProjSelect(mod), _flurryPunchDamage, 3);
                }
                _flurryActiveTimer--;

                // process no other triggers. 
                return;
            }

            // returns a list of actions to be performed based on trigger states.            
            var actionsToPerform = ControlHelper.ProcessInputs(triggersSet);

            #region Mouse Clicks
            if (actionsToPerform.blockPhase1)//both click, for blocking
            {
                MyPlayer.ModPlayer(player).blockState = 1;
            }
            else if (actionsToPerform.blockPhase2)
            {
                MyPlayer.ModPlayer(player).blockState = 2;
            }
            else if (actionsToPerform.blockPhase3)
            {
                MyPlayer.ModPlayer(player).blockState = 3;
            } else
            {
                MyPlayer.ModPlayer(player).blockState = 0;
                if (actionsToPerform.flurry && CanPerformFlurry(player))
                {
                    _shootSpeed = 2;
                    _flurryActiveTimer = GetFlurryDuration(player);
                    _flurryCooldownTimer = GetFlurryCooldownDuration(player);
                }
                else if (actionsToPerform.lightAttack && CanPerformLightAttack(player))
                {
                    PerformLightAttack(mod, player);
                }
                else if (actionsToPerform.heavyAttack)
                {
                    if (!player.HasBuff(mod.BuffType("HeavyPunchCooldown")) && MyPlayer.ModPlayer(player).canUseHeavyHit)
                    {
                        if (CanUseZanzokenHeavy(player))
                        {
                            // do zanzoken heavy attack combo
                        }
                        else
                        {
                            Projectile.NewProjectile(player.position, GetProjectileVelocity(player), mod.ProjectileType("KiFistProjHeavy"), _heavyPunchDamage, 50);
                        }
                    }
                }
            }
            #endregion

            #region Dash Checks
            if (actionsToPerform.dashUp)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Up);
            }
            if (actionsToPerform.dashDown)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Down);
            }
            if (actionsToPerform.dashLeft)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Left);
            }
            if (actionsToPerform.dashRight)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Right);
            }
            if (actionsToPerform.dashUpLeft)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Up, Controls.Left);
            }
            if (actionsToPerform.dashUpRight)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Up, Controls.Right);
            }
            if (actionsToPerform.dashDownLeft)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Down, Controls.Left);
            }
            if (actionsToPerform.dashDownRight)
            {
                MyPlayer.ModPlayer(player).isDashing = true;
                PerformZanzoken(mod, player, Controls.Down, Controls.Right);
            }
            #endregion

            #region boss downed bools
            if (NPC.downedBoss1)
            {
                eyeDowned = true;
            }
            if (NPC.downedQueenBee)
            {
                beeDowned = true;
            }
            if (Main.hardMode)
            {
                wallDowned = true;
            }
            if (NPC.downedPlantBoss)
            {
                plantDowned = true;
            }
            if (NPC.downedFishron)
            {
                dukeDowned = true;
            }
            if (NPC.downedMoonlord)
            {
                moonlordDowned = true;
            }
            #endregion

            #region Stat Checks
            _basicPunchDamage = 8;
            _heavyPunchDamage = _basicPunchDamage * 3;
            _flurryPunchDamage = _basicPunchDamage / 2;
            if (eyeDowned)
            {
                _basicPunchDamage += 6;
            }
            if (beeDowned)
            {
                _basicPunchDamage += 8;
            }
            if (wallDowned)
            {
                _basicPunchDamage += 26;
            }
            if (plantDowned)
            {
                _basicPunchDamage += 32;
            }
            if (dukeDowned)
            {
                _basicPunchDamage += 28;
            }
            if (moonlordDowned)
            {
                _basicPunchDamage += 124;
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
            bool isImmobilized = MyPlayer.ModPlayer(player).IsPlayerImmobilized();
            return !isImmobilized && HasKiForZanzoken(player) && !IsZanzokenOnCooldown(player);
        }

        private bool HasKiForZanzoken(Player player)
        {
            return MyPlayer.ModPlayer(player).HasKi(GetZanzokenKiCost(player));
        }

        private int GetZanzokenKiCost(Player player)
        {
            // PUT STUFF HERE TO IMPACT THE ZANZOKEN KI COST IF DESIRED.
            return (int)Math.Ceiling(ZANZOKEN_KI_COST * _zanzokenKiCostMultiplier);
        }

        private bool IsZanzokenOnCooldown(Player player)
        {
            // PUT STUFF HERE TO IMPACT THE ZANZOKEN COOLDOWN IF DESIRED.
            return _zanzokenCooldownTimer > 0;
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
            MyPlayer.ModPlayer(player).AddKi(-GetZanzokenKiCost(player), false, false);
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
            _zanzokenKiCostMultiplier *= ZANZOKEN_KI_COST_DELTA;
            _zanzokenDistanceMultiplier *= ZANZOKEN_DISTANCE_DELTA;
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
            _zanzokenHeavyInputTimer = GetZanzokenHeavyTimer(player);

            _zanzokenCooldownTimer = GetZanzokenCooldownDuration(player);
        }
    }
}
