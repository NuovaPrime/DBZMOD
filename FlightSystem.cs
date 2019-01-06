using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using DBZMOD.Util;
using DBZMOD.Projectiles;

namespace DBZMOD
{
    public class FlightSystem
    {

        //constants
        const int FLIGHT_KI_DRAIN = 4;
        const float BURST_SPEED = 0.5f;
        const float FLIGHT_SPEED = 0.3f;

        public static void Update(Player player)
        {
            // this might seem weird but the server isn't allowed to control the flight system.
            if (Main.netMode == NetmodeID.Server)
                return;

            MyPlayer modPlayer = MyPlayer.ModPlayer(player);

            //check for ki or death lol
            if ((modPlayer.IsKiDepleted() || player.dead || player.mount.Type != -1 || player.ropeCount != 0) && modPlayer.IsFlying)
            {
                modPlayer.IsFlying = false;
                AddKatchinFeetBuff(player);
            }

            if (modPlayer.IsFlying)
            {
                // cancel platform collision
                player.DryCollision(true, true);

                //prepare vals
                player.fullRotationOrigin = new Vector2(11, 22);
                Vector2 m_rotationDir = Vector2.Zero;

                int FlightDustType = 261;

                //Input checks
                float boostSpeed = (BURST_SPEED) * (modPlayer.IsCharging ? 1 : 0);
                int totalFlightUsage = Math.Max(1, FLIGHT_KI_DRAIN - modPlayer.FlightUsageAdd);
                float totalHorizontalFlightSpeed = FLIGHT_SPEED + boostSpeed + (player.moveSpeed / 3) + modPlayer.FlightSpeedAdd;
                float totalVerticalFlightSpeed = FLIGHT_SPEED + boostSpeed + (Player.jumpSpeed / 2) + modPlayer.FlightSpeedAdd;

                if (modPlayer.IsUpHeld)
                {
                    // for some reason flying up is way, way faster than flying down.
                    player.velocity.Y -= (totalVerticalFlightSpeed / 3.8f);
                    m_rotationDir = Vector2.UnitY;
                }
                else if (modPlayer.IsDownHeld)
                {
                    player.maxFallSpeed = 20f;
                    player.velocity.Y += totalVerticalFlightSpeed / 3.6f;
                    m_rotationDir = -Vector2.UnitY;
                }

                if (modPlayer.IsRightHeld)
                {
                    player.velocity.X += totalHorizontalFlightSpeed;
                    m_rotationDir += Vector2.UnitX;
                }
                else if (modPlayer.IsLeftHeld)
                {
                    player.velocity.X -= totalHorizontalFlightSpeed;
                    m_rotationDir -= Vector2.UnitX;
                }

                if (player.velocity.Length() > 0.5f)
                {
                    SpawnFlightDust(player, boostSpeed, FlightDustType, 0f);
                }

                if (Transformations.IsSSJ(player) && !Transformations.IsSSJG(player))
                {
                    FlightDustType = 170;
                }
                else if (Transformations.IsLSSJ(player))
                {
                    FlightDustType = 107;
                }
                else if (Transformations.IsSSJG(player))
                {
                    FlightDustType = 174;
                }
                else if (Transformations.IsAnyKaioken(player))
                {
                    FlightDustType = 182;
                }
                else
                {
                    FlightDustType = 267;
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
                float radRot = GetPlayerFlightRotation(m_rotationDir, player);

                player.fullRotation = MathHelper.Lerp(player.fullRotation, radRot, 0.1f);

                //drain ki
                if (!modPlayer.flightUpgraded)
                {
                    if (Main.time > 0 && Main.time % 2 == 0)
                    {
                        modPlayer.AddKi((totalFlightUsage + (totalFlightUsage * (int)boostSpeed)) * -1, false, false);                        
                    }
                }
                else
                {
                    if (Main.time > 0 && Main.time % 4 == 0)
                    {
                        modPlayer.AddKi(-1, false, false);                        
                    }
                }
            }

            // altered to only fire once, the moment you exit flight, to avoid overburden of sync packets when moving normally.
            if (!modPlayer.IsFlying)
            {
                player.fullRotation = MathHelper.Lerp(player.fullRotation, 0, 0.1f);
            }
        }

        public static bool IsPlayerUsingKiWeapon(MyPlayer modPlayer)
        {
            // try to figure out if the player is using a beam weapon actively.
            // the reason we have to do this is because the IsMouseLeftHeld call isn't quite on time
            // for the beam routine updates - we need to know if the charge ball is firing earlier than that.
            bool isExistingBeamFiring = false;
            if (modPlayer.player.heldProj > 0)
            {
                var proj = Main.projectile[modPlayer.player.heldProj];
                if (proj.modProjectile != null && proj.modProjectile is BaseBeamCharge)
                {
                    var beamCharge = proj.modProjectile as BaseBeamCharge;
                    if (beamCharge.IsSustainingFire)
                    {
                        isExistingBeamFiring = true;
                    }
                }
            }
            return modPlayer.IsHoldingKiWeapon && ((modPlayer.IsMouseLeftHeld && isExistingBeamFiring) || modPlayer.IsMouseRightHeld);
        }

        public static Tuple<int, float> GetFlightFacingDirectionAndPitchDirection(MyPlayer modPlayer)
        {
            int octantDirection = 0;
            int octantPitch = 0;
            // since the player is mirrored, there's really only 3 ordinal positions we care about
            // up angle, no angle and down angle
            // we don't go straight up or down cos it looks weird as shit
            switch (modPlayer.MouseWorldOctant)
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

        public static float GetPlayerFlightRotation(Vector2 m_rotationDir, Player player)
        {
            float radRot = 0f;

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            float leanThrottle = 180;
            // make sure if the player is using a ki weapon during flight, we're facing a way that doesn't make it look extremely goofy
            if (IsPlayerUsingKiWeapon(modPlayer))
            {
                var directionInfo = GetFlightFacingDirectionAndPitchDirection(modPlayer);
                // get flight rotation from octant
                var octantDirection = directionInfo.Item1;
                leanThrottle = directionInfo.Item2;

                bool isPlayerHorizontal = m_rotationDir.X != 0;
                bool isMouseAbove = leanThrottle < 0;
                int dir = octantDirection;
                if (dir != player.direction && player.whoAmI == Main.myPlayer)
                {
                    DebugUtil.Log(string.Format("Player direction {0} should be {1}", player.direction, dir));
                    player.ChangeDir(dir);
                }                
            }

            if (IsPlayerUsingKiWeapon(modPlayer))
            {
                // we already got the lean throttle from above, and set the direction we needed to not look stupid
                if (player.direction == 1)
                    radRot = MathHelper.ToRadians(leanThrottle);
                else if (player.direction == -1)
                    radRot = MathHelper.ToRadians(-leanThrottle);
            }
            else if (m_rotationDir != Vector2.Zero)
            {
                m_rotationDir.Normalize();
                radRot = (float)Math.Atan((m_rotationDir.X / m_rotationDir.Y));
                if (m_rotationDir.Y < 0)
                {
                    if (m_rotationDir.X > 0)
                        radRot += MathHelper.ToRadians(leanThrottle);
                    else if (m_rotationDir.X < 0)
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

