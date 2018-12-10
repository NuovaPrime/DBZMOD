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
        public const byte FlightMovementHandler = 2;
        public const byte PlayerHandler = 3;

        internal static TransformationPacketHandler formSync = new TransformationPacketHandler(TransformationHandler);
        internal static PlayerPacketHandler playerSync = new PlayerPacketHandler(PlayerHandler);

        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            switch (r.ReadByte())
            {
                case TransformationHandler:
                    formSync.HandlePacket(r, fromWho);
                    break;
                case PlayerHandler:
                    playerSync.HandlePacket(r, fromWho);
                    break;
            }
        }
    }
}
