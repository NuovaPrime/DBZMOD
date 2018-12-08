using DBZMOD;
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
    internal class FlightMovementPacketHandler : PacketHandler
    {
        public const byte SyncFlight = 1;

        public FlightMovementPacketHandler(byte handlerType) : base(handlerType)
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

        public void SendFlightChanges(int toWho, int fromWho, int whichPlayer, float positionX, float positionY, float velocityX, float velocityY, float rotation, float rotationOriginX, float rotationOriginY, int flightDustType, float boostSpeed)
        {
            ModPacket packet = GetPacket(SyncFlight, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(positionX);
            packet.Write(positionY);
            packet.Write(velocityX);
            packet.Write(velocityY);
            packet.Write(rotation);
            packet.Write(rotationOriginX);
            packet.Write(rotationOriginY);
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
            float rotationOriginX = reader.ReadSingle();
            float rotationOriginY = reader.ReadSingle();
            int flightDustType = reader.ReadInt32();
            float boostSpeed = reader.ReadSingle();
            if (Main.netMode == NetmodeID.Server)
            {
                SendFlightChanges(-1, fromWho, whichPlayer, positionX, positionY, velocityX, velocityY, rotation, rotationOriginX, rotationOriginY, flightDustType, boostSpeed);
            }
            else
            {
                Player thePlayer = Main.player[whichPlayer];
                thePlayer.position.X = positionX;
                thePlayer.position.Y = positionY;
                thePlayer.velocity.X = velocityX;
                thePlayer.velocity.Y = velocityY;
                thePlayer.fullRotation = rotation;
                thePlayer.fullRotationOrigin = new Vector2(rotationOriginX, rotationOriginY);
                // fake dust spawn, checks if the player is kinda stationary, if not, spray some dust, who cares.
                if (Math.Abs(velocityX) > 4f || Math.Abs(velocityY) > 4f)
                {
                    FlightSystem.SpawnFlightDust(thePlayer, boostSpeed, flightDustType, 0f);
                }
            }
        }
    }
}
