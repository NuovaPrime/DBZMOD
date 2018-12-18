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
        const float BURST_SPEED = 0.5f;
        const float FLIGHT_SPEED = 0.3f;

        public static void Update(Player player)
        {
            // this might seem weird but the server isn't allowed to control the flight system.
            if (Main.netMode == NetmodeID.Server)
                return;

            MyPlayer modPlayer = MyPlayer.ModPlayer(player);

            //if (Math.Ceiling(Main.time % 60) == 0)
            //DebugUtil.Log(string.Format("Update flight firing for player {0} whose controls are Up {1} Down {2} Left {3} Right {4}", player.whoAmI, modPlayer.IsUpHeld, modPlayer.IsDownHeld, modPlayer.IsLeftHeld, modPlayer.IsRightHeld));

            //check for ki or death lol
            if ((modPlayer.IsKiDepleted() || player.dead) && modPlayer.IsFlying)
            {
                // DebugUtil.Log(string.Format("Disabling flying on player {0} because ki is depleted or player is dead? Player was flying.", player.whoAmI));
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
                float totalFlightSpeed = FLIGHT_SPEED + boostSpeed + (player.moveSpeed / 3) + modPlayer.FlightSpeedAdd;

                if (modPlayer.IsUpHeld)
                {
                    //DebugUtil.Log(string.Format("Player {0} is moving because my client thinks their button is pushed!", modPlayer.player.whoAmI));
                    player.velocity.Y -= totalFlightSpeed;
                    m_rotationDir = Vector2.UnitY;
                }
                else if (modPlayer.IsDownHeld)
                {
                    //DebugUtil.Log(string.Format("Player {0} is moving because my client thinks their button is pushed!", modPlayer.player.whoAmI));
                    player.velocity.Y += totalFlightSpeed;
                    m_rotationDir = -Vector2.UnitY;
                }

                if (modPlayer.IsRightHeld)
                {
                    //DebugUtil.Log(string.Format("Player {0} is moving because my client thinks their button is pushed!", modPlayer.player.whoAmI));
                    player.velocity.X += totalFlightSpeed;
                    m_rotationDir += Vector2.UnitX;
                }
                else if (modPlayer.IsLeftHeld)
                {
                    //DebugUtil.Log(string.Format("Player {0} is moving because my client thinks their button is pushed!", modPlayer.player.whoAmI));
                    player.velocity.X -= totalFlightSpeed;
                    m_rotationDir -= Vector2.UnitX;
                }

                if (player.velocity.Length() > 0.5f)
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

                //calculate velocity
                player.velocity.X = MathHelper.Lerp(player.velocity.X, 0, 0.1f);
                player.velocity.Y = MathHelper.Lerp(player.velocity.Y, 0, 0.1f);
                // keep the player suspended at worst.
                // player.gravDir = 0;
                player.velocity = player.velocity - (Vector2.UnitY * player.gravity);
                player.fallStart = (int)(player.position.Y / 16f);
                
                // DebugUtil.Log(string.Format("Player gravity {0} gravDir {1}", player.gravity, player.gravDir));

                //calculate rotation
                float radRot = 0;
                // DebugUtil.Log(string.Format("m_rotation is X: {0} Y: {1}", m_rotationDir.X, m_rotationDir.Y));
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
                            if (player.velocity.X >= 0)
                                radRot = MathHelper.ToRadians(180);
                            else if (player.velocity.X < 0)
                                radRot = MathHelper.ToRadians(-180);
                        }
                    }
                }
                player.fullRotation = MathHelper.Lerp(player.fullRotation, radRot, 0.1f);

                //drain ki
                if (!modPlayer.flightUpgraded)
                {
                    if (Main.time > 0 && Main.time % 2 == 0)
                    {
                        modPlayer.AddKi((totalFlightUsage + (totalFlightUsage * (int)boostSpeed)) * -1);                        
                    }
                }
                else
                {
                    if (Main.time > 0 && Main.time % 4 == 0)
                    {
                        modPlayer.AddKi(-1);                        
                    }
                }
            }

            // altered to only fire once, the moment you exit flight, to avoid overburden of sync packets when moving normally.
            if (!modPlayer.IsFlying)
            {                
                player.fullRotation = MathHelper.ToRadians(0);
            }
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

