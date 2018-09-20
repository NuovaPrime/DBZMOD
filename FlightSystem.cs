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

namespace DBZMOD
{
    public class FlightSystem
    {
        //constants
        const int FLIGHT_KI_DRAIN = 4;
        public const float BURST_SPEED = 0.5f;
        const float FLIGHT_SPEED = 0.3f;

        bool m_FlightMode = false;
        Vector2 m_currentVel = new Vector2(0, 0);
        //float m_targetRotation = 0.0f;

        public void ToggleFlight()
        {
            m_FlightMode = !m_FlightMode;
        }
       
        public void Update(TriggersSet triggersSet, Player player)
        {
            if (m_FlightMode)
            {
                //check for ki
                if (MyPlayer.ModPlayer(player).KiCurrent <= 0)
                {
                    m_FlightMode = false;
                    player.fullRotation = MathHelper.ToRadians(0);
                    return;
                }

                //prepare vals
                player.fullRotationOrigin = new Vector2(11, 22);
                MyPlayer.ModPlayer(player).IsFlying = true;
                Vector2 m_rotationDir = Vector2.Zero;

                //m_targetRotation = 0;

                //Input checks
                float boostSpeed = (BURST_SPEED) * (MyPlayer.EnergyCharge.Current? 1 : 0);
                float totalFlightUsage = FLIGHT_KI_DRAIN - MyPlayer.ModPlayer(player).FlightUsageAdd;
                float totalFlightSpeed = FLIGHT_SPEED + boostSpeed + (player.moveSpeed / 3) + MyPlayer.ModPlayer(player).FlightSpeedAdd;

                if (triggersSet.Up)
                {
                    m_currentVel.Y -= totalFlightSpeed;
                    m_rotationDir = Vector2.UnitY;
                }
                else if (triggersSet.Down)
                {
                    m_currentVel.Y += totalFlightSpeed;
                    m_rotationDir = -Vector2.UnitY;
                }

                if (triggersSet.Right)
                {
                    m_currentVel.X += totalFlightSpeed;
                    m_rotationDir += Vector2.UnitX;
                }
                else if (triggersSet.Left)
                {
                    m_currentVel.X -= totalFlightSpeed;
                    m_rotationDir -= Vector2.UnitX;
                }

                if (m_currentVel.Length() > 0.5f)
                {
                    if(boostSpeed == 0) //not boosting?
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            Dust tDust = Dust.NewDustDirect(player.position - (Vector2.UnitY * 0.7f) - (Vector2.UnitX * 3.5f), 50, 50, 15, 0f, 0f, 0, new Color(0, 0, 0, 0.1f), 1f);
                            tDust.noGravity = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Dust tDust = Dust.NewDustDirect(player.position - (Vector2.UnitY * 0.7f) - (Vector2.UnitX * 3.5f), 50, 50, 15, 0f, 0f, 0, new Color(0, 0, 0, 0.1f), 1f);
                            tDust.noGravity = true;
                        }
                    }

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

                //drain ki
                MyPlayer.ModPlayer(player).KiCurrent -= FLIGHT_KI_DRAIN + (FLIGHT_KI_DRAIN * (int)boostSpeed);
            }
            else //no longer flying cuz of mode change or ki ran out
            {
                player.fullRotation = MathHelper.ToRadians(0);
                MyPlayer.ModPlayer(player).IsFlying = false;
            }
        }

    }
}

