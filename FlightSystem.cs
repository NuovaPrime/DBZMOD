using System;
using DBZMOD.Extensions;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace DBZMOD
{
    public class FlightSystem
    {

        //constants
        const float FLIGHT_KI_DRAIN = 1f;
        const float BURST_SPEED = 0.3f;
        const float FLIGHT_SPEED = 0.1f;

        public static void Update(Player player)
        {
            // this might seem weird but the server isn't allowed to control the flight system.
            if (Main.netMode == NetmodeID.Server)
                return;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            //check for ki or death lol
            if ((modPlayer.IsKiDepleted() || player.dead || player.mount.Type != -1 || player.ropeCount != 0) && modPlayer.isFlying)
            {
                modPlayer.isFlying = false;
                AddKatchinFeetBuff(player);
            }

            if (modPlayer.isFlying)
            {
                // cancel platform collision
                player.DryCollision(true, true);

                //prepare vals
                player.fullRotationOrigin = new Vector2(11, 22);
                Vector2 mRotationDir = Vector2.Zero;

                int flightDustType = 261;

                //Input checks
                float boostSpeed = (BURST_SPEED) * (modPlayer.isCharging ? 1 : 0);
                
                // handle ki drain
                float totalFlightUsage = Math.Max(1f, FLIGHT_KI_DRAIN * modPlayer.flightKiConsumptionMultiplier);
                float flightCostMult = modPlayer.flightUpgraded ? 0.25f : (modPlayer.flightDampeningUnlocked ? 0.5f : 1f);
                totalFlightUsage *= flightCostMult;
                modPlayer.AddKi((totalFlightUsage * (1f + boostSpeed)) * -1, false, false);
                float flightSpeedMult = (1f + boostSpeed);
                flightSpeedMult *= modPlayer.flightUpgraded ? 1.25f : (modPlayer.flightDampeningUnlocked ? 1f : 0.75f);
                float flightSpeed = FLIGHT_SPEED * flightSpeedMult;
                                    
                float totalHorizontalFlightSpeed = flightSpeed + (player.moveSpeed / 3) + modPlayer.flightSpeedAdd;
                float totalVerticalFlightSpeed = flightSpeed + (Player.jumpSpeed / 2) + modPlayer.flightSpeedAdd;

                if (modPlayer.isUpHeld)
                {
                    // for some reason flying up is way, way faster than flying down.
                    player.velocity.Y -= (totalVerticalFlightSpeed / 3.8f);
                    mRotationDir = Vector2.UnitY;
                }
                else if (modPlayer.isDownHeld)
                {
                    player.maxFallSpeed = 20f;
                    player.velocity.Y += totalVerticalFlightSpeed / 3.6f;
                    mRotationDir = -Vector2.UnitY;
                }

                if (modPlayer.isRightHeld)
                {
                    player.velocity.X += totalHorizontalFlightSpeed;
                    mRotationDir += Vector2.UnitX;
                }
                else if (modPlayer.isLeftHeld)
                {
                    player.velocity.X -= totalHorizontalFlightSpeed;
                    mRotationDir -= Vector2.UnitX;
                }

                // TODO Add in TransformationAppearance class.
                if (modPlayer.IsTransformedInto(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition) && !modPlayer.IsTransformedInto(DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition))
                {
                    flightDustType = 170;
                }
                else if (modPlayer.IsTransformedInto(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition))
                {
                    flightDustType = 107;
                }
                else if (modPlayer.IsTransformedInto(DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition))
                {
                    flightDustType = 174;
                }
                else if (DBZMOD.Instance.TransformationDefinitionManager.IsKaioken(modPlayer.ActiveTransformations))
                {
                    flightDustType = 182;
                }
                else
                {
                    flightDustType = 267;
                }

                if (player.velocity.Length() > 0.5f)
                {
                    SpawnFlightDust(player, boostSpeed, flightDustType, 0f);
                }

                //calculate velocity
                player.velocity.X = MathHelper.Lerp(player.velocity.X, 0, 0.1f);
                player.velocity.Y = MathHelper.Lerp(player.velocity.Y, 0, 0.1f);
                // keep the player suspended at worst.
                player.velocity = player.velocity - (player.gravity * Vector2.UnitY);                

                // handles keeping legs from moving when the player is in flight/moving fast/channeling.
                if (player.velocity.X > 0)
                {
                    player.legFrameCounter = -player.velocity.X;
                } else
                {
                    player.legFrameCounter = player.velocity.X;
                }                

                //calculate rotation
                float radRot = GetPlayerFlightRotation(mRotationDir, player);

                player.fullRotation = MathHelper.Lerp(player.fullRotation, radRot, 0.1f);
            }

            // altered to only fire once, the moment you exit flight, to avoid overburden of sync packets when moving normally.
            if (!modPlayer.isFlying)
            {
                player.fullRotation = MathHelper.Lerp(player.fullRotation, 0, 0.1f);
            }
        }

        public static Tuple<int, float> GetFlightFacingDirectionAndPitchDirection(MyPlayer modPlayer)
        {
            int octantDirection = 0;
            int octantPitch = 0;
            // since the player is mirrored, there's really only 3 ordinal positions we care about
            // up angle, no angle and down angle
            // we don't go straight up or down cos it looks weird as shit
            switch (modPlayer.mouseWorldOctant)
            {
                case -3:
                case -2:
                case -1:
                    octantPitch = -1;
                    break;
                case 0:
                case 4:
                    octantPitch = 0;
                    break;
                case 1:
                case 2:
                case 3:
                    octantPitch = 1;
                    break;
            }

            // for direction we have to do things a bit different.
            Vector2 mouseVector = modPlayer.GetMouseVectorOrDefault();
            if (mouseVector == Vector2.Zero)
            {
                // we're probably trying to run direction on a player who isn't ours, don't do this. They can control their own dir.
                octantDirection = 0;
            }
            else
            {
                octantDirection = mouseVector.X < 0 ? -1 : 1;
            }

            return new Tuple<int, float>(octantDirection, octantPitch * 45f);
        }

        public static float GetPlayerFlightRotation(Vector2 mRotationDir, Player player)
        {
            float radRot = 0f;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            float leanThrottle = 180;
            // make sure if the player is using a ki weapon during flight, we're facing a way that doesn't make it look extremely goofy
            if (modPlayer.isPlayerUsingKiWeapon)
            {
                var directionInfo = GetFlightFacingDirectionAndPitchDirection(modPlayer);
                // get flight rotation from octant
                var octantDirection = directionInfo.Item1;
                leanThrottle = directionInfo.Item2;

                int dir = octantDirection;
                if (dir != player.direction && player.whoAmI == Main.myPlayer)
                {                    
                    player.ChangeDir(dir);
                }                
            }

            if (modPlayer.isPlayerUsingKiWeapon)
            {
                // we already got the lean throttle from above, and set the direction we needed to not look stupid
                if (player.direction == 1)
                    radRot = MathHelper.ToRadians(leanThrottle);
                else if (player.direction == -1)
                    radRot = MathHelper.ToRadians(-leanThrottle);
            }
            else if (mRotationDir != Vector2.Zero)
            {
                mRotationDir.Normalize();
                radRot = (float)Math.Atan((mRotationDir.X / mRotationDir.Y));
                if (mRotationDir.Y < 0)
                {
                    if (mRotationDir.X > 0)
                        radRot += MathHelper.ToRadians(leanThrottle);
                    else if (mRotationDir.X < 0)
                        radRot -= MathHelper.ToRadians(leanThrottle);
                    else
                    {
                        if (player.velocity.X >= 0)
                            radRot = MathHelper.ToRadians(leanThrottle);
                        else if (player.velocity.X < 0)
                            radRot = MathHelper.ToRadians(-leanThrottle);
                    }
                }
            }

            return radRot;
        }

        public static void AddKatchinFeetBuff(Player player)
        {
            // reset the player fall position here, even if they don't have flight dampening.
            player.fallStart = (int)(player.position.Y / 16f);
            if (MyPlayer.ModPlayer(player).flightDampeningUnlocked)
            {
                Mod mod = ModLoader.GetMod("DBZMOD");
                player.AddBuff(mod.BuffType("KatchinFeet"), 600);
            }
        }

        public static void SpawnFlightDust(Player thePlayer, float boostSpeed, int flightDustType, float scale)
        {
            for (int i = 0; i < (boostSpeed == 0 ? 2 : 10); i++)
            {
                Dust tdust = Dust.NewDustDirect(thePlayer.position - (Vector2.UnitY * 0.7f) - (Vector2.UnitX * 3.5f), 30, 30, flightDustType, 0f, 0f, 0, new Color(255, 255, 255), scale);
                tdust.noGravity = true;
            }
        }
    }
}

