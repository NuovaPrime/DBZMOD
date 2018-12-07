using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Network
{
    internal class FlightPacketHandler : PacketHandler
    {
        public const byte SyncFlight = 1;

        public FlightPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncFlight):
                    ReceiveFlightChanges(reader, fromWho);
                    break;
            }
        }

        public void SendFlightChanges(int toWho, int fromWho, int whichPlayer, float positionX, float positionY, float velocityX, float velocityY, float rotation, int flightDustType, float boostSpeed)
        {
            ModPacket packet = GetPacket(SyncFlight, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(positionX);
            packet.Write(positionY);
            packet.Write(velocityX);
            packet.Write(velocityY);
            packet.Write(rotation);
            packet.Write(flightDustType);
            packet.Write(boostSpeed);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveFlightChanges(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float velocityX = reader.ReadSingle();
            float velocityY = reader.ReadSingle();
            float rotation = reader.ReadSingle();
            int flightDustType = reader.ReadInt32();
            float boostSpeed = reader.ReadSingle();
            if (Main.netMode == NetmodeID.Server)
            {
                SendFlightChanges(-1, fromWho, whichPlayer, positionX, positionY, velocityX, velocityY, rotation, flightDustType, boostSpeed);
            }
            else
            {
                Player thePlayer = Main.player[whichPlayer];
                thePlayer.position.X = positionX;
                thePlayer.position.Y = positionY;
                thePlayer.velocity.X = velocityX;
                thePlayer.velocity.Y = velocityY;
                thePlayer.fullRotation = rotation;
                for (int i = 0; i < (boostSpeed == 0 ? 2 : 10); i++)
                {
                    Dust tdust = Dust.NewDustDirect(thePlayer.position - (Vector2.UnitY * 0.7f) - (Vector2.UnitX * 3.5f), 30, 30, flightDustType, 0f, 0f, 0, new Color(255, 255, 255), 1.5f);
                    tdust.noGravity = true;
                }
            }
        }
    }
}
