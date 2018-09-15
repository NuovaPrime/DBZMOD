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
        bool m_FlightMode = false;
        const float FLIGHT_SPEED = 0.5f;
        Vector2 currentVel = new Vector2(0, 0);
        const float FLIGHT_KI_DRAIN = 10;

        public void ToggleFlight()
        {
            m_FlightMode = !m_FlightMode;
        }
       
        public void Update(TriggersSet triggersSet, Player player)
        {
            if (m_FlightMode && MyPlayer.ModPlayer(player).KiCurrent > 0)
            {
                if (triggersSet.Up)
                {
                    currentVel.Y -= FLIGHT_SPEED;
                    player.fullRotation = MathHelper.ToRadians(0);
                }
                if (triggersSet.Down)
                {
                    currentVel.Y += FLIGHT_SPEED;
                    player.fullRotation = MathHelper.ToRadians(180);
                }
                if (triggersSet.Right)
                {
                    currentVel.X += FLIGHT_SPEED;
                    player.fullRotation = MathHelper.ToRadians(90);
                }
                if (triggersSet.Left)
                {
                    currentVel.X -= FLIGHT_SPEED;
                    player.fullRotation = MathHelper.ToRadians(270);
                }

                if (currentVel.Length() > 0.5f)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust tDust = Dust.NewDustDirect(player.position - (Vector2.UnitY * 0.7f) - (Vector2.UnitX*1.0f), 50, 50, 15, 0f, 0f, 5, default(Color), 2.0f);
                        tDust.noGravity = true;
                    }
                }

                player.velocity = currentVel - (Vector2.UnitY * 0.4f);

                currentVel.X = MathHelper.Lerp(currentVel.X, 0, 0.1f);
                currentVel.Y = MathHelper.Lerp(currentVel.Y, 0, 0.1f);

                MyPlayer.ModPlayer(player).KiCurrent -= (int)FLIGHT_KI_DRAIN;
            }
        }

    }
}

