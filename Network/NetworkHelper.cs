using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace Network
{
    internal class NetworkHelper
    { 
        public const byte TransformationHandler = 1;
        public const byte FlightHandler = 2;
        public const byte FlightAuraHandler = 3;

        internal static TransformationPacketHandler formSync = new TransformationPacketHandler(TransformationHandler);
        internal static FlightPacketHandler flightSync = new FlightPacketHandler(FlightHandler);
        internal static FlightAuraPacketHandler flightAuraSync = new FlightAuraPacketHandler(FlightAuraHandler);
        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            switch (r.ReadByte())
            {
                case TransformationHandler:
                    formSync.HandlePacket(r, fromWho);
                    break;
                case FlightHandler:
                    flightSync.HandlePacket(r, fromWho);
                    break;
                case FlightAuraHandler:
                    flightAuraSync.HandlePacket(r, fromWho);
                    break;
            }
        }
    }
}
