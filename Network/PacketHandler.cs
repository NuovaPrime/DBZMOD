using System.IO;
using Terraria.ModLoader;

namespace DBZMOD.Network
{
    internal abstract class PacketHandler
    {
        internal byte HandlerType { get; set; }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        protected PacketHandler(byte handlerType)
        {
            HandlerType = handlerType;
        }

        protected ModPacket GetPacket(byte packetType, int fromWho)
        {
            var p = global::DBZMOD.DBZMOD.instance.GetPacket();
            p.Write(HandlerType);
            p.Write(packetType);
            // this seems to fuck shit up and I don't understand why blushie put it in the example.
            //if (Main.netMode == NetmodeID.Server)
            //{
            //    p.Write((byte)fromWho);
            //}
            return p;
        }
    }
}
