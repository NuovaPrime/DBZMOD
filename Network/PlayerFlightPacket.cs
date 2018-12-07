using DBZMOD;
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

        public void SendFlightChanges(int toWho, int fromWho, int whichPlayer, bool isFlying)
        {
            ModPacket packet = GetPacket(SyncFlight, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(isFlying);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveFlightChanges(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            bool isFlying = reader.ReadBoolean();            
            if (Main.netMode == NetmodeID.Server)
            {
                SendFlightChanges(-1, fromWho, whichPlayer, isFlying);
            }
            else
            {
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().IsFlying = isFlying;                
            }
        }
    }
}
