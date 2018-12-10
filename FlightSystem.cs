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
using Util;
using Network;

namespace DBZMOD
{
    public class FlightSystem
    {
        //constants
        const int FLIGHT_KI_DRAIN = 4;
        public const float BURST_SPEED = 0.5f;
        const float FLIGHT_SPEED = 0.3f;

        Vector2 m_currentVel = new Vector2(0, 0);
        private int FLIGHT_KI_DRAIN_TIMER = 0;

        private int FlightDustType = 261;
       
        public void Update(Player player)
        {            
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);           

            //check for ki or death lol
            if ((modPlayer.IsKiDepleted() || player.dead) && modPlayer.IsFlying)
            {
                modPlayer.IsFlying = false;                
            }

            if (modPlayer.IsFlying)
            {
                // cancel platform collision
                player.DryCollision(true, true);

                //prepare vals
                player.fullRotationOrigin = new Vector2(11, 22);
                Vector2 m_rotationDir = Vector2.Zero;

                //m_targetRotation = 0;

                //Input checks
                float boostSpeed = (BURST_SPEED) * (modPlayer.IsCharging ? 1 : 0);
                int totalFlightUsage = FLIGHT_KI_DRAIN - modPlayer.FlightUsageAdd;
                float totalFlightSpeed = FLIGHT_SPEED + boostSpeed + (player.moveSpeed / 3) + modPlayer.FlightSpeedAdd;

                if (modPlayer.IsUpHeld)
                {
                    m_currentVel.Y -= totalFlightSpeed;
                    m_rotationDir = Vector2.UnitY;
                }
                else if (modPlayer.IsDownHeld)
                {
                    m_currentVel.Y += totalFlightSpeed;
                    m_rotationDir = -Vector2.UnitY;
                }

                if (modPlayer.IsRightHeld)
                {
                    m_currentVel.X += totalFlightSpeed;
                    m_rotationDir += Vector2.UnitX;
                }
                else if (modPlayer.IsLeftHeld)
                {
                    m_currentVel.X -= totalFlightSpeed;
                    m_rotationDir -= Vector2.UnitX;
                }

                if (m_currentVel.Length() > 0.5f)
                {
                    SpawnFlightDust(player, boostSpeed, FlightDustType, 0f);
                }

                if (Transformations.IsSSJ(player) && !Transformations.IsGodlike(player))
                {
                    FlightDustType = 170;
                }
                else if (Transformations.IsLSSJ(player))
                {
                    FlightDustType = 107;
                }
                else if (Transformations.IsGodlike(player))
                {
                    FlightDustType = 174;
                }
                else if (Transformations.IsKaioken(player) || Transformations.IsSSJ1Kaioken(player))
                {
                    FlightDustType = 182;
                }
                else
                {
                    FlightDustType = 267;
                }

                //caluclate velocity
                player.velocity = m_currentVel - (Vector2.UnitY * 0.4f);
                m_currentVel.X = MathHelper.Lerp(m_currentVel.X, 0, 0.1f);
                m_currentVel.Y = MathHelper.Lerp(m_currentVel.Y, 0, 0.1f);

                //calculate rotation
                float radRot = 0;
                if (m_rotationDir != Vector2.Zero)
                {
                    m_rotationDir.Normalize();
                    radRot = (float)Math.Atan((m_rotationDir.X / m_rotationDir.Y));

                    if (m_rotationDir.Y < 0)
                    {
                        if (m_rotationDir.X > 0)
                            radRot += MathHelper.ToRadians(180);
                        else if (m_rotationDir.X < 0)
                            radRot -= MathHelper.ToRadians(180);
                        else
                        {
                            if (m_currentVel.X > 0)
                                radRot = MathHelper.ToRadians(180);
                            else if (m_currentVel.X < 0)
                                radRot = MathHelper.ToRadians(-180);
                        }

                    }
                }
                player.fullRotation = MathHelper.Lerp(player.fullRotation, radRot, 0.1f);
                FLIGHT_KI_DRAIN_TIMER++;
                //drain ki
                if (!modPlayer.flightUpgraded)
                {
                    if (FLIGHT_KI_DRAIN_TIMER >= 1)
                    {
                        modPlayer.AddKi((totalFlightUsage + (totalFlightUsage * (int)boostSpeed)) * -1);
                        FLIGHT_KI_DRAIN_TIMER = 0;
                    }
                }
                else if (modPlayer.flightUpgraded)
                {
                    if (FLIGHT_KI_DRAIN_TIMER >= 3)
                    {
                        modPlayer.AddKi(-1);
                        FLIGHT_KI_DRAIN_TIMER = 0;
                    }
                }
                if (totalFlightUsage < 1)
                {
                    totalFlightUsage = 1;
                }

                if (player.velocity.Y == -0.4f && Math.Abs(player.velocity.X) > 0.5f)
                {
                    // if the player is "running", stop that. EXPERIMENTAL
                    player.velocity.Y = -0.4001f;                    
                }
            }

            // altered to only fire once, the moment you exit flight, to avoid overburden of sync packets when moving normally.
            if (!modPlayer.IsFlying)
            {                
                player.fullRotation = MathHelper.ToRadians(0);
                AddKatchinFeetBuff(player);
            }
        }

        public void AddKatchinFeetBuff(Player player)
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

