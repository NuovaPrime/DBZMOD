using System.IO;

namespace DBZMOD.Network
{
    internal class NetworkHelper
    { 
        public const byte TRANSFORMATION_HANDLER = 1;
        public const byte FLIGHT_MOVEMENT_HANDLER = 2;
        public const byte PLAYER_HANDLER = 3;

        internal static TransformationPacketHandler formSync = new TransformationPacketHandler(TRANSFORMATION_HANDLER);
        internal static PlayerPacketHandler playerSync = new PlayerPacketHandler(PLAYER_HANDLER);

        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            switch (r.ReadByte())
            {
                case TRANSFORMATION_HANDLER:
                    formSync.HandlePacket(r, fromWho);
                    break;
                case PLAYER_HANDLER:
                    playerSync.HandlePacket(r, fromWho);
                    break;
            }
        }
    }
}
